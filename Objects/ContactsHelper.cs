using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BcmMigrationTool;
using BCM_Migration_Tool.Properties;
using Newtonsoft.Json;
using TracerX;

namespace BCM_Migration_Tool.Objects
{
    internal class ContactsHelper : HelperBase
    {
        private static readonly Logger Log = Logger.GetLogger("ContactsHelper");
        #region Constructors
        //internal ContactsHelper()
        //{}
        internal ContactsHelper(string accessToken, string connectionString)
            : base(accessToken, connectionString)
        {
            
        }
        #endregion
        #region Properties
        //internal List<ContactFullView> BCMContacts { get; set; }
        internal OCMCompanyTemplate.Rootobject ContactTemplate { get; set; }
        private string LastPageResult { get; set; }
        internal static List<OCMContact2Value> OCMContacts { get; set; }        
        #endregion
        #region Methods
        private async Task<bool> CreateCompanyLink(string itemLinkID, string companyXrmID)
        {
            using (Log.VerboseCall())
            {
                //string xmlRequest = Resources.CreateXrmGraphRelationshipRequest;
                string xmlRequest = Resources.CreateXrmGraphRelationshipRequest2;

                Uri uri = new Uri(Settings.Default.EWSEndPoint);

                //xmlRequest = xmlRequest.Replace("{FROMENTITYID}", companyXrmID);
                //xmlRequest = xmlRequest.Replace("{FROMENTITYTYPE}", "XrmOrganization");
                //xmlRequest = xmlRequest.Replace("{TOENTITYID}", itemLinkID);
                //xmlRequest = xmlRequest.Replace("{TOENTITYTYPE}", "Person");
                //xmlRequest = xmlRequest.Replace("{LINKTYPE}", "WorksFor");
                xmlRequest = String.Format(xmlRequest, companyXrmID, "XrmOrganization", itemLinkID, "Person", "WorksFor");
            
                try
                {
                    //Log.VerboseFormat("Posting to {1}:{2}{0}{2}", xmlRequest, uri, Environment.NewLine);
                    if (FullRESTLogging)
                        Log.VerboseFormat("EWS query: {0}", xmlRequest);
                        PrepareRequest(RequestDataTypes.Links, RequestDataFormats.XML);
                    //RestResponse<HttpWebResponse> response = await Post<HttpWebResponse>(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml"), null, true, "", true, false);
                    RestResponse<HttpWebResponse> response = await Post<HttpWebResponse>(uri, xmlRequest, RequestDataFormats.XML, true, "", true, false);

                    if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
                    {
                        Log.Error(response.ErrorContent);
                        OnError(null, new HelperEventArgs(String.Format("ERROR '{1}': {0}", response.ErrorContent, response.StatusCode), HelperEventArgs.EventTypes.Error));
                        NumberOfErrors += 1;
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                    //using (var response = await _httpClient.PostAsync(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml")))
                    //{
                    //    var content = await response.Content.ReadAsStringAsync();

                    //    if (!response.IsSuccessStatusCode)
                    //    {
                    //        Log.Error(content);
                    //        OnError(null, new HelperEventArgs(String.Format("ERROR: {0}", content), HelperEventArgs.EventTypes.Error));
                    //        NumberOfErrors += 1;
                    //        return false;
                    //    }
                    //    else
                    //    {
                    //        return true;
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return false;
                }
            }
        }
        internal async Task CreateCompanyLinks()
        {
            using (Log.VerboseCall())
            {
                //TEST Build filtered collection from OCMContacts with those that only have ItemLinkID values
                //NumberToProcess = OCMContacts.Count;
                //Log.InfoFormat("Linking {0} contacts", OCMContacts.Count);

                try
                {
                    //var filteredOCMContacts = OCMContacts.Where(contacts => !String.IsNullOrEmpty(contacts.ItemLinkId));
                    var filteredOCMContacts = OCMContacts.Where(contacts => !String.IsNullOrEmpty(contacts.ItemLinkId) && contacts.Linked == false);
                    var filteredOcmContacts = filteredOCMContacts as IList<OCMContact2Value> ?? filteredOCMContacts.ToList();
                    NumberToProcess = filteredOcmContacts.Count();
                    Log.InfoFormat("Linking {0} contacts", NumberToProcess);

                    OnStart(null, new HelperEventArgs("Linking contacts...", HelperEventArgs.EventTypes.Status));

                    int linkCount = 0;

                    foreach (var contact in filteredOcmContacts)
                    {
                        contact.Linked = true; //Set to true to skip Contacts previously processed in the current migration session
                        linkCount++;
                        try
                        {
                            if (contact.CompanyName == null || String.IsNullOrEmpty(contact.CompanyName.ToString()))
                            {
                                Log.VerboseFormat("No company for Contact '{0}')", contact.DisplayName);
                                continue;
                            }

                            if (!String.IsNullOrEmpty(contact.ItemLinkId))
                            {
                                OCMCompany2Value company = AccountsHelper.GetOCMCompany(contact.CompanyName?.ToString(),
                                    UpdateKeyTypes.Name);
                                if (company == null)
                                {
                                    Log.WarnFormat("Could not retrieve company '{0}' from internal collection for contact '{1}'", contact.CompanyName, contact.DisplayName);
                                    continue;
                                }
                                bool existingLink = false;

                                if (company.InlineLinks?.Relationships != null)
                                {
                                    Log.VerboseFormat("{3} Relationships for contact '{0}' (ItemLinkID: {1}; XrmID: {2})", contact.DisplayName, contact.ItemLinkId, company.XrmId, company.InlineLinks?.Relationships.Length);

                                    foreach (var link in company.InlineLinks?.Relationships)
                                    {
                                        if (link.ItemLinkId == contact.ItemLinkId)
                                        {
                                            existingLink = true;
                                        }
                                        if (link.DisplayName == contact.DisplayName)
                                        {
                                            existingLink = true;
                                        }
                                    }
                                }

                                OnIncrementProgress(null, new HelperEventArgs("", HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Migrating));

                                if (existingLink || String.IsNullOrEmpty(company.XrmId))
                                {
                                    Log.VerboseFormat("Existing link for contact '{0}' (ItemLinkID: {1}; XrmID: {2}); skipping", contact.DisplayName, contact.ItemLinkId, company.XrmId);
                                    continue;
                                }

                                //HIGH BUG Seen a contact with an ID doubled in ItemLinkId; check for length, or take only the first x chars (eg. 0d4723e8-7137-46ca-be8a-e9b3f6f7dc9610d4723e8-7137-46ca-be8a-e9b3f6f7dc96)
                                //Use 0d4723e8-7137-46ca-be8a-e9b3f6f7dc96 as reference
                                bool linked = await CreateCompanyLink(contact.ItemLinkId, company.XrmId);
                                if (linked)
                                {
                                    Log.DebugFormat("Contact '{0}' linked to company '{1}'", contact.DisplayName, contact.CompanyName);
                                    OnDisplayMessage(null,
                                        new HelperEventArgs(
                                            String.Format("Contact '{0}' linked to company '{1}' ({2}/{3})", contact.DisplayName,
                                                contact.CompanyName, linkCount, NumberToProcess), HelperEventArgs.EventTypes.Status));
                                }
                                else
                                {
                                    Log.WarnFormat("Contact '{0}' NOT linked to company '{1}'", contact.DisplayName, contact.CompanyName);
                                }
                            }
                            else
                            {
                                //Ignore if it has a BCMID?
                                string bcmID = contact.GetBCMID();
                                if (!String.IsNullOrEmpty(bcmID))
                                {
                                    //Occurs for manually created contacts only?
                                    Log.WarnFormat("No item link ID for Contact '{0}' (company '{1}')", contact.DisplayName,
                                        contact.CompanyName);
                                    OnError(null,
                                        new HelperEventArgs(
                                            String.Format("No item link ID for Contact '{0}' (company '{1}')",
                                                contact.DisplayName,
                                                contact.CompanyName), HelperEventArgs.EventTypes.Error));
                                }
                                else
                                {
                                    Log.VerboseFormat("Skipping Contact '{0}' (BCMID: {1}; ItemLinkId: {2})", contact.DisplayName, bcmID, contact.ItemLinkId);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            OnError(null, new HelperEventArgs(String.Format("Unknown error in  CreateCompanyLinks for Contact '{0}' (company '{1}')", contact.DisplayName,
                                contact.CompanyName), HelperEventArgs.EventTypes.Error));
                        }

                    }

                    Log.InfoFormat("{0} contacts processed for linking", NumberToProcess);
                    OnGetComplete(null, new HelperEventArgs(String.Format("{0} contacts processed for linking", NumberToProcess), HelperEventArgs.EventTypes.Status));
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                    OnError(null, new HelperEventArgs("Unexpected error linking contacts to companies", HelperEventArgs.EventTypes.Error));
                    return;
                }
            }
        }
        internal async Task CreateContacts()
        {
            using (Log.VerboseCall())
            {
                Cancelled = false;

                switch (DBVersion)
                {
                    case SupportedDBVersions.V3:
                        await RunCreateContactsAsyncV3();
                        break;
                    case SupportedDBVersions.V4:
                        await RunCreateContactsAsync();
                        break;
                }
            }                
        }

        internal async Task FindPeople()
        {
            //Get PersonaId for every contact, then get ItemLinkID from GetPersona call from each contact

            using (Log.VerboseCall())
            {
                Uri uri = new Uri(Settings.Default.EWSEndPoint);
                string xmlRequest = Resources.FindPeopleRequest2;
                int offset = 0;
                int totalPeople = 0;

                try
                {
                    OnStart(null, new HelperEventArgs("Getting contact IDs...", HelperEventArgs.EventTypes.Status));

                    do
                    {
                        int peopleFound = 0;
                        IEnumerable<XElement> elementsRoot;

                        xmlRequest = Resources.FindPeopleRequest2;
                        xmlRequest = xmlRequest.Replace("{0}", offset.ToString());
                        xmlRequest = xmlRequest.Replace("{1}", MigrationHelper.ContactsFolderID);
                        
                        //Log.DebugFormat("Posting to {1}:{2}{0}{2}", xmlRequest, uri, Environment.NewLine);
                        PrepareRequest(RequestDataTypes.EWS, RequestDataFormats.XML);
                        string content = "";

                        //RestResponse<string> response = await Post<string>(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml"));
                        RestResponse<string> response = await Post<string>(uri, xmlRequest, RequestDataFormats.XML);

                        if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
                        {
                            Log.ErrorFormat("ERROR '{1}': {0}", response.ErrorContent, response.StatusCode);
                            OnError(null, new HelperEventArgs(String.Format("Fatal error with FindPeople operation '{1}': {0}", response.ErrorContent), HelperEventArgs.EventTypes.Error));
                            return;
                        }
                        else
                        {
                            //content = await response.Content.Content.ReadAsStringAsync();                            
                            content = response.StringContent;
                        }

                        //using (var response = await _httpClient.PostAsync(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml")))
                        //{
                        //    content = await response.Content.ReadAsStringAsync();
                        //}

                        if (FullRESTLogging)
                        {
                            Log.DebugFormat("FindPeople (offset: {0}): {1}", offset, content);
                        }
                        else
                        {
                            Log.DebugFormat("FindPeople (offset: {0})", offset);
                        }                        

                        XElement responseEnvelope = XElement.Load(new StringReader(content));

                        if (responseEnvelope == null)
                        {
                            OnError(null, new HelperEventArgs(String.Format("ERROR: {0}", "Fatal error with FindPeople operation"), HelperEventArgs.EventTypes.Error));
                            return;
                        }
                        // Check the response for error codes. If there is an error, throw an application exception.
                        IEnumerable<XElement> errorCodes = from errorCode in responseEnvelope.Descendants
                                ("{http://schemas.microsoft.com/exchange/services/2006/messages}ResponseCode")
                                                           select errorCode;
                        foreach (var errorCode in errorCodes)
                        {
                            if (errorCode.Value != "NoError")
                            {
                                switch (errorCode.Parent.Name.LocalName.ToString())
                                {
                                    case "Response":
                                        string responseError = "Response-level error getting FindPeople information:\n" + errorCode.Value;
                                        OnError(null, new HelperEventArgs(String.Format("ERROR: {0}", responseError), HelperEventArgs.EventTypes.Error));
                                        break;
                                    case "UserResponse":
                                        string userError = "User-level error getting FindPeople information:\n" + errorCode.Value;
                                        OnError(null, new HelperEventArgs(String.Format("ERROR: {0}", userError), HelperEventArgs.EventTypes.Error));
                                        break;
                                }
                                return;
                            }
                        }

                        if (totalPeople == 0)
                        {
                            //Get number of contacts in the folder
                            elementsRoot =
                                responseEnvelope.Descendants("{http://schemas.microsoft.com/exchange/services/2006/messages}FindPeopleResponse");
                            elementsRoot = elementsRoot.Descendants("{http://schemas.microsoft.com/exchange/services/2006/messages}TotalNumberOfPeopleInView");
                            totalPeople = Convert.ToInt32(elementsRoot.ElementAt(0).Value);
                            OnDisplayMessage(null, new HelperEventArgs(String.Format("Checking {0} contacts...", totalPeople), HelperEventArgs.EventTypes.Status));
                        }

                        try
                        {
                            //Enumerate Persona nodes in response
                            IEnumerable<XElement> elements = from elementA in
                                responseEnvelope.Descendants
                                    ("{http://schemas.microsoft.com/exchange/services/2006/messages}People")
                                                             select elementA;
                            IEnumerable<XElement> elementsB = from elementB in
                                elements.Descendants("{http://schemas.microsoft.com/exchange/services/2006/types}Persona")
                                                              select elementB;

                            //peopleFound += elementsB.Count();
                            peopleFound = elementsB.Count();

                            int idIndexes = 0;
                            foreach (var elementC in elementsB)
                            {
                                XNode personaNode = elementC.FirstNode;
                                XAttribute att = elementC.FirstAttribute;
                                XElement element1;
                                string personaID = "";
                                string displayName = "";
                                OCMContact2Value ocmContact = null;

                                element1 = (XElement)personaNode;
                                att = element1.FirstAttribute;
                                personaID = att.Value;
                                //Debug.WriteLine(personaID);
                                elementsRoot =
                                    elementC.Descendants("{http://schemas.microsoft.com/exchange/services/2006/types}DisplayName");
                                displayName = elementsRoot.ElementAt(0).Value;
                                ocmContact = GetOCMContact(displayName);

                                if (ocmContact != null)
                                {
                                    Debug.WriteLine(String.Format("Found OCM contact: {0}", ocmContact.DisplayName));
                                    ocmContact.PersonaId = personaID;
                                }
                                else
                                {
                                    Debug.WriteLine(String.Format("Non-OCM contact: {0}", displayName));
                                    continue;
                                }

                                elementsRoot =
                                    elementC.Descendants("{http://schemas.microsoft.com/exchange/services/2006/types}PersonaType");
                                if (elementsRoot.ElementAt(0).Value == "DistributionList")
                                    continue;

                                //TESTED Get ItemLinkId
                                //BUGFIXED 3/15/2018 This is returning the ItemLinkIds node from ALL Persona nodes; thus why the hack below was used
                                //var myelement = from anElement in elementsB.Descendants
                                //        ("{http://schemas.microsoft.com/exchange/services/2006/types}ItemLinkIds")
                                //                select anElement.Value;

                                try
                                {
                                    IEnumerable<XElement> itemLinkIds =
                                        elementC.Descendants(
                                            "{http://schemas.microsoft.com/exchange/services/2006/types}ItemLinkIds");
                                    
                                   //BUGFIXED 3/15/2018 Is this also doubling the ID?  E.g. 0e95a856-464b-4a7c-9e36-00e29c09323520e95a856-464b-4a7c-9e36-00e29c093235. But note this case has a 2 in the middle of the duplicated GUID
                                    XNode itemLinkNode = itemLinkIds.ElementAt(0).FirstNode;
                                    XElement itemLinkElement = (XElement) itemLinkNode;
                                    string id = itemLinkElement.Value.Substring(0, 36);
                                    if (itemLinkElement.Value.Length > 37)
                                    {
                                        Log.WarnFormat("ItemLinkID for '{0}' is unexpected: {1}", displayName, itemLinkElement.Value);
                                        //XNode link1 = itemLinkElement.FirstNode;

                                        IEnumerable<XElement> values = itemLinkElement.Descendants("{http://schemas.microsoft.com/exchange/services/2006/types}Values");
                                        XElement e1 = (XElement)values.ElementAt(0).FirstNode;
                                        id = e1.Value;
                                    }
                                    ocmContact.ItemLinkId = id;

                                    //ocmContact.ItemLinkId = myelement.ElementAt(idIndexes);
                                    //idIndexes += 1;
                                    ////ocmContact.ItemLinkId = myelement.First();
                                    //HACK HACK HACK!!! Must remove trailing 1!
                                    //ocmContact.ItemLinkId = ocmContact.ItemLinkId.Substring(0, ocmContact.ItemLinkId.Length - 1);

                                    ////Try .Value.Value? or .Child.Child, or folderName.First. Still doesn't work! Just use above hack
                                    ////ALSO TRY: folderName.Descendants("Value") .Descendants("Value") 
                                    ////result = element.Descendants("Value").Descendants("Value").ToString();    

                                    Log.DebugFormat("{0} (ItemLinkID: {1})", ocmContact.DisplayName, ocmContact.ItemLinkId);

                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex);
                                    OnError(null, new HelperEventArgs(String.Format("Unknown error in FindPeopleA: {0})", ex), HelperEventArgs.EventTypes.Error));
                                    //throw;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            OnError(null, new HelperEventArgs(String.Format("Unknown error in FindPeopleB: {0})", ex.ToString()), HelperEventArgs.EventTypes.Error));
                        }

                        OnDisplayMessage(null, new HelperEventArgs(String.Format("Processed {0} contacts...", offset), HelperEventArgs.EventTypes.Status));
                        offset += peopleFound;

                    } while (totalPeople > offset);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    OnError(null, new HelperEventArgs(String.Format("Unknown error in FindPeopleC: {0})", ex.ToString()), HelperEventArgs.EventTypes.Error));
                }
            }
        }
        internal async Task GetBCMContacts()
        {
            using (Log.VerboseCall())
            {
                try
                {
                    if (BCMDataLogged)
                    {
                        Log.Debug("Previously run - skipping");
                        return;
                    }

                    OnStart(null, new HelperEventArgs("Getting BCM Contacts data", HelperEventArgs.EventTypes.Status));

                    //using (var context = new MSSampleBusinessEntities())
                    //{
                    //    //#if !DEBUG
                    //    //                    context.Database.Connection.ConnectionString = ConnectionString;
                    //    //#else
                    //    //                    //NOTE Use this for mounting a LocalDB db during debugging
                    //    //                    object dbPath = AppDomain.CurrentDomain.GetData("DataDirectory");
                    //    //                    if (dbPath == null)
                    //    //                    {
                    //    //                        string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    //    //                        if (Environment.MachineName == "PRECISION")
                    //    //                            path = @"C:\Professional\Projects\Microsoft\BCM Migration Tool\BCM Migration Tool";
                    //    //                        AppDomain.CurrentDomain.SetData("DataDirectory", path);
                    //    //                        dbPath = AppDomain.CurrentDomain.GetData("DataDirectory");
                    //    //                    }
                    //    //#endif

                    //    //NOTE Can also use BCM_BusinessContacts_Core.sql in Resources instead of EF
                    //    context.Database.Connection.ConnectionString = ConnectionString;

                    //    var contacts = context.ContactFullViews.Where(mycontacts => mycontacts.Type == 1 && mycontacts.IsDeletedLocally == false);
                    //}

                    switch (DBVersion)
                    {
                        case SupportedDBVersions.V3:
                            LogBCMContactsV3();
                            break;
                        case SupportedDBVersions.V4:
                            LogBCMContactsV4();
                            break;
                    }

                    BCMDataLogged = true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    OnError(null, new HelperEventArgs(String.Format("Error in ContactsHelper.GetBCmContacts: {0}", ex.Message), HelperEventArgs.EventTypes.Error));                
                }
            }
        }
        private OCMContact2Value GetOCMContact(string val, UpdateKeyTypes keyType)
        {
            if (OCMContacts == null)
                return null;

            try
            {
                switch (keyType)
                {
                    case UpdateKeyTypes.BCMID:
                        foreach (var contact in OCMContacts)
                        {
                            if (contact.SingleValueExtendedProperties != null)
                            {
                                foreach (var prop in contact.SingleValueExtendedProperties)
                                {
                                    //"String {bc013ba3-3a6d-4826-b0ec-cb703a722b09} Name BCMID"
                                    if (prop.PropertyId.ToLower() == "string {bc013ba3-3a6d-4826-b0ec-cb703a722b09} name bcmid" && prop.Value == val)
                                    {
                                        return contact;
                                    }
                                }
                            }
                        }
                        break;
                    case UpdateKeyTypes.EmailAddress:
                        foreach (var account in OCMContacts)
                        {
                            if (account.EmailAddresses != null)
                            {
                                foreach (var email in account.EmailAddresses)
                                {
                                    if (email.Address == val)
                                        return account;
                                }
                            }
                        }

                        break;
                    case UpdateKeyTypes.Name:
                        var mycontact = OCMContacts.FirstOrDefault(contacts => contacts.DisplayName == val);
                        return mycontact;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return null;
        }
        internal OCMContact2Value GetOCMContact(string nameorid)
        {
            OCMContact2Value result = null;

            //var contacts = from contact in OCMAccounts where contact.CompanyName.ToString() == nameorid select contact;
            if (OCMContacts == null)
                return null;
            OCMContact2Value contact = null;
            try
            {
                contact = OCMContacts.FirstOrDefault(contacts => contacts.DisplayName.ToString() == nameorid);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            if (contact != null)
                result = (OCMContact2Value) contact;

            return result;
        }
        internal async Task GetOCMContacts()
        {
            using (Log.VerboseCall())
            {
                if (OCMDataRetrieved)
                {
                    Log.Debug("Previously retrieved - skipping");
                    return;
                }

                OCMContacts = new List<OCMContact2Value>();
                await RunGetOCMContactsAsync();
                bool headerShown = false;

                //OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} existing contacts (* = previously imported):", OCMContacts?.Count), HelperEventArgs.EventTypes.Status));
                //OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} existing OCM contacts", OCMContacts?.Count), HelperEventArgs.EventTypes.Status));

                Log.InfoFormat("Found {0} existing contacts", OCMContacts?.Count);

                foreach (var item in OCMContacts)
                {
                    string bcmID = item.GetBCMID();
                    string output = "";

                    try
                    {
                        ////output = String.Format(" -{3}{0}{2}{1}", item.DisplayName,
                        //    !String.IsNullOrEmpty(bcmID) ? String.Format(" (BCM ID: {0})", bcmID) : "",
                        //    String.IsNullOrEmpty(item.CompanyName?.ToString()) ? "" : ": " + item.CompanyName,
                        //    !String.IsNullOrEmpty(bcmID) ? "*" : "");
                        
                        //BUG HIGH If values have braces, format exception!!! Use string interpolation? Wait - had a bad Log.DebugFormat(val) line below; switched to Log.Debug
                        output =
                            $" -{(!String.IsNullOrEmpty(bcmID) ? "" : "")}{item.DisplayName}{(String.IsNullOrEmpty(item.CompanyName?.ToString()) ? "" : ": " + item.CompanyName)}{(!String.IsNullOrEmpty(bcmID) ? String.Format(" (BCM ID: {0})", bcmID) : "")}";
                        //output = $" -@{!String.IsNullOrEmpty(bcmID) ? "" : ""}";
                        if (!String.IsNullOrEmpty(bcmID)) //Only output previously imported contacts to log window
                        {
                            if (!headerShown)
                            {                            
                                OnGetComplete(null, new HelperEventArgs("Previously imported contacts:", HelperEventArgs.EventTypes.Status));
                                headerShown = true;
                            }
                            Log.Debug(output);
                            if (LogRecordNames)
                                OnGetItemComplete(null, new HelperEventArgs(output, HelperEventArgs.EventTypes.Status));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }

                OCMDataRetrieved = true;
            }
        }
        private async Task GetOCMContactsAsync(bool getSharedContacts)
        {
            using (Log.VerboseCall())
            {
                try
                {                
                    string request;

                    if (!String.IsNullOrEmpty(LastPageResult))
                    {
                        request = LastPageResult;
                    }
                    else
                    {
                        //request = "https://outlook.office.com/api/v2.0/me/contacts?$select=EmailAddresses,GivenName,SurName,DisplayName,CompanyName,JobTitle,BusinessPhones,HomePhones,Birthday,PersonalNotes";
                        //request = "https://outlook.office.com/api/v2.0/me/contacts?$top=50&$select=EmailAddresses,GivenName,SurName,DisplayName,CompanyName,JobTitle,BusinessPhones,HomePhones,Birthday,PersonalNotes";                  
                        //request = String.Format("{0}/contacts?$top=50&$expand=SingleValueExtendedProperties($filter=PropertyId%20eq%20'String%20{1}%20Name%20BCMID')&$select=EmailAddresses,GivenName,SurName,DisplayName,CompanyName", Properties.Settings.Default.V2EndPoint, FieldMappings.BCMIDPropertyGUID);
                        //Get ALL fields

                        if (getSharedContacts)
                        {
                            //Shared contacts will be in the All Sales Team folder under the default Contacts Folder
                            //e.g. https://graph.microsoft.com/beta/me/contactfolders/AAMkAGUzNmEzYTBmLTI1NDItNGE0My1iZDk5LWFkMDgxODI3YWNlOQAuAAAAAACK2VEhi72QSaw+u0XV7xUHAQAuH-1UA8tzTYD5jbYriaIUAAJ4xE13AAA=/contacts
                            request = String.Format("{0}/contactfolders/{2}/contacts?$top=50&$expand=SingleValueExtendedProperties($filter=PropertyId%20eq%20'String%20{1}%20Name%20BCMID')", Settings.Default.V2EndPoint, FieldMappings.BCMIDPropertyGUID, MigrationHelper.ContactsFolderID.Replace("/", "-"));
                        }
                        else
                        {
                            request = String.Format("{0}/contacts?$top=50&$expand=SingleValueExtendedProperties($filter=PropertyId%20eq%20'String%20{1}%20Name%20BCMID')", Settings.Default.V2EndPoint, FieldMappings.BCMIDPropertyGUID);
                        }
                    }

                    //Debug.WriteLine("Request to : " + request);
                    PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.None);
                    
                    RestResponse<OCMContact2> contacts = await Get<OCMContact2>(request);
                    if (contacts.StatusCode == HttpStatusCode.OK)
                    {
                        if (getSharedContacts == true)
                        {
                            foreach (var contact in contacts.Content.value)
                            {
                                if (String.IsNullOrEmpty(contact.BCMID))
                                {
                                    //TESTED Must set BCMID for later use if we are fetching existing shared contacts. Should probably call GetBCMID in the class' constructor?
                                    contact.BCMID = contact.GetBCMID();
                                }
                            }
                        }

                        if (OCMContacts == null)
                        {
                            OCMContacts = contacts.Content.value.ToList(); //contacts2.value.ToList();
                            //Debug.WriteLine(String.Format("{0} OCM Companies retrieved", OCMContacts.Count));
                        }
                        else
                        {
                            //Append result to previously set collection
                            OCMContacts.AddRange(contacts.Content.value.ToList());
                            //Debug.WriteLine(String.Format("Added {0} more OCM Companies", contacts.value.Length));
                        }

                        Debug.WriteLine(contacts.Content.odatanextLink);
                        if (!String.IsNullOrEmpty(contacts.Content.odatanextLink))
                        {
                            if (contacts.Content.odatanextLink == LastPageResult) //Done paging
                            {
                                //This doesn't fire
                                OnGetItemComplete(null, new HelperEventArgs(String.Format("Fetched {0} contacts", OCMContacts.Count), HelperEventArgs.EventTypes.Status));
                                return;
                            }
                            //Debug.WriteLine(String.Format("Setting page: {0}", contacts.odatanextLink));
                            LastPageResult = contacts.Content.odatanextLink.ToString();
                        }
                        else
                        {
                            //Done paging
                            LastPageResult = null;
                        }
                    }
                    else
                    {                        
                        OnError(null, new HelperEventArgs(String.Format("ERROR in GetOCMContactsAsync(): {1}: {0}", contacts.ErrorContent, contacts.StatusCode), HelperEventArgs.EventTypes.Error));
                        NumberOfErrors += 1;
                        return;
                    }

                }
                catch(Exception ex)
                {
                    Log.Error(ex);                    
                }
            }
        }
        private async Task<OCMCompanyTemplate.Rootobject> GetTemplateAsync()
        {
            using (Log.VerboseCall())
            {
                try
                {
                    PrepareRequest(RequestDataTypes.Templates, RequestDataFormats.None);
                    using (
                        var response =
                            await _httpClient.GetAsync(
                                "https://outlook.office.com/api/beta/me/XrmContactTemplate('ContactTemplate')"))
                    {
                        // Check status code.
                        //response.EnsureSuccessStatusCode(); //throw a message in case of an error                

                        if (!response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            OnError(null,
                                new HelperEventArgs(String.Format("ERROR '{0}': {1}", response.ReasonPhrase, content),
                                    HelperEventArgs.EventTypes.Error, HelperEventArgs.ProcessingModes.Mapping));
                            NumberOfErrors += 1;
                            return null;
                        }

                        // Read and deserialize response.                                
                        var content2 = await response.Content.ReadAsStreamAsync();
                        var content3 = await response.Content.ReadAsStringAsync();
                        var jsonSerializer = new DataContractJsonSerializer(typeof(OCMCompanyTemplate.Rootobject));
                        OCMCompanyTemplate.Rootobject jsonResponse = jsonSerializer.ReadObject(content2) as OCMCompanyTemplate.Rootobject;
                        //Version: 11
                        Log.InfoFormat("Contacts template retrieved with {0} custom fields: {1}", jsonResponse.Template?.FieldList?.Length, content3);
                        OnGetComplete(null, new HelperEventArgs(String.Format("Contacts template retrieved with {0} custom fields", jsonResponse.Template?.FieldList?.Length), HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
                        return jsonResponse;
                    }

                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    OnError(null, new HelperEventArgs(String.Format("ERROR in GetTemplateAsync: {0}", ex.Message), HelperEventArgs.EventTypes.Error, HelperEventArgs.ProcessingModes.Mapping));
                    NumberOfErrors += 1;
                    return null;
                }
            }
        }

        private async Task ImportContactAsync(ContactFullView contact, string companyName, ImportModes importMode, OCMContact2Value existingContact = null)
        {
            using (Log.VerboseCall())
            {
                try
                {
                    OCMContact.Rootobject ocmContact = new OCMContact.Rootobject();
                    int defaultProps = 0;
                    SingleValueExtendedProperty[] customProps = null;

                    if (importMode == ImportModes.UpdatePreviouslyImportedItem)
                    {
                        Log.VerboseFormat("Contact '{0}' previously imported", contact.FullName);
                        goto setProperties; //No need to create sharing or BCMID props on previously imported item
                    }

                    defaultProps = MigrationHelper.IsSharingEnabled ? 5 : 1;
                    
                    //defaultProps = 1; //TESTED 1.0.20 Set extended props after copy op

                    ocmContact.SingleValueExtendedProperties = new SingleValueExtendedProperty[defaultProps];
                    //Add BCMID prop
                    SingleValueExtendedProperty bcmIDProp = new SingleValueExtendedProperty();
                    bcmIDProp.PropertyId = String.Format("String {0} Name BCMID", FieldMappings.BCMIDPropertyGUID);
                    bcmIDProp.Value = contact.EntryGUID.ToString();
                    ocmContact.SingleValueExtendedProperties[0] = bcmIDProp;

                    if (MigrationHelper.IsSharingEnabled)
                    {
                        //TESTED When sharing Contacts, these extra properties must be set:
                        //Set “XrmSourceMailboxGuid” with the mailbox guid for the sharing modern - group.
                        //Set “SmbBusinessType” to “Individual”
                        //Set “SmbIsCustomer” to “1”

                        //When sharing any item, the following properties must be set: 
                        //Set “XrmSharingSourceUserDisplayName” with the display name of the current user
                        //e.g. <ExtendedFieldURI PropertySetId="1a417774-4779-47c1-9851-e42057495fca" PropertyName="XrmSharingSourceUserDisplayName" PropertyType="String" xmlns="http://schemas.microsoft.com/exchange/services/2006/messages" />                
                        //Set “XrmSharingSourceUser” with the Object - Id(AAD ID) of the current user                
                        //e.g. <ExtendedFieldURI PropertySetId="1a417774-4779-47c1-9851-e42057495fca" PropertyName="XrmSharingSourceUser" PropertyType="String" xmlns="http://schemas.microsoft.com/exchange/services/2006/messages" />

                        Log.VerboseFormat("Sharing Contact '{0}'", contact.FullName);

                        SingleValueExtendedProperty addProp = null;
                        //TESTED 1.0.20 Try setting these props when doing a copy (NOT MOVE!). Doesn't work - not visible in UI but visible in OCM folder in ExSpy
                        addProp = new SingleValueExtendedProperty();
                        addProp.PropertyId = "String 1a417774-4779-47c1-9851-e42057495fca Name XrmSharingSourceUserDisplayName";
                        addProp.Value = MigrationHelper.CurrentUser.DisplayName;
                        ocmContact.SingleValueExtendedProperties[1] = addProp;

                        addProp = new SingleValueExtendedProperty();
                        addProp.PropertyId = "String 1a417774-4779-47c1-9851-e42057495fca Name XrmSharingSourceUser";
                        addProp.Value = MigrationHelper.CurrentUserID;
                        ocmContact.SingleValueExtendedProperties[2] = addProp;
                        //END TEST

                        addProp = new SingleValueExtendedProperty();
                        addProp.PropertyId = "String bdba944b-fc2b-47a1-8ba4-cafc4ae13ea2 Name BusinessType"; //TESTED Different name; NOT SmbBusinessType
                        addProp.Value = "Individual";
                        ocmContact.SingleValueExtendedProperties[3] = addProp;

                        //Set "Business Contact" bit
                        addProp = new SingleValueExtendedProperty();
                        //TESTED As per Nick's suggestion
                        addProp.PropertyId = "Boolean 00062004-0000-0000-c000-000000000046 Name CustomerBit";
                        addProp.Value = "true";
                        ocmContact.SingleValueExtendedProperties[4] = addProp;
                    }

                    setProperties:

                    if (FieldMappings.BCMBusinessContactFields.BCMFields == null) //Why would this be null? Does this get thit?
                    {
                        Log.Warn("FieldMappings.BCMBusinessContactFields.BCMFields = null!");
                        goto populateContactDetails;
                    }
                    if (FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields.Count == 0 || DisableCustomFields)
                        //Just the BCMID, no custom fields
                    {                        
                        if (DisableCustomFields)
                            Log.Verbose("Custom fields disabled for Contacts");
                        goto populateContactDetails;
                    }

                    //Import custom fields
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        //HIGH Filter SQL query by Contact GUID instead of looping
                        using (SqlCommand com = new SqlCommand(Resources.BCM_CustomFields_Contacts, con))
                        {
                            con.Open();

                            using (DbDataReader reader = com.ExecuteReader())
                            {
                                try
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            string contactID = Convert.ToString(reader["EntryGUID"]);

                                            if (!String.IsNullOrEmpty(contactID) && contactID != contact.EntryGUID.ToString())
                                                continue;

                                            Log.VerboseFormat("Importing values from {0} mapped fields for contact '{1}'", FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields.Count, contact.FullName);
                                            foreach (var bcmField in FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields)
                                            {
                                                try
                                                {
                                                    if (bcmField.OCMFieldMapping == null)
                                                    //Is this ever likely to happen if we are pre-validating field mappings now?
                                                    {
                                                        Log.ErrorFormat("Invalid OCMFieldMapping '{0}'", bcmField.Name);
                                                        NumberOfErrors += 1;
                                                        //OnError(null, new HelperEventArgs(String.Format("Unknown error (D) in ImportContactAsync for Account '{0}'", contact.FullName), HelperEventArgs.EventTypes.Error));
                                                        continue;
                                                    }
                                                    for (int i = 0; i < reader.FieldCount; i++)
                                                    {
                                                        try
                                                        {
                                                            string fieldName = reader.GetName(i);
                                                            if (fieldName == bcmField.Name)
                                                            {
                                                                string valStr = reader[i].ToString() as string ?? default(string);

#if DEBUG
                                                                //Seed with test values if empty
                                                                if (String.IsNullOrEmpty(valStr))
                                                                {
                                                                    switch (bcmField.OCMFieldMapping.FieldType)
                                                                    {
                                                                        case FieldMappings.OCMField.OCMFieldTypes.NumberOrText:
                                                                        case FieldMappings.OCMField.OCMFieldTypes.Text:
                                                                            valStr = "10";
                                                                            break;
                                                                        case FieldMappings.OCMField.OCMFieldTypes.Date:
                                                                            valStr = DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ssZ");
                                                                            break;
                                                                    }
                                                                }
#endif

                                                                if (!String.IsNullOrEmpty(valStr))
                                                                {
                                                                    SingleValueExtendedProperty prop =
                                                                        new SingleValueExtendedProperty();
                                                                    prop.PropertyId = String.Format("{0} {1} Name {2}",
                                                                        bcmField.OCMFieldMapping.DataTypeLabelForJSON,
                                                                        "{1a417774-4779-47c1-9851-e42057495fca}",
                                                                        bcmField.OCMFieldMapping.PropertyID);

                                                                    //prop.Value = valStr;
                                                                    if (bcmField.OCMFieldMapping.FieldType == FieldMappings.OCMField.OCMFieldTypes.Date)
                                                                    {
                                                                        //TESTED Convert to proper date format
                                                                        //prop2.Value = (DateTimeOffset)val;
                                                                        DateTime convertedDate = DateTime.Parse(valStr);
                                                                        //Convert.ToDateTime(reader["CreatedOn"]).ToString("yyyy-MM-ddTHH:MM:ssZ");
                                                                        prop.Value = convertedDate.ToString("yyyy-MM-ddTHH:MM:ssZ");
                                                                    }
                                                                    else
                                                                    {
                                                                        prop.Value = valStr;
                                                                    }

                                                                    if (customProps == null)
                                                                    {
                                                                        customProps = new SingleValueExtendedProperty[1];
                                                                    }
                                                                    else
                                                                    {
                                                                        Array.Resize(ref customProps, customProps.Length + 1);
                                                                    }
                                                                    customProps[customProps.Length - 1] = prop;
                                                                }
                                                                else
                                                                {
                                                                    Log.VerboseFormat("Null value in field '{0}'", fieldName);
                                                                }
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                //Log.VerboseFormat("Skipping field '{0}'", fieldName);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Log.Error(ex);
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log.Error(ex);
                                                }
                                            }
                                            break;//we found the Contact and are done
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex);
                                    NumberOfErrors += 1;
                                    //OnError(null, new HelperEventArgs(String.Format("Unknown error (A) in ImportContactAsync for Company '{0}'", contact.FullName), HelperEventArgs.EventTypes.Error, ex));
                                }
                            }
                        }
                    }

                    try
                    {
                        //TESTED Now loop through MappedFields and find non-custom BCM fields; set values of these fields
                        foreach (var bcmField in FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields)
                        {
                            try
                            {
                                if (bcmField.OCMFieldMapping == null)
                                {
                                    Log.Warn("BCM field '{0}' not mapped to OCM field", bcmField.Name);
                                    continue;
                                }
                                if (bcmField.IsCustom)
                                    continue; //Handled above
                                PropertyInfo prop = contact.GetType().GetProperty(bcmField.Name, BindingFlags.Public | BindingFlags.Instance);

                                string val = "";

                                try
                                {
                                    val = prop.GetValue(contact).ToString();
                                }
                                catch (Exception e)
                                { }
                                if (!String.IsNullOrEmpty(val))
                                {
                                    SingleValueExtendedProperty prop2 = new SingleValueExtendedProperty();
                                    prop2.PropertyId = String.Format("{0} {1} Name {2}",
                                        bcmField.OCMFieldMapping.DataTypeLabelForJSON, "{1a417774-4779-47c1-9851-e42057495fca}",
                                        bcmField.OCMFieldMapping.PropertyID);
                                    if (bcmField.OCMFieldMapping.FieldType == FieldMappings.OCMField.OCMFieldTypes.Date)
                                    {
                                        DateTime convertedDate = DateTime.Parse(val);
                                        prop2.Value = convertedDate.ToString("yyyy-MM-ddTHH:MM:ssZ");
                                    }
                                    else
                                    {
                                        prop2.Value = val;
                                    }
                                    
                                    if (customProps == null)
                                    {
                                        customProps = new SingleValueExtendedProperty[1];
                                    }
                                    else
                                    {
                                        Array.Resize(ref customProps, customProps.Length + 1);
                                    }
                                    customProps[customProps.Length - 1] = prop2;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }

                    if (customProps != null)
                    {              
                        //TESTED Adding to existing props                       
                        try
                        {
                            SingleValueExtendedProperty[] curProps = ocmContact.SingleValueExtendedProperties;
                            int idxStart = curProps.Length;
                            Array.Resize(ref curProps, customProps.Length + curProps.Length);
                            customProps.CopyTo(curProps, idxStart);
                            ocmContact.SingleValueExtendedProperties = curProps;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                    }

                    //=========================================================================================
                    //Automatic mappings
                    //=========================================================================================
                    populateContactDetails:

                    if (existingContact != null)
                    {
                        //TESTED If updating existing company, use existingCompany var or copy to ocmCompany

                        //BUGFIXED 1.0.17 Will get error as per PATCH below if we don't use a nullable type and ensure we don't set if it's a minvalue
                        if (existingContact.Birthday != null && existingContact.Birthday != DateTimeOffset.MinValue)
                            ocmContact.Birthday = (DateTimeOffset)existingContact.Birthday;
                        ocmContact.BusinessAddress = existingContact.BusinessAddress;
                        try
                        {
                            ocmContact.BusinessPhones = (string[]) existingContact.BusinessPhones;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Update error for BusinessPhones", ex);
                        }
                        ocmContact.CompanyName = existingContact.CompanyName;
                        ocmContact.DisplayName = existingContact.DisplayName;
                        try
                        {
                            ocmContact.EmailAddresses = (OCMContact.Emailaddress[]) existingContact.EmailAddresses;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Update error for EmailAddresses", ex);
                        }
                        ocmContact.GivenName = existingContact.GivenName;
                        try
                        {
                            ocmContact.HomePhones = (string[]) existingContact.HomePhones;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Update error for HomePhones", ex);
                        }
                        ocmContact.JobTitle = existingContact.JobTitle;
                        ocmContact.MiddleName = existingContact.MiddleName;
                        ocmContact.PersonalNotes = existingContact.PersonalNotes;
                        ocmContact.Surname = existingContact.Surname?.ToString();
                        ocmContact.Title = existingContact.Title;

                        //TESTED 1.0.17 Wasn't setting ID or mobile...
                        ocmContact.Id = existingContact.Id;
                        ocmContact.MobilePhone1 = existingContact.MobilePhone1?.ToString();
                    }

                    //Skip null value (do not update existing non-empty value with an empty/null value from BCM)

                    if (!String.IsNullOrEmpty(contact.FullName))
                        ocmContact.DisplayName = contact.FullName;

                    try
                    {
                        //{1/20/2009 12:00:00 AM}
                        if (contact.Birthday != null)
                            ocmContact.Birthday = (DateTimeOffset) contact.Birthday;
                        //BUGFIXED "Cannot convert the literal '2009-01-20T00:00:00' to the expected type 'Edm.DateTimeOffset'." Try now with switch to DateTimeOffset type}
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }

                    try
                    {
                        //if (!String.IsNullOrEmpty(contact.CompanyName))
                        //    ocmContact.CompanyName = contact.CompanyName;
                        if (!String.IsNullOrEmpty(companyName))
                            ocmContact.CompanyName = companyName;
                        if (!String.IsNullOrEmpty(contact.FirstName))
                        {
                            ocmContact.GivenName = contact.FirstName;
                        }
                        else
                        {
                            //BUGFIXED 1.0.16 Error creating 'De heer Joop Visser': {"error":{"code":"ErrorInvalidProperty","message":"The property 'GivenName' is required when creating the entity."}}
                            //NOTE: Warning will occur if FirstName is null; null LastName values are oddly okay
                            ocmContact.GivenName = "_UNKNOWN";
                        }

                        //ocmContact.Id = contact.CustomerID; //TODO Set ID to what?
                        if (!String.IsNullOrEmpty(contact.JobTitle))
                            ocmContact.JobTitle = contact.JobTitle;                        
                        if (!String.IsNullOrEmpty(contact.ContactNotes))
                            ocmContact.PersonalNotes = contact.ContactNotes;
                        if (!String.IsNullOrEmpty(contact.LastName))
                            ocmContact.Surname = contact.LastName;

                        if (!String.IsNullOrEmpty(contact.Email1Address))
                        {
                            ocmContact.EmailAddresses = new OCMContact.Emailaddress[1];
                            OCMContact.Emailaddress email = new OCMContact.Emailaddress();

                            email.Name = contact.Email1DisplayAs;
                            email.Address = contact.Email1Address;
                            ocmContact.EmailAddresses[0] = email;
                        }

                        if (!String.IsNullOrEmpty(contact.WorkAddressStreet))
                        {
                            Businessaddress businessAddress = new Businessaddress();
                            businessAddress.City = contact.WorkAddressCity;
                            businessAddress.CountryOrRegion = contact.WorkAddressCountry;
                            businessAddress.PostalCode = contact.WorkAddressZip;
                            businessAddress.State = contact.WorkAddressState;
                            businessAddress.Street = contact.WorkAddressStreet;

                            ocmContact.BusinessAddress = businessAddress;
                        }

                        try
                        {
                            //Changed mapping from BCM CompanyMainPhoneNum to OCM BusinessPhones to BCM WorkPhoneNum to OCM BusinessPhones  
                            //BUGFIXED 1.0.17 Error updating a contact: {"error":{"code":"ErrorInvalidProperty","message":"The multi value property BusinessPhones has 3 entries, that exceeds the max allowed value of 2."}}. However, only one business phone is listed in the UI...don't add multiple, and only update non-null (could be non-null if an existing contact)
                            if (!String.IsNullOrEmpty(contact.WorkPhoneNum) && (ocmContact.BusinessPhones?.Length == 0 || ocmContact.BusinessPhones == null))
                            {                                
                                ocmContact.BusinessPhones = new string[1];
                                ocmContact.BusinessPhones[0] = contact.WorkPhoneNum;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                        try
                        {
                            if (!String.IsNullOrEmpty(contact.HomePhoneNum))
                            {
                                //1.0.18 only one home phone is listed in the UI...don't add multiple, and only update non-null (could be non-null if an existing contact)
                                if (!String.IsNullOrEmpty(contact.HomePhoneNum) &&
                                    (ocmContact.HomePhones?.Length == 0 || ocmContact.HomePhones == null))
                                {
                                    ocmContact.HomePhones = new string[1];
                                    ocmContact.HomePhones[0] = contact.HomePhoneNum;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                        if (!String.IsNullOrEmpty(contact.MobilePhoneNum))
                        {
                            ocmContact.MobilePhone1 = contact.MobilePhoneNum;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        OnError(null, new HelperEventArgs(String.Format("Unknown error (B) in CreateContactAsync for Company '{0}'", contact.FullName), HelperEventArgs.EventTypes.Error, ex));
                    }

                    //=====================================================================================

                    var json = JsonConvert.SerializeObject(ocmContact);
                    
                    try
                    {
                        Uri uri;
                        if (importMode == ImportModes.Create)
                        {
                            uri = new Uri(String.Format("{0}/contacts?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUserDisplayName'))", Settings.Default.V2EndPoint));
                            //{https://outlook.office.com/api/v2.0/me/contacts?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+{1a417774-4779-47c1-9851-e42057495fca}+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+{1a417774-4779-47c1-9851-e42057495fca}+Name+XrmSharingSourceUserDisplayName'))}

                            //Debug.WriteLine(String.Format("Posting to {1}:{2}{0}{2}Token:{2}{3}", json, uri, Environment.NewLine, AccessToken));

                            Log.VerboseFormat("Posting POST (create) call to {1}:{0}", json, uri);
                            PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.JSON);

                            //RestResponse<OCMContact2Value> newContact = await Post<OCMContact2Value>(uri, new StringContent(json, Encoding.UTF8, "application/json"));
                            RestResponse<OCMContact2Value> newContact = await Post<OCMContact2Value>(uri, json, RequestDataFormats.JSON);

                            if (newContact.StatusCode == HttpStatusCode.Created)
                            {
                                NumberCreated += 1;

                                Log.DebugFormat("Created Contact '{0}' (BCMID: {1}) ({2}/{3})", contact.FullName, contact.EntryGUID.ToString(), NumberCreated, NumberToProcess);
                                OnCreateItemComplete(null, new HelperEventArgs(String.Format("Created Contact '{0}' ({1}/{2})...", contact.FullName, NumberCreated, NumberToProcess), false));                                

                                newContact.Content.BCMID = contact.EntryGUID.ToString();
                                OCMContacts.Add(newContact.Content);

                                PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.XML);
                                uri = new Uri(Settings.Default.EWSEndPoint);

                                string id = newContact.Content.Id.Replace("_", "+");
                                id = id.Replace("-", "/"); //HACK to make the MoveItem operation work (ErrorInvalidIdMalformed)

                                //string xmlRequest = String.Format(Resources.ShareContactsRequest, MigrationHelper.ContactsFolderID, id);
                                //TESTED 1.0.20 Use a COPY operation, not a MOVE
                                string xmlRequest = String.Format(Resources.ShareContactByCopyingRequest, MigrationHelper.ContactsFolderID,
                                    id);

                                //To make a Contact or Company shared: make a REST call to ‘/move’ with the folder-id of the sharing child-folder, not the modern-group mailbox guid.//NOTE: Now a COPY (as of 3/2018)
                                //NOTE If doing a PATCH, do we need to check if it has already been shared so as not to 'move' it again?

                                //REVIEW We could gather all these IDs and do the sharing operation all in one call by creating ItemID elements for each contact to put in the ItemIds element

                                //RestResponse<HttpWebResponse> copyRequestResponse = await Post<HttpWebResponse>(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml"));
                                RestResponse<HttpWebResponse> copyRequestResponse = await Post<HttpWebResponse>(uri, xmlRequest,
    RequestDataFormats.XML);

                                if (copyRequestResponse.StatusCode != HttpStatusCode.Created &&
                                    copyRequestResponse.StatusCode != HttpStatusCode.OK) // Check status code.
                                {
                                    Log.ErrorFormat("Error sharing '{1}': {0}", copyRequestResponse.ErrorContent,
                                        contact.FullName);
                                    OnError(null,
                                        new HelperEventArgs(
                                            String.Format("ERROR sharing '{1}': {0}", copyRequestResponse.ErrorContent,
                                                contact.FullName),
                                            HelperEventArgs.EventTypes.Error));
                                    NumberOfErrors += 1;
                                }
    //                            else
    //                            {
    //                                //TESTED 1.0.20 Set extended props now? No - set in JSON for when creating the item. Testing shows these contacts do not become visible in UI, only in OCM folder as per ExSpy
    //                                string xmlRequest2 = String.Format(Resources.SetExtendedPropertiesOnContactRequest,
    //                                    newContact.Content.Id, newContact.Content.ChangeKey,
    //                                    MigrationHelper.CurrentUserID, MigrationHelper.CurrentUser.DisplayName,
    //                                    "Individual", 1);
    //                                PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.XML);
    //                                RestResponse<HttpWebResponse> setPropsRequestResponse = await Post<HttpWebResponse>(uri, new StringContent(xmlRequest2, Encoding.UTF8, "text/xml"));
    //                                if (setPropsRequestResponse.StatusCode != HttpStatusCode.Created &&
    //copyRequestResponse.StatusCode != HttpStatusCode.OK) // Check status code.
    //                                {
    //                                    Log.ErrorFormat("Error setting extended properties '{1}': {0}", setPropsRequestResponse.ErrorContent,
    //                                        contact.FullName);
    //                                    OnError(null,
    //                                        new HelperEventArgs(
    //                                            String.Format("ERROR setting extended properties '{1}': {0}", setPropsRequestResponse.ErrorContent,
    //                                                contact.FullName),
    //                                            HelperEventArgs.EventTypes.Error));
    //                                    NumberOfErrors += 1;
    //                                }
    //                            }
                            }
                            else
                            {
                                Log.ErrorFormat("Error creating '{1}': {0}", newContact.ErrorContent, contact.FullName);
                                OnError(null, new HelperEventArgs(String.Format("ERROR creating '{0}': {2}: {1}", contact.FullName, newContact.ErrorContent, newContact.StatusCode), HelperEventArgs.EventTypes.Error));
                                NumberOfErrors += 1;
                            }                            
                        }
                        else
                        {
                            uri = new Uri(String.Format("{0}/contacts('{1}')?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUserDisplayName'))", Settings.Default.V2EndPoint, existingContact.Id));

                            HttpMethod method = new HttpMethod("PATCH");
                            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpRequestMessage request = new HttpRequestMessage(method, uri) { Content = httpContent };

                            PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.JSON);

                            if (FullRESTLogging)
                                Log.VerboseFormat("Posting PATCH (update) call to {1}:{0}", json, uri);

                            //Don't log json/uri in Patch call - do above
                            RestResponse<HttpWebResponse> response = await Patch<HttpWebResponse>(request, true, null, true, false);
                            if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
                            {
                                Log.ErrorFormat("Error updating '{1}': {0}", response.ErrorContent, contact.FullName);
                                OnError(null, new HelperEventArgs(String.Format("ERROR updating '{1}': {0}. JSON: {2}", response.ErrorContent, contact.FullName, json), HelperEventArgs.EventTypes.Error));
                                NumberOfErrors += 1;
                            }
                            else
                            {
                                OnPatchComplete(null, new HelperEventArgs(String.Format("Updated Contact '{0}'...", contact.FullName), false));
                                NumberUpdated += 1;
                                //REVIEW Do we need to set the BCMID on an existing contact? So far so good...
                                existingContact.BCMID = contact.EntryGUID.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        OnError(null, new HelperEventArgs(String.Format("Unknown error (C) in CreateContactAsync for Contact '{0}'", contact.FullName), HelperEventArgs.EventTypes.Error, ex));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        private void LogBCMContactsV3()
        {
            int cnt = 0;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {                
                using (SqlCommand com = new SqlCommand(Resources.BCM_Contacts_Simple, con))
                {
                    con.Open();

                    using (SqlCommand comA = new SqlCommand(Resources.BCM_ContactFullViewCount_V3, con))
                    {
                        //cnt = comA.ExecuteNonQuery();
                        cnt = Convert.ToInt32(comA.ExecuteScalar());
                    }
                    using (DbDataReader reader = com.ExecuteReader())
                    {
                        try
                        {
                            if (reader.HasRows)
                            {
                                Log.InfoFormat("Found {0} BCM Contacts:", cnt); 
                                OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} BCM Contacts:", cnt), HelperEventArgs.EventTypes.Status));

                                while (reader.Read())
                                {
                                    try
                                    {
                                        string record = $" -{Convert.ToString(reader["FullName"])} (Company: {Convert.ToString(reader["CompanyName"])})";
                                        if (LogRecordNames)
                                            OnGetItemComplete(null, new HelperEventArgs(record, HelperEventArgs.EventTypes.Status));
                                        Log.Debug(record);
                                    }
                                    catch (System.Exception ex)
                                    {
                                        Log.Error(ex);
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Log.Error(ex);
                        }
                        finally
                        { }
                    }
                }
            }
        }
        private void LogBCMContactsV4()
        {
            try
            {
                using (var context = new MSSampleBusinessEntities())
                {
                    //#if !DEBUG
                    //                    context.Database.Connection.ConnectionString = ConnectionString;
                    //#else
                    //                    //NOTE Use this for mounting a LocalDB db during debugging
                    //                    object dbPath = AppDomain.CurrentDomain.GetData("DataDirectory");
                    //                    if (dbPath == null)
                    //                    {
                    //                        string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    //                        if (Environment.MachineName == "PRECISION")
                    //                            path = @"C:\Professional\Projects\Microsoft\BCM Migration Tool\BCM Migration Tool";
                    //                        AppDomain.CurrentDomain.SetData("DataDirectory", path);
                    //                        dbPath = AppDomain.CurrentDomain.GetData("DataDirectory");
                    //                    }
                    //#endif

                    //NOTE Can also use BCM_BusinessContacts_Core.sql in Resources instead of EF
                    try
                    {
                        context.Database.Connection.ConnectionString = ConnectionString;

                        var contacts = context.ContactFullViews.Where(mycontacts => mycontacts.Type == 1 && mycontacts.IsDeletedLocally == false);

                        List<ContactFullView> bcmContacts = new List<ContactFullView>();

                        Log.InfoFormat("Found {0} BCM Contacts:", contacts.Count());
                        OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} BCM Contacts:", contacts.Count()), HelperEventArgs.EventTypes.Status));

                        foreach (var contact in contacts)
                        {
                            try
                            {
                                if (LogRecordNames)
                                    OnGetItemComplete(null, new HelperEventArgs(String.Format(" -{0} (Company: {1})", contact.FullName, contact.CompanyName), HelperEventArgs.EventTypes.Status));
                                Log.DebugFormat(" -{0} (Company: {1})", contact.FullName, contact.CompanyName);
                                bcmContacts.Add(contact);
                            }
                            catch (System.Exception ex)
                            {
                                Log.Error(ex);
                            }
                            finally
                            { }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error(ex);
                    }
                    finally
                    { }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
            }
            finally
            { }
        }
        private async Task RunCreateContactsAsync()
        {
            using (var context = new MSSampleBusinessEntities())
            {                
                try
                {
                    context.Database.Connection.ConnectionString = ConnectionString;

                    var contacts = from mycontact in context.ContactFullViews where !mycontact.IsDeletedLocally && mycontact.Type == 1 select mycontact;

                    if (TestMode)
                    {
                        NumberToProcess = TestingMaximum;
                        Log.DebugFormat("Test mode - importing to {0} maximum", TestingMaximum);
                        OnStart(null, new HelperEventArgs(String.Format("Importing Contacts (max {0} - TESTING MODE)", TestingMaximum), false));
                    }
                    else
                    {
                        NumberToProcess = contacts.Count();
                        OnStart(null, new HelperEventArgs("Importing Contacts...", false));
                    }
                    
                    int cnt = 0;
                    foreach (var contact in contacts)
                    {
                        if (TestMode && cnt >= TestingMaximum)
                            break;
                        if (Cancelled)
                        {
                            Log.Warn("Cancelled!");
                            break;
                        }

                        //TESTED Check to see if there is an existing previously imported BCM contact (using only BCM ID) – if it exists, then update it.
                        //TESTED If there is no contact with BCM ID, then if a contact exists with same email address – if it exists, then update it + add the BCM ID to that contact
                        //TESTED If there is no contact with email OR BCM ID, then create the contact 

                        ImportModes importMode;

                        //Look for a Company with the same BCMID
                        OCMContact2Value ocmContact = null;
                        string bcmID = "";
                        
                        
                        ocmContact = GetOCMContact(contact.EntryGUID.ToString(), UpdateKeyTypes.BCMID);

                        if (ocmContact == null)
                        {
                            //Look for matching Company on name match
                            ocmContact = GetOCMContact(contact.FullName, UpdateKeyTypes.Name);
                        }

                        if (ocmContact != null)
                        {
                            //TESTED Must distinguish between existing manually created Companies (no BCMID) and those previously imported (with BCMID)

                            bcmID = ocmContact.GetBCMID();
                            if (!String.IsNullOrEmpty(bcmID))
                            {
                                //Previously imported - IGNORE
                                Log.DebugFormat("Contact '{0}' previously imported; skipping", ocmContact.DisplayName);
                                OnCreateItemComplete(null, new HelperEventArgs(String.Format("Contact '{0}' previously imported; skipping", ocmContact.DisplayName), false));
                                continue;
                            }
                            else
                            {
                                //Update existing manually created item
                                Log.DebugFormat("Updating Contact '{0}'", ocmContact.DisplayName);
                                importMode = ImportModes.Update;
                            }
                        }
                        else
                        {
                            importMode = ImportModes.Create;
                        }

                        //Get company name by join on Account
                        string companyName = "";
                        using (var context2 = new MSSampleBusinessEntities())
                        {
                            context2.Database.Connection.ConnectionString = ConnectionString;
                            var account = context2.AccountsFullViews.SingleOrDefault(accounts => accounts.EntryGUID == contact.ParentEntryID);
                            //1.0.15 Wasn't checking for null account
                            if (account != null)
                            {
                                companyName = account.FullName;
                            }
                            else
                            {
                                Log.VerboseFormat("No company match for contact '{0}' (ParentEntryID: {1}; ContactServiceID: {2})", contact.FullName, contact.ParentEntryID, contact.ContactServiceID);
                            }
                        }
                                                    
                        await ImportContactAsync(contact, companyName, importMode, ocmContact);
                        cnt += 1;                        
                    }

                    if (!Cancelled)
                    {
                        OnHelperComplete(null, new HelperEventArgs(String.Format("Contacts import complete!{0}Created: {1}{0}Errors: {2}", Environment.NewLine, NumberCreated, NumberOfErrors), false));
                    }
                    else
                    {
                        OnHelperComplete(null, new HelperEventArgs(String.Format("Contacts import stopped{0}Created: {1}{0}Errors: {2}", Environment.NewLine, NumberCreated, NumberOfErrors), false));
                    }
                    Log.InfoFormat("Created {0} OCM Contacts ({1} Errors)", NumberCreated, NumberOfErrors);
                }
                catch (Exception ex)
                {
                    OnError(this, new HelperEventArgs(ex.ToString(), HelperEventArgs.EventTypes.FatalError));

                }
            }            
        }
        private async Task RunCreateContactsAsyncV3()
        {
            using (Log.VerboseCall())
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        con.Open();

                        using (SqlCommand comA = new SqlCommand(Resources.BCM_ContactFullViewCount_V3, con))
                        {
                            NumberToProcess = Convert.ToInt32(comA.ExecuteScalar());
                        }

                        using (SqlCommand comB = new SqlCommand(Resources.BCM_ContactFullView_V3, con))
                        {
                            //con.Open();
                            using (DbDataReader reader = comB.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    Log.Error("Unexpected - no records");
                                    return;
                                }

                                if (TestMode)
                                {
                                    NumberToProcess = TestingMaximum;
                                    Log.DebugFormat("Test mode - importing to {0} maximum", TestingMaximum);
                                    OnStart(null, new HelperEventArgs(String.Format("Importing Contacts (max {0} - TESTING MODE)", TestingMaximum), false));
                                }
                                else
                                {                                    
                                    OnStart(null, new HelperEventArgs("Importing Contacts...", false));
                                }

                                while (reader.Read())
                                {
                                    int cnt = 0;
                                    if (TestMode && cnt >= TestingMaximum)
                                        break;
                                    if (Cancelled)
                                    {
                                        Log.Warn("Cancelled!");
                                        break;
                                    }

                                    ContactFullView contact = new ContactFullView();
                                    ImportModes importMode;
                                    OCMContact2Value ocmContact = null;
                                    string bcmID = "";

                                    try
                                    {
                                        contact.EntryGUID = Guid.Parse(Convert.ToString(reader["EntryGUID"]));
                                        if (!DBNull.Value.Equals(reader["ParentEntryID"]))
                                            contact.ParentEntryID = Guid.Parse(Convert.ToString(reader["ParentEntryID"]));

                                        contact.ContactServiceID = Convert.ToInt32(reader["ContactServiceID"]);

                                        try
                                        {
                                            if (!DBNull.Value.Equals(reader["FullName"]))
                                                contact.FullName = reader["FullName"].ToString();
                                            if (!DBNull.Value.Equals(reader["FirstName"]))
                                                contact.FirstName = reader["FirstName"].ToString();
                                            if (!DBNull.Value.Equals(reader["LastName"]))
                                                contact.LastName = reader["LastName"].ToString();
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Log.Error(ex);
                                        }

                                        try
                                        {
                                            if (!DBNull.Value.Equals(reader["Birthday"]))
                                                contact.Birthday = Convert.ToDateTime(reader["Birthday"]);
                                            if (!DBNull.Value.Equals(reader["JobTitle"]))
                                                contact.JobTitle = reader["JobTitle"].ToString();
                                            if (!DBNull.Value.Equals(reader["ContactNotes"]))
                                                contact.ContactNotes = reader["ContactNotes"].ToString();
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Log.Error(ex);
                                        }
                                        finally
                                        { }

                                        try
                                        {
                                            if (!DBNull.Value.Equals(reader["Email1Address"]))
                                                contact.Email1Address = reader["Email1Address"].ToString();
                                            if (!DBNull.Value.Equals(reader["Email1DisplayAs"]))
                                                contact.Email1DisplayAs = reader["Email1DisplayAs"].ToString();
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Log.Error(ex);
                                        }
                                        finally
                                        { }

                                        try
                                        {
                                            if (!DBNull.Value.Equals(reader["WorkAddressStreet"]))
                                                contact.WorkAddressStreet = reader["WorkAddressStreet"].ToString();
                                            if (!DBNull.Value.Equals(reader["WorkAddressCity"]))
                                                contact.WorkAddressCity = reader["WorkAddressCity"].ToString();
                                            if (!DBNull.Value.Equals(reader["WorkAddressCountry"]))
                                                contact.WorkAddressCountry = reader["WorkAddressCountry"].ToString();
                                            if (!DBNull.Value.Equals(reader["WorkAddressZip"]))
                                                contact.WorkAddressZip = reader["WorkAddressZip"].ToString();
                                            if (!DBNull.Value.Equals(reader["WorkAddressState"]))
                                                contact.WorkAddressState = reader["WorkAddressState"].ToString();
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Log.Error(ex);
                                        }
                                        finally
                                        { }

                                        try
                                        {
                                            if (!DBNull.Value.Equals(reader["WorkPhoneNum"]))
                                                contact.WorkPhoneNum = reader["WorkPhoneNum"].ToString();
                                            if (!DBNull.Value.Equals(reader["HomePhoneNum"]))
                                                contact.HomePhoneNum = reader["HomePhoneNum"].ToString();
                                            if (!DBNull.Value.Equals(reader["MobilePhoneNum"]))
                                                contact.MobilePhoneNum = reader["MobilePhoneNum"].ToString();
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Log.Error(ex);
                                        }
                                        finally
                                        { }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        Log.Error(ex);
                                    }

                                    //Look for a Company with the same BCMID
                                    ocmContact = GetOCMContact(contact.EntryGUID.ToString(), UpdateKeyTypes.BCMID);

                                    if (ocmContact == null)
                                    {
                                        //Look for matching Company on name match
                                        ocmContact = GetOCMContact(contact.FullName, UpdateKeyTypes.Name);
                                    }

                                    if (ocmContact != null)
                                    {
                                        //TESTED Must distinguish between existing manually created Companies (no BCMID) and those previously imported (with BCMID)

                                        bcmID = ocmContact.GetBCMID();
                                        if (!String.IsNullOrEmpty(bcmID))
                                        {
                                            //Previously imported - IGNORE
                                            Log.DebugFormat("Contact '{0}' previously imported; skipping", ocmContact.DisplayName);
                                            OnCreateItemComplete(null, new HelperEventArgs(String.Format("Contact '{0}' previously imported; skipping", ocmContact.DisplayName), false));
                                            continue;
                                        }
                                        else
                                        {
                                            //Update existing manually created item
                                            Log.DebugFormat("Updating Contact '{0}'", ocmContact.DisplayName);
                                            importMode = ImportModes.Update;
                                        }
                                    }
                                    else
                                    {
                                        importMode = ImportModes.Create;
                                    }

                                    //Get company name by join on Account
                                    string companyName = "";
                                    using (var context2 = new MSSampleBusinessEntities())
                                    {
                                        context2.Database.Connection.ConnectionString = ConnectionString;
                                        var account = context2.AccountsFullViews.SingleOrDefault(accounts => accounts.EntryGUID == contact.ParentEntryID);
                                        //1.0.15 Wasn't checking for null account
                                        if (account != null)
                                        {
                                            companyName = account.FullName;
                                        }
                                        else
                                        {
                                            Log.VerboseFormat("No company match for contact '{0}' (ParentEntryID: {1}; ContactServiceID: {2})", contact.FullName, contact.ParentEntryID, contact.ContactServiceID);
                                        }
                                    }

                                    await ImportContactAsync(contact, companyName, importMode, ocmContact);
                                    cnt += 1;
                                }
                            }
                        }
                    }

                    if (!Cancelled)
                    {
                        OnHelperComplete(null, new HelperEventArgs(String.Format("Contacts import complete!{0}Created: {1}{0}Errors: {2}", Environment.NewLine, NumberCreated, NumberOfErrors), false));
                    }
                    else
                    {
                        OnHelperComplete(null, new HelperEventArgs(String.Format("Contacts import stopped{0}Created: {1}{0}Errors: {2}", Environment.NewLine, NumberCreated, NumberOfErrors), false));
                    }
                    Log.InfoFormat("Created {0} OCM Contacts ({1} Errors)", NumberCreated, NumberOfErrors);
                }
                catch (System.Exception ex)
                {
                    OnError(this, new HelperEventArgs(ex.ToString(), HelperEventArgs.EventTypes.FatalError));
                }
            }
        }
        private async Task RunGetOCMContactsAsync()
        {
            using (Log.VerboseCall())
            {
                do
                {                    
                    await GetOCMContactsAsync(false);                    
                } while (!String.IsNullOrEmpty(LastPageResult));

                //TESTED We need another run of GetOCMContactsAsync, using MigrationHelper.ContactsFolderID to capture any OCM Contacts that have been shared!          
                //HIGH 3/15/2018 REALLY?? Was this only needed because we were incorrectly doing a MOVE instead of a COPY?
                //do
                //{
                //    await GetOCMContactsAsync(true);
                //} while (!String.IsNullOrEmpty(LastPageResult));

                Debug.WriteLine("Leaving GetOCMCompaniesAsync()");
            }
        }

        public async Task RunGetTemplateAsync()
        {

            using (Log.VerboseCall())
            {
                OnStart(null, new HelperEventArgs("Retrieving Contacts template...", HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
                ContactTemplate = await GetTemplateAsync();
                FieldMappings.OCMBusinessContactFields.ContactTemplate = ContactTemplate;
            }
        }        
        //private async Task UpdateContactAsync(OCMContact.Rootobject contact)
        //{
        //    //REVIEW Not implemented
        //    var json = JsonConvert.SerializeObject(contact);

        //    try
        //    {
        //        Uri uri = new Uri(String.Format("{0}('{1}')", Properties.Settings.Default.V2EndPoint, contact.Id));

        //        HttpMethod method = new HttpMethod("PATCH");
        //        HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpRequestMessage request = new HttpRequestMessage(method, uri){Content = httpContent};

        //        PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.JSON);

        //        Debug.WriteLine("Request to: " + uri.ToString());

        //        try
        //        {
        //            using (var response = await _httpClient.SendAsync(request))
        //            {
        //                Debug.WriteLine(response);
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    OnPatchComplete(null, new HelperEventArgs(String.Format("Contact {0} updated", contact.DisplayName), HelperEventArgs.EventTypes.Status));
        //                }
        //            }
        //        }
        //        catch (TaskCanceledException e)
        //        {
        //            Debug.WriteLine("ERROR: " + e.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}
#endregion
    }
}
