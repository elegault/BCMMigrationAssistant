using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        //internal List<string> BCMContacts { get; set; }
        internal List<ContactFullView> BCMContacts { get; set; }
        internal OCMCompanyTemplate.Rootobject ContactTemplate { get; set; }
        private string LastPageResult { get; set; }
        internal static List<OCMContact2Value> OCMContacts { get; set; }
        //internal OCMContact.Rootobject OCMContacts { get; set; }
        #endregion
        #region Methods
        private async Task<bool> CreateCompanyLink(string itemLinkID, string companyXrmID)
        {
            string xmlRequest = Resources.CreateXrmGraphRelationshipRequest;

            Uri uri = new Uri(Settings.Default.EWSEndPoint);

            xmlRequest = xmlRequest.Replace("{FROMENTITYID}", companyXrmID);
            xmlRequest = xmlRequest.Replace("{FROMENTITYTYPE}", "XrmOrganization");
            xmlRequest = xmlRequest.Replace("{TOENTITYID}", itemLinkID);
            xmlRequest = xmlRequest.Replace("{TOENTITYTYPE}", "Person");
            xmlRequest = xmlRequest.Replace("{LINKTYPE}", "WorksFor");
            
            try
            {
                Log.VerboseFormat("Posting to {1}:{2}{0}{2}", xmlRequest, uri, Environment.NewLine);
                PrepareRequest(RequestDataTypes.Links, RequestDataFormats.XML);
                using (
                 var response = await _httpClient.PostAsync(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml"))
             )
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Log.Error(content);
                        OnError(null, new HelperEventArgs(String.Format("ERROR: {0}", content), HelperEventArgs.EventTypes.Error));
                        NumberOfErrors += 1;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
        internal async Task CreateCompanyLinks()
        {
            using (Log.VerboseCall())
            {
                NumberToProcess = OCMContacts.Count;
                Log.InfoFormat("Linking {0} contacts", OCMContacts.Count);
                OnStart(null, new HelperEventArgs("Linking contacts...", HelperEventArgs.EventTypes.Status));

                foreach (var contact in OCMContacts)
                {
                    try
                    {
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

                            OnIncrementProgress(null, new HelperEventArgs("", HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Migrating));

                            if (existingLink || String.IsNullOrEmpty(company.XrmId))
                            {
                                Log.VerboseFormat("Existing link for contact '{0}' (ItemLinkID: {1}); skipping", contact.DisplayName, contact.ItemLinkId);
                                continue;
                            }

                            bool linked = await CreateCompanyLink(contact.ItemLinkId, company.XrmId);
                            if (linked)
                            {
                                Log.DebugFormat("Contact '{0}' linked to company '{1}'", contact.DisplayName, contact.CompanyName);
                                OnDisplayMessage(null,
                                    new HelperEventArgs(
                                        String.Format("Contact '{0}' linked to company '{1}'", contact.DisplayName,
                                            contact.CompanyName), HelperEventArgs.EventTypes.Status));
                            }
                        }
                        else
                        {
                            //Ignore if it has a BCMID?
                            string bcmID = contact.GetBCMID();
                            if (!String.IsNullOrEmpty(bcmID))
                            {
                                Log.ErrorFormat("No item link ID for Contact '{0}' (company '{1}')", contact.DisplayName, contact.CompanyName);
                                OnError(null, new HelperEventArgs(String.Format("No item link ID for Contact '{0}' (company '{1}')", contact.DisplayName,
                                    contact.CompanyName), HelperEventArgs.EventTypes.Error));}
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        OnError(null, new HelperEventArgs(String.Format("Unknown error in  CreateCompanyLinks for Contact '{0}' (company '{1}')", contact.DisplayName,
                            contact.CompanyName), HelperEventArgs.EventTypes.Error));
                    }

                }

                Log.InfoFormat("{0} contacts processed for linking", OCMContacts.Count);
                OnGetComplete(null, new HelperEventArgs(String.Format("{0} contacts processed for linking", OCMContacts.Count), HelperEventArgs.EventTypes.Status));

            }
        }
        internal async Task CreateContacts()
        {
            Cancelled = false;
            await RunCreateContactsAsync();
                
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
                        using (var response = await _httpClient.PostAsync(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml")))
                        {
                            var content = await response.Content.ReadAsStringAsync();

                            Log.DebugFormat("FindPeople (offset: {0}): {1}", offset, content);
                            //Debug.WriteLine(content);

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

                                    element1 = (XElement) personaNode;
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
                                    var myelement = from anElement in elementsB.Descendants
                                            ("{http://schemas.microsoft.com/exchange/services/2006/types}ItemLinkIds")
                                        select anElement.Value;
                                    try
                                    {
                                        //HACK HACK HACK!!! Must remove trailing 1!
                                        ocmContact.ItemLinkId = myelement.ElementAt(idIndexes);
                                        idIndexes += 1;
                                        //ocmContact.ItemLinkId = myelement.First();
                                        ocmContact.ItemLinkId = ocmContact.ItemLinkId.Substring(0, ocmContact.ItemLinkId.Length - 1);
                                        Log.DebugFormat("{0} (ItemLinkID: {1})", ocmContact.DisplayName, ocmContact.ItemLinkId);
                                        //Try .Value.Value? or .Child.Child, or folderName.First. Still doesn't work! Just use above hack
                                        //ALSO TRY: folderName.Descendants("Value") .Descendants("Value") 
                                        //result = element.Descendants("Value").Descendants("Value").ToString();
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
                        }                       
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
           
            try
            {

                OnStart(null, new HelperEventArgs("Getting BCM Contacts data", HelperEventArgs.EventTypes.Status));

                using (var context = new MSSampleBusinessEntities())
                {
                    context.Database.Connection.ConnectionString = ConnectionString;

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

                    //TESTED Use LINQ instead
                    //NOTE Can also use BCM_BusinessContacts_Core.sql in Resources instead of EF
                                        
                    var contacts = context.ContactFullViews.Where(mycontacts => mycontacts.Type == 1 && mycontacts.IsDeletedLocally == false);
                    
                    BCMContacts = new List<ContactFullView>();

                    Log.DebugFormat("Found {0} BCM Contacts:", contacts.Count());
                    OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} BCM Contacts:", contacts.Count()), HelperEventArgs.EventTypes.Status));

                    foreach (var contact in contacts)
                    {                        
                        OnGetItemComplete(null, new HelperEventArgs(String.Format(" -{0} (Company: {1})", contact.FullName, contact.CompanyName), HelperEventArgs.EventTypes.Status));
                        BCMContacts.Add(contact);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                OnError(null, new HelperEventArgs(String.Format("Error in ContactsHelper.GetBCmContacts: {0}", ex.Message), HelperEventArgs.EventTypes.Error));                
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

            OCMContacts = new List<OCMContact2Value>();
            await RunGetOCMContactsAsync();
            bool headerShown = false;

            //OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} existing contacts (* = previously imported):", OCMContacts?.Count), HelperEventArgs.EventTypes.Status));
            //OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} existing OCM contacts", OCMContacts?.Count), HelperEventArgs.EventTypes.Status));

            Log.InfoFormat("Total contacts: {0}", OCMContacts.Count);

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
                    output = String.Format(" -{3}{0}{2}{1}", item.DisplayName,
                    !String.IsNullOrEmpty(bcmID) ? String.Format(" (BCM ID: {0})", bcmID) : "",
                        String.IsNullOrEmpty(item.CompanyName?.ToString()) ? "" : ": " + item.CompanyName,
                        !String.IsNullOrEmpty(bcmID) ? "" : "");
                    if (!String.IsNullOrEmpty(bcmID)) //Only output previously imported contacts to log window
                    {
                        if (!headerShown)
                        {                            
                            OnGetComplete(null, new HelperEventArgs("Previously imported contacts:", HelperEventArgs.EventTypes.Status));
                            headerShown = true;
                        }
                        Log.DebugFormat(output);
                        OnGetItemComplete(null, new HelperEventArgs(output, HelperEventArgs.EventTypes.Status));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
        private async Task GetOCMContactsAsync(bool getSharedContacts)
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
                        request = String.Format("{0}/contactfolders/{2}/contacts?$top=50&$expand=SingleValueExtendedProperties($filter=PropertyId%20eq%20'String%20{1}%20Name%20BCMID')", Properties.Settings.Default.V2EndPoint, FieldMappings.BCMIDPropertyGUID, MigrationHelper.ContactsFolderID.Replace("/", "-"));
                    }
                    else
                    {
                        request = String.Format("{0}/contacts?$top=50&$expand=SingleValueExtendedProperties($filter=PropertyId%20eq%20'String%20{1}%20Name%20BCMID')", Properties.Settings.Default.V2EndPoint, FieldMappings.BCMIDPropertyGUID);
                    }
                }

                //Debug.WriteLine("Request to : " + request);
                PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.None);
                using (var response = await _httpClient.GetAsync(request))
                {
                    // Check status code.
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        OnError(null, new HelperEventArgs(String.Format("ERROR in GetOCMContactsAsync(): {0}", responseContent), HelperEventArgs.EventTypes.Error));
                        NumberOfErrors += 1;
                        return;
                    }

                    // Read and deserialize response.                
                    var content = await response.Content.ReadAsStringAsync();

                    OCMContact2 contacts = JsonConvert.DeserializeObject<OCMContact2>(content);

                    if (getSharedContacts == true)
                    {
                        foreach (var contact in contacts.value)
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
                        OCMContacts = contacts.value.ToList(); //contacts2.value.ToList();
                        //Debug.WriteLine(String.Format("{0} OCM Companies retrieved", OCMContacts.Count));
                    }
                    else
                    {
                        //Append result to previously set collection
                        OCMContacts.AddRange(contacts.value.ToList());
                        //Debug.WriteLine(String.Format("Added {0} more OCM Companies", contacts.value.Length));
                    }

                    Debug.WriteLine(contacts.odatanextLink);
                    if (!String.IsNullOrEmpty(contacts.odatanextLink))
                    {
                        if (contacts.odatanextLink == LastPageResult) //Done paging
                        {
                            //This doesn't fire
                            OnGetItemComplete(null, new HelperEventArgs(String.Format("Fetched {0} contacts", OCMContacts.Count), HelperEventArgs.EventTypes.Status));
                            return;
                        }
                        //Debug.WriteLine(String.Format("Setting page: {0}", contacts.odatanextLink));
                        LastPageResult = contacts.odatanextLink.ToString();
                    }
                    else
                    {
                        //Done paging
                        //NOTE: This method is actually run twice, and logs 2 total amounts so misleading
                        //OnGetItemComplete(null, new HelperEventArgs(String.Format("Fetched {0} total contacts", OCMContacts.Count), HelperEventArgs.EventTypes.Status));
                        LastPageResult = null;
                    }
                }                
            }
            catch(System.Exception ex)
            {
                
                //return null;
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
                        addProp = new SingleValueExtendedProperty();
                        addProp.PropertyId = "String 1a417774-4779-47c1-9851-e42057495fca Name XrmSharingSourceUserDisplayName";
                        addProp.Value = MigrationHelper.CurrentUser.DisplayName;
                        ocmContact.SingleValueExtendedProperties[1] = addProp;

                        addProp = new SingleValueExtendedProperty();
                        addProp.PropertyId = "String 1a417774-4779-47c1-9851-e42057495fca Name XrmSharingSourceUser";
                        addProp.Value = MigrationHelper.CurrentUserID;
                        ocmContact.SingleValueExtendedProperties[2] = addProp;

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
                    if (FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields.Count == 0)
                        //Just the BCMID, no custom fields
                    {                        
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
                                                                Log.VerboseFormat("Skipping field '{0}'", fieldName);
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
                        ocmContact.Birthday = existingContact.Birthday;
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
                        ocmContact.Surname = existingContact.Surname.ToString();
                        ocmContact.Title = existingContact.Title;
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
                            ocmContact.GivenName = contact.FirstName;
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
                            if (!String.IsNullOrEmpty(contact.WorkPhoneNum))
                            {
                                if (ocmContact.BusinessPhones != null)
                                {
                                    //Existing contact - check if phone number exists
                                    if (!ocmContact.BusinessPhones.Contains(contact.WorkPhoneNum))
                                    {
                                        string[] phones = ocmContact.BusinessPhones;
                                        Array.Resize(ref phones, ocmContact.BusinessPhones.Length + 1);
                                        phones[phones.Length + 1] = contact.WorkPhoneNum;
                                        ocmContact.BusinessPhones = phones;
                                    }
                                }
                                else
                                {
                                    ocmContact.BusinessPhones = new string[1];
                                    ocmContact.BusinessPhones[0] = contact.WorkPhoneNum;
                                }
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
                                if (ocmContact.HomePhones != null)
                                {
                                    //Existing contact - check if phone number exists
                                    if (!ocmContact.HomePhones.Contains(contact.HomePhoneNum))
                                    {
                                        string[] phones = ocmContact.HomePhones;
                                        Array.Resize(ref phones, ocmContact.HomePhones.Length + 1);
                                        phones[phones.Length + 1] = contact.HomePhoneNum;
                                        ocmContact.HomePhones = phones;
                                    }
                                }
                                else
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

                            Log.VerboseFormat("Posting to {1}:{0}", json, uri);
                            PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.JSON);
                            using (var response = await _httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json")))
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                // Check status code.
                                if (!response.IsSuccessStatusCode)
                                {
                                    Log.ErrorFormat("Error creating '{1}': {0}", content, contact.FullName);
                                    OnError(null,
                                        new HelperEventArgs(String.Format("ERROR creating '{1}': {0}", content, contact.FullName),
                                            HelperEventArgs.EventTypes.Error));
                                    NumberOfErrors += 1;
                                }
                                else
                                {
                                    //TESTED Get response with Contact details so we can get the ID for use in moving operation to share it
                                    Log.DebugFormat("Created Contact '{0}' (BCMID: {1})'", contact.FullName, contact.EntryGUID.ToString());
                                    OnCreateItemComplete(null, new HelperEventArgs(String.Format("Created Contact '{0}'...", contact.FullName), false));
                                    NumberCreated += 1;

                                    OCMContact2Value newContact = JsonConvert.DeserializeObject<OCMContact2Value>(content);
                                    newContact.BCMID = contact.EntryGUID.ToString();
                                    OCMContacts.Add(newContact);

                                    //To make a Contact or Company shared: make a REST call to ‘/move’ with the folder-id of the sharing child-folder, not the modern-group mailbox guid.
                                    //NOTE If doing a PATCH, do we need to check if it has already been shared so as not to 'move' it again?

                                    //REVIEW We could gather all these IDs and do the sharing operation all in one call by creating ItemID elements for each contact to put in the ItemIds element
                                    PrepareRequest(RequestDataTypes.Companies, RequestDataFormats.XML);
                                    uri = new Uri(Settings.Default.EWSEndPoint);

                                    string id = newContact.Id.Replace("_", "+");
                                    id = id.Replace("-", "/"); //HACK to make the MoveItem operation work (ErrorInvalidIdMalformed)
                                    string xmlRequest = String.Format(Resources.ShareContactsRequest, MigrationHelper.ContactsFolderID,
                                        id);

                                    using (var moveResponse = await _httpClient.PostAsync(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml")))
                                    {
                                        if (!moveResponse.IsSuccessStatusCode) // Check status code.
                                        {
                                            var moveResponseContent = await moveResponse.Content.ReadAsStringAsync();
                                            Log.ErrorFormat("Error sharing '{1}': {0}", moveResponseContent, contact.FullName);
                                            OnError(null,
                                                new HelperEventArgs(
                                                    String.Format("ERROR sharing '{1}': {0}", moveResponseContent, contact.FullName),
                                                    HelperEventArgs.EventTypes.Error));
                                            NumberOfErrors += 1;
                                        }
                                        else
                                        {
                                            //Log message?
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            uri = new Uri(String.Format("{0}/contacts('{1}')?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUserDisplayName'))", Settings.Default.V2EndPoint, existingContact.Id));

                            HttpMethod method = new HttpMethod("PATCH");
                            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpRequestMessage request = new HttpRequestMessage(method, uri) { Content = httpContent };

                            //Debug.WriteLine(String.Format("Patching to {1}:{2}{0}{2}Token:{2}{3}", json, uri, Environment.NewLine, AccessToken));

                            PrepareRequest(RequestDataTypes.Contacts, RequestDataFormats.JSON);
                            using (var response = await _httpClient.SendAsync(request))
                            {
                                if (!response.IsSuccessStatusCode) // Check status code.
                                {
                                    //BUGWATCH "{\"error\":{\"code\":\"ErrorIncorrectUpdatePropertyCount\",\"message\":\"An object within a change description must contain one and only one property to modify.\"}}" Happens with Mr. Syed Abbas; was ImportModes.Update; maybe problem with sharing props that aren't needed?                                    
                                    var content = await response.Content.ReadAsStringAsync();
                                    Log.ErrorFormat("Error updating '{1}': {0}", content, contact.FullName);
                                    OnError(null, new HelperEventArgs(String.Format("ERROR updating '{1}': {0}", content, contact.FullName), HelperEventArgs.EventTypes.Error));
                                    NumberOfErrors += 1;
                                }
                                else
                                {
                                    OnPatchComplete(null, new HelperEventArgs(String.Format("Updated Contact '{0}'...", contact.FullName), false));
                                    NumberUpdated += 1;
                                    //TEST Do we need to set the BCMID on an existing contact?
                                    existingContact.BCMID = contact.EntryGUID.ToString();
                                }
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

        private async Task RunCreateContactsAsync()
        {
            using (var context = new MSSampleBusinessEntities())
            {                
                try
                {
                    context.Database.Connection.ConnectionString = ConnectionString;

                    var contacts = from mycontact in context.ContactFullViews where !mycontact.IsDeletedLocally && mycontact.Type == 1 select mycontact;
                    //var contacts = (from a in context.ContactFullViews join c in context.AccountsFullViews on a.ParentEntryID equals c.EntryGUID select new { CompanyName = c.FullName, Contact = a });
                    if (TestMode)
                    {
                        NumberToProcess = TestingMaximum;
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
                        if (TestMode && cnt == TestingMaximum)
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
                            companyName = account.FullName;
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
                }
                catch (Exception ex)
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
                    //Debug.WriteLine("Calling GetOCMCompaniesAsync()");
                    await GetOCMContactsAsync(false);
                    //TESTED We need another run of GetOCMContactsAsync, using MigrationHelper.ContactsFolderID to capture any OCM Contacts that have been shared!          
                    //Debug.WriteLine(String.Format("GetOCMCompaniesAsync() returned; LastPageResult = {0}", LastPageResult));
                } while (!String.IsNullOrEmpty(LastPageResult));

                do
                {
                    await GetOCMContactsAsync(true);
                } while (!String.IsNullOrEmpty(LastPageResult));
            
                Debug.WriteLine("Leaving GetOCMCompaniesAsync()");
            }
        }

        public async Task RunGetTemplateAsync()
        {

            OnStart(null, new HelperEventArgs("Retrieving Contacts template...", HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
            ContactTemplate = await GetTemplateAsync();
            FieldMappings.OCMBusinessContactFields.ContactTemplate = ContactTemplate;
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
