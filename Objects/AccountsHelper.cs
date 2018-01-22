using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BcmMigrationTool;
using BCM_Migration_Tool.Properties;
using Newtonsoft.Json;
using TracerX;

namespace BCM_Migration_Tool.Objects
{
    internal class AccountsHelper: HelperBase
    {

        #region Constructors
        internal AccountsHelper(string accessToken, string connectionString)
            : base(accessToken, connectionString)
        {
        }
        #endregion
        #region Fields
        //private List<string> _bcmAccounts;
        //private bool cancelled;
        private static readonly Logger Log = Logger.GetLogger("AccountsHelper");
        #endregion
        #region Properties

        internal List<AccountsFullView> BCMAccountsFullView { get; set; }
        internal OCMCompanyTemplate.Rootobject CompanyTemplate { get;set; }
        private int PageSkip { get; set; }
        internal static List<OCMCompany2Value> OCMAccounts { get; set; } 

        #endregion
        #region Methods
        internal async Task CreateCompanies()
        {
            Cancelled = false;
            await RunCreateCompanyAsync();
        }
        internal async Task GetBCMAccounts()
        {
            using (Log.VerboseCall())
            {
                try
                {
                    OnStart(null, new HelperEventArgs("Getting BCM Account data", HelperEventArgs.EventTypes.Status));
                    using (var context = new MSSampleBusinessEntities())
                    {
                        context.Database.Connection.ConnectionString = ConnectionString;

                        //NOTE Can use BCM_Accounts_Core.sql in Resources instead of Entity Framework, but this is fine
                        var companies = context.AccountsFullViews.Where(a => (!a.IsDeletedLocally) && (a.AccountActive)).ToList();
                        BCMAccountsFullView = companies; //REVIEW BCMAccountsFullView is set but not referenced
                        Log.InfoFormat("Found {0} BCM Accounts: ", companies.Count());
                        OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} BCM Accounts: ", companies.Count()), HelperEventArgs.EventTypes.Status));
                        foreach (var company in companies)
                        {                            
                            OnGetItemComplete(null, new HelperEventArgs(String.Format(" -{0}", company.FileAs), HelperEventArgs.EventTypes.Status));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }   

                Debug.WriteLine("Leaving GetBCMAccounts");   
            }      
        }
        internal async Task GetOCMCompanies()
        {
            using (Log.VerboseCall())
            {
                OCMAccounts = new List<OCMCompany2Value>();
                await RunGetOCMCompaniesAsync(); //NOTE: Do NOT use .Wait() unless the caller is an async method, or else it will hang

                //OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} existing OCM Companies (* = previously imported):", OCMAccounts?.Count), HelperEventArgs.EventTypes.Status));
                Log.InfoFormat("Found {0} existing OCM Companies", OCMAccounts?.Count);
                OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} existing OCM Companies", OCMAccounts?.Count), HelperEventArgs.EventTypes.Status));

                bool headerShown = false;
                foreach (OCMCompany2Value item in OCMAccounts)
                {
                    if (item.SingleValueExtendedProperties != null)
                    {
                        if (!headerShown)
                        {
                            OnGetComplete(null, new HelperEventArgs("Previously imported companies:", HelperEventArgs.EventTypes.Status));
                            headerShown = true;
                        }

                        OnGetItemComplete(null, new HelperEventArgs(String.Format(" -{0} (BCM ID: {1})", item.DisplayName, item.GetBCMID()), HelperEventArgs.EventTypes.Status));
                        Log.DebugFormat("{0} (BCMID: {1}; XRMID: {2})", item.DisplayName, item.BCMID, item.XrmId);
                    }
                    else
                    {
                        //OnGetItemComplete(null, new HelperEventArgs(String.Format(" -{0}", item.DisplayName), HelperEventArgs.EventTypes.Status));
                    }
                }
                Debug.WriteLine("Leaving GetBCMAccounts");
            }
        }
        private async Task GetOCMCompaniesAsync()
        {
            try
            {
                string request;
         
                if (PageSkip == 0)
                {
                    request = String.Format("{0}/XrmOrganizations?$top=50&$expand=SingleValueExtendedProperties($filter=PropertyId%20eq%20'String%20{1}%20Name%20BCMID')", Properties.Settings.Default.BetaEndPoint, FieldMappings.BCMIDPropertyGUID);
                }
                else
                {
                    request = String.Format("{0}/XrmOrganizations?$top=50&$expand=SingleValueExtendedProperties($filter=PropertyId%20eq%20'String%20{1}%20Name%20BCMID')&$skip={2}", Properties.Settings.Default.BetaEndPoint, FieldMappings.BCMIDPropertyGUID, PageSkip);
                }
                
                PrepareRequest(RequestDataTypes.Companies, RequestDataFormats.None);
                using (var response = await _httpClient.GetAsync(request))
                {
                    // Check status code.
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        OnError(null, new HelperEventArgs(String.Format("ERROR in GetOCMCompaniesAsync(): {0}", responseContent), HelperEventArgs.EventTypes.Error));
                        NumberOfErrors += 1;
                        return;
                    }
                    // Read and deserialize response.    
                    var content = await response.Content.ReadAsStringAsync();

                    OCMCompany2 contacts2 = null;

                    try
                    {
                        contacts2 = JsonConvert.DeserializeObject<OCMCompany2>(content);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);

                    }

                    if (OCMAccounts == null)
                        OCMAccounts = new List<OCMCompany2Value>();

                    foreach (var company in contacts2.value)
                    {
                        OCMAccounts.Add(company);
                    }


                    //if (!String.IsNullOrEmpty(contacts2.odatanextLink?.ToString()))
                    //{
                    //    if (contacts2.odatanextLink == LastPageResult) //Done paging
                    //    {
                    //        OnGetItemComplete(null, new HelperEventArgs(String.Format("Fetched {0} OCM Companies", OCMAccounts.Count), HelperEventArgs.EventTypes.Status));
                    //        return;
                    //    }
                    //    //Debug.WriteLine(String.Format("Setting page: {0}", contacts2.odatanextLink));
                    //    LastPageResult = contacts2.odatanextLink.ToString();
                    //}
                    //else
                    //{
                    //    //Done paging
                    //    LastPageResult = null;
                    //    return;
                    //}
                    if (contacts2.value.Length < 50)
                    {
                        //No more items to get
                        PageSkip = 0;
                        OnGetItemComplete(null, new HelperEventArgs(String.Format("Fetched {0} OCM Companies", OCMAccounts.Count), HelperEventArgs.EventTypes.Status));
                    }
                    else
                    {
                        PageSkip += 50;
                    }
                }                
            }
            catch (System.Exception ex)
            {

                //return null;
            }
            Debug.WriteLine("Leaving GetOCMCompaniesAsync");
        }

        internal static OCMCompany2Value GetOCMCompany(string val, UpdateKeyTypes keyType)
        {
            if (OCMAccounts == null)
                return null;

            try
            {
                switch (keyType)
                {
                    case UpdateKeyTypes.BCMID:
                        foreach (var account in OCMAccounts)
                        {
                            if (account.SingleValueExtendedProperties != null)
                            {
                                string bcmID = account.GetBCMID();
                                if (!String.IsNullOrEmpty(bcmID) && bcmID.ToLower() == val.ToLower())
                                {
                                    return account;
                                }
                                //foreach (var prop in account.SingleValueExtendedProperties)
                                //{
                                //    //"String {bc013ba3-3a6d-4826-b0ec-cb703a722b09} Name BCMID"
                                //    if (prop.PropertyId.ToLower() == "string {bc013ba3-3a6d-4826-b0ec-cb703a722b09} name bcmid" && prop.Value == val)
                                //    {
                                //        return account;
                                //    }
                                //    //if (prop.PropertyId == FieldMappings.BCMIDPropertyGUID && prop.Value == val)
                                //    //    return account;
                                //}
                            }
                        }
                        break;
                    case UpdateKeyTypes.EmailAddress:
                        foreach (var account in OCMAccounts)
                        {
                            if (account.EmailAddresses != null)
                            {
                                foreach (var email in account.EmailAddresses)
                                {
                                    if (email == val)
                                        return account;
                                }
                            }
                        }

                        break;
                    case UpdateKeyTypes.Name:
                        //var contacts = from contact in OCMAccounts where contact.CompanyName.ToString() == val select contact;
                        var mycontact = OCMAccounts.FirstOrDefault(contacts => contacts.DisplayName == val);
                        return mycontact;
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return null;
        }
        private async Task<OCMCompanyTemplate.Rootobject> GetTemplateAsync()
        {
            try
            {
                PrepareRequest(RequestDataTypes.Templates, RequestDataFormats.None);
                // Get response
                using (
                    var response =
                        await _httpClient.GetAsync(
                            "https://outlook.office.com/api/beta/me/XrmOrganizationTemplate('CompanyTemplate')"))
                {
                    // Check status code.
                    //response.EnsureSuccessStatusCode(); //throw a message in case of an error
                    var content = await response.Content.ReadAsStringAsync();

                    Log.InfoFormat("Company Template: {0}", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        Log.ErrorFormat("Error'{0}': {1}", response.ReasonPhrase, content);
                        OnError(null,
                            new HelperEventArgs(String.Format("ERROR '{0}': {1}", response.ReasonPhrase, content),
                                HelperEventArgs.EventTypes.Error, HelperEventArgs.ProcessingModes.Mapping));
                        NumberOfErrors += 1;
                        return null;
                    }

                    // Read and deserialize response.                
                    var content2 = await response.Content.ReadAsStreamAsync();
                    var jsonSerializer = new DataContractJsonSerializer(typeof(OCMCompanyTemplate.Rootobject));
                    OCMCompanyTemplate.Rootobject jsonResponse = jsonSerializer.ReadObject(content2) as OCMCompanyTemplate.Rootobject;     
                    Log.InfoFormat("Companies template retrieved with {0} custom fields: {1}", jsonResponse.Template.FieldList?.Length, content);       
                    //Version: 8        
                    return jsonResponse;
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                OnError(null, new HelperEventArgs(String.Format("ERROR in GetTemplateAsync: {0}", ex.Message), HelperEventArgs.EventTypes.Error, HelperEventArgs.ProcessingModes.Mapping));
                NumberOfErrors += 1;
                return null;
            }
        }
        private async Task ImportCompanyAsync(AccountsFullView company, ImportModes importMode, OCMCompany2Value existingCompany = null)
        {
            using (Log.VerboseCall())
            {
                OCMCompany.Rootobject ocmCompany = new OCMCompany.Rootobject(); //Was type OCMContact2Value
                int defaultProps = 0;
                SingleValueExtendedProperty[] customProps = null;

                try
                {               
                    //Manual mappings
                    //========================================================================================================================

                    //Create collection of custom properties, including one for BCMID and TWO more for sharing fields if sharing is enabled
                    if (importMode == ImportModes.UpdatePreviouslyImportedItem)
                        goto setProperties; //No need to create sharing or BCMID props on previously imported item

                    defaultProps = MigrationHelper.IsSharingEnabled ? 3 : 1;

                    ocmCompany.SingleValueExtendedProperties = new SingleValueExtendedProperty[defaultProps];
                    //Add BCMID prop
                    SingleValueExtendedProperty bcmIDProp = new SingleValueExtendedProperty();
                    bcmIDProp.PropertyId = String.Format("String {0} Name BCMID", FieldMappings.BCMIDPropertyGUID);
                    bcmIDProp.Value = company.EntryGUID.ToString();
                    ocmCompany.SingleValueExtendedProperties[0] = bcmIDProp;                    

                    if (MigrationHelper.IsSharingEnabled)
                    {
                        //TESTED · When sharing any item, the following properties must be set: 
                        //Set “XrmSharingSourceUserDisplayName” with the display name of the current user
                        //e.g. <ExtendedFieldURI PropertySetId="1a417774-4779-47c1-9851-e42057495fca" PropertyName="XrmSharingSourceUserDisplayName" PropertyType="String" xmlns="http://schemas.microsoft.com/exchange/services/2006/messages" />                
                        //Set “XrmSharingSourceUser” with the Object - Id(AAD ID) of the current user                
                        //e.g. <ExtendedFieldURI PropertySetId="1a417774-4779-47c1-9851-e42057495fca" PropertyName="XrmSharingSourceUser" PropertyType="String" xmlns="http://schemas.microsoft.com/exchange/services/2006/messages" />

                        SingleValueExtendedProperty addProp = null;
                        addProp = new SingleValueExtendedProperty();
                        addProp.PropertyId = "String 1a417774-4779-47c1-9851-e42057495fca Name XrmSharingSourceUserDisplayName";
                        addProp.Value = MigrationHelper.CurrentUser.DisplayName;
                        ocmCompany.SingleValueExtendedProperties[1] = addProp;

                        addProp = new SingleValueExtendedProperty();
                        addProp.PropertyId = "String 1a417774-4779-47c1-9851-e42057495fca Name XrmSharingSourceUser";
                        addProp.Value = MigrationHelper.CurrentUserID;
                        ocmCompany.SingleValueExtendedProperties[2] = addProp;
                    }
                    //=========================================================================================

                    setProperties:

                    if (FieldMappings.BCMAccountFields.BCMFields == null) //Why would this be null?
                        goto populateContactDetails;
                    if (FieldMappings.BCMAccountFields.BCMFields.MappedFields.Count == 0) //Just the BCMID, no custom fields
                        goto populateContactDetails;

                    //Import custom fields
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        //HIGH Filter SQL query by Contact GUID instead of looping
                        using (SqlCommand com = new SqlCommand(Resources.BCM_CustomFields_Accounts, con))
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
                                        
                                            if (!String.IsNullOrEmpty(contactID) && contactID != company.EntryGUID.ToString())
                                                continue;

                                            //int mappedFieldCnt = defaultProps; //TESTED Seed with number of additional manual props for sharing and BCMID
                                            foreach (var bcmField in FieldMappings.BCMAccountFields.BCMFields.MappedFields)
                                            {
                                                if (bcmField.OCMFieldMapping == null)
                                                {
                                                    Log.ErrorFormat("Unknown error for Account'{0}'", company.FullName);
                                                    OnError(null, new HelperEventArgs(String.Format("Unknown error (D) in ImportCompanyAsync for Account '{0}'", company.FullName), HelperEventArgs.EventTypes.Error));
                                                    continue;
                                                }
                                                for (int i = 0; i < reader.FieldCount; i++)
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
                                                            SingleValueExtendedProperty prop = new SingleValueExtendedProperty();
                                                            prop.PropertyId = String.Format("{0} {1} Name {2}", bcmField.OCMFieldMapping.DataTypeLabelForJSON, "{1a417774-4779-47c1-9851-e42057495fca}", bcmField.OCMFieldMapping.PropertyID);

                                                            //prop.Value = valStr;
                                                            if (bcmField.OCMFieldMapping.FieldType == FieldMappings.OCMField.OCMFieldTypes.Date)
                                                            {
                                                                //TESTED Convert to proper date format
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
                                                        break;
                                                    }
                                                }
                                                //mappedFieldCnt += 1;
                                            }
                                            break;//TESTED Leaving while; we found the Company and are done
                                        }
                                    }                                
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex);
                                    OnError(null, new HelperEventArgs(String.Format("Unknown error (A) in ImportCompanyAsync for Account '{0}'", company.FullName), HelperEventArgs.EventTypes.Error, ex));
                                }
                            }
                        }
                    }

                    try
                    {
                        //TESTED Now loop through MappedFields and find non-custom BCM fields; set values of these fields
                        foreach (var bcmField in FieldMappings.BCMAccountFields.BCMFields.MappedFields)
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
                                PropertyInfo prop = company.GetType().GetProperty(bcmField.Name, BindingFlags.Public | BindingFlags.Instance);

                                string val = "";

                                try
                                {
                                    val = prop.GetValue(company).ToString();
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
                                        //TESTED Convert to proper date format
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
                            SingleValueExtendedProperty[] curProps = ocmCompany.SingleValueExtendedProperties;
                            int idxStart = curProps.Length;
                            Array.Resize(ref curProps, customProps.Length + curProps.Length);
                            customProps.CopyTo(curProps, idxStart);
                            ocmCompany.SingleValueExtendedProperties = curProps;
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

                    if (existingCompany != null)
                    {
                        //TESTED If updating existing company, use existingCompany var or copy to ocmCompany
                        try
                        {
                            ocmCompany.DisplayName = existingCompany.DisplayName;
                            ocmCompany.BusinessAddress = existingCompany.BusinessAddress;
                            ocmCompany.BusinessHomePage = existingCompany.BusinessHomePage;
                            ocmCompany.BusinessPhones = existingCompany.BusinessPhones;
                            ocmCompany.CustomerBit = existingCompany.CustomerBit;
                            ocmCompany.CustomerStatus = existingCompany.CustomerStatus;
                            ocmCompany.EmailAddresses = existingCompany.EmailAddresses;
                            ocmCompany.Manager = existingCompany.Manager;
                            ocmCompany.Notes = existingCompany.Notes;
                            ocmCompany.Profession = existingCompany.Profession;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Update error for Account '{0}': {1}", company.FullName, ex);
                        }
                    }                

                    //Now, skip any null values (do not update existing non-empty value with an empty/null value from BCM)

                    if (!String.IsNullOrEmpty(company.FullName))
                        ocmCompany.DisplayName = company.FullName; //NOTE: CompanyName could be blank; use FullName for DisplayName        

                    //1.0.15 FullName can also be blank! Look to CompanyName (usually blank?) and if still empty, use FileAs   

                    if (String.IsNullOrEmpty(ocmCompany.DisplayName) && !String.IsNullOrEmpty(company.CompanyName))
                        ocmCompany.DisplayName = company.CompanyName;
                    if (String.IsNullOrEmpty(ocmCompany.DisplayName) && !String.IsNullOrEmpty(company.FileAs))
                        ocmCompany.DisplayName = company.FileAs;
                    if (String.IsNullOrEmpty(ocmCompany.DisplayName) && !String.IsNullOrEmpty(company.Email1Address))
                        ocmCompany.DisplayName = company.Email1Address;
                    if (String.IsNullOrEmpty(ocmCompany.DisplayName))
                        ocmCompany.DisplayName = "UNKNOWN COMPANY";

                    //Address data
                    if (!String.IsNullOrEmpty(company.WorkAddressStreet))
                    {
                        Businessaddress businessAddress = new Businessaddress(); //Set whether it is null or not?

                        businessAddress.Street = company.WorkAddressStreet;
                        businessAddress.City = company.WorkAddressCity;
                        businessAddress.State = company.WorkAddressState;
                        businessAddress.CountryOrRegion = company.WorkAddressCountry;
                        businessAddress.PostalCode = company.WorkAddressZip;

                        ocmCompany.BusinessAddress = businessAddress; //Set whether it is null or not?
                    }

                    //Changeed: From [BCM] CompanyMainPhoneNum to [OCM] Business Phone, to[BCM] WorkPhoneNum to[OCM] Business Phone                    
                    if (!String.IsNullOrEmpty(company.WorkPhoneNum))
                    {
                        ocmCompany.BusinessPhones = new object[1];
                        ocmCompany.BusinessPhones[0] = company.WorkPhoneNum;
                    }

                    if (!String.IsNullOrEmpty(company.Email1Address))
                    {
                        //Email
                        OCMCompany.Emailaddress emailAddress = new OCMCompany.Emailaddress();

                        emailAddress.Name = company.Email1DisplayAs;
                        emailAddress.Address = company.Email1Address;
                        ocmCompany.EmailAddresses = new object[1];
                        ocmCompany.EmailAddresses[0] = emailAddress;
                    }

                    if (!String.IsNullOrEmpty(company.WebAddress))
                        ocmCompany.BusinessHomePage = company.WebAddress;
                    //=====================================================================================

                    if (!String.IsNullOrEmpty(company.ContactNotes))
                        ocmCompany.Notes = company.ContactNotes; //NOTE: Notes is special: auto-mapped but optional
                
                }
                catch (Exception ex)
                {
                    //Log error; skip record?
                    Log.Error(ex);
                    OnError(null, new HelperEventArgs(String.Format("Unknown error (B) in ImportCompanyAsync for Account '{0}'", company.FullName), HelperEventArgs.EventTypes.Error, ex));
                }

                var json = JsonConvert.SerializeObject(ocmCompany);
                OCMCompany2Value newCompany = null;

                try
                {
                    Uri uri = null;

                    //Multiple props:
                    //?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUserDisplayName'))

                    if (importMode == ImportModes.Create)
                    {
                        uri = new Uri(String.Format("{0}/XrmOrganizations?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUserDisplayName'))", Settings.Default.BetaEndPoint));
                        //e.g. https://outlook.office.com/api/beta/Me/XrmOrganizations?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+{1a417774-4779-47c1-9851-e42057495fca}+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+{1a417774-4779-47c1-9851-e42057495fca}+Name+XrmSharingSourceUserDisplayName'))

                        //Debug.WriteLine(String.Format("Posting to {1}:{2}{0}{2}Token:{2}{3}", json, uri, Environment.NewLine, AccessToken));
                        Log.VerboseFormat("Posting to {1}:{0}", json, uri);
                        PrepareRequest(RequestDataTypes.Companies, RequestDataFormats.JSON);      
                                  
                        using (var response = await _httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json")))
                        {
                            var content = await response.Content.ReadAsStringAsync();

                            if (!response.IsSuccessStatusCode) // Check status code.
                            {
                                var responseContent = response.Content.ReadAsStringAsync();
                                Log.ErrorFormat("ERROR creating '{1}': {0}", responseContent, company.FullName);
                                OnError(null, new HelperEventArgs(String.Format("ERROR creating '{1}': {0}", responseContent, company.FullName), HelperEventArgs.EventTypes.Error));
                                NumberOfErrors += 1;
                            }
                            else
                            {                            
                                Log.DebugFormat("Created Company '{0}' (BCMID: {1})'", company.FullName, company.EntryGUID.ToString());
                                OnCreateItemComplete(null, new HelperEventArgs(String.Format("Created Company '{0}'...", company.FullName), false));
                                NumberCreated += 1;
                            
                                try
                                {                           
                                    //TESTED Get response with Contact details so we can get the ID for use in moving operation to share it; move with contacts/{ID}/move call
                                    newCompany = JsonConvert.DeserializeObject<OCMCompany2Value>(content);
                                    newCompany.BCMID = company.EntryGUID.ToString();
                                    OCMAccounts.Add(newCompany);

                                    //NOTE To make a Contact or Company shared: make a REST call to ‘/move’ with the folder-id of the sharing child-folder, not the modern-group mailbox guid.
                                    //REVIEW If doing a PATCH, do we need to check if it has already been shared so as not to 'move' it again? try it on a shared Company and see if it throws an error

                                    PrepareRequest(RequestDataTypes.Companies, RequestDataFormats.XML);
                                    uri = new Uri(Settings.Default.EWSEndPoint);

                                    string id = newCompany.Id.Replace("_", "+");
                                    id = id.Replace("-", "/"); //HIGH HACK to make the MoveItem operation work (ErrorInvalidIdMalformed)
                                    string xmlRequest = String.Format(Resources.ShareContactsRequest, MigrationHelper.CompaniesFolderID,
                                        id);

                                    using (var moveResponse = await _httpClient.PostAsync(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml")))
                                    {
                                        if (!moveResponse.IsSuccessStatusCode) // Check status code.
                                        {
                                            var moveResponseContent = await moveResponse.Content.ReadAsStringAsync();
                                            Log.ErrorFormat("ERROR sharing '{1}': {0}", moveResponseContent, company.FullName);
                                            OnError(null,
                                                new HelperEventArgs(
                                                    String.Format("ERROR sharing '{1}': {0}", moveResponseContent, company.FullName),
                                                    HelperEventArgs.EventTypes.Error));
                                            NumberOfErrors += 1;
                                        }
                                        else
                                        {
                                            //Log message?
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex);
                                }                            
                            }
                        }
                    }
                    else
                    {                                        
                        try
                        {
                            uri = new Uri(String.Format("{0}/XrmOrganizations('{1}')?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+%7B1a417774-4779-47c1-9851-e42057495fca%7D+Name+XrmSharingSourceUserDisplayName'))", Settings.Default.BetaEndPoint, existingCompany.Id));
                            //e.g. https://outlook.office.com/api/beta/Me/XrmOrganizations?%24Expand=SingleValueExtendedProperties(%24filter%3d(PropertyId+eq+'String+{1a417774-4779-47c1-9851-e42057495fca}+Name+XrmSharingSourceUser')+OR+(PropertyId+eq+'String+{1a417774-4779-47c1-9851-e42057495fca}+Name+XrmSharingSourceUserDisplayName'))

                            PrepareRequest(RequestDataTypes.Companies, RequestDataFormats.JSON);
                            HttpMethod method = new HttpMethod("PATCH");
                            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpRequestMessage request = new HttpRequestMessage(method, uri) { Content = httpContent };
                            using (var response = await _httpClient.SendAsync(request))
                            {
                            
                                //Debug.WriteLine(String.Format("Patching to {1}:{2}{0}{2}Token:{2}{3}", json, uri, Environment.NewLine, AccessToken));
                                //Debug.WriteLine(response);
                                var content = await response.Content.ReadAsStringAsync();
                                if (!response.IsSuccessStatusCode) // Check status code.
                                {
                                    //BUGFIXED ERROR updating 'Major Sport Suppliers': {"error":{"code":"ErrorInvalidRequest","message":"Method 'PATCH/MERGE' not allowed for 'Microsoft.OData.UriParser.NavigationPropertySegment'"}}
                                    Log.ErrorFormat("ERROR updating '{1}': {0}", content, company.FullName);
                                    OnError(null, new HelperEventArgs(String.Format("ERROR updating '{1}': {0}", content, company.FullName), HelperEventArgs.EventTypes.Error));
                                    NumberOfErrors += 1;
                                }
                                else
                                {
                                    OnPatchComplete(null, new HelperEventArgs(String.Format("Updated Company '{0}'...", company.FullName), false));
                                    NumberUpdated += 1;
                                    //REVIEW Do we need to set the BCMID on an existing company?
                                    existingCompany.BCMID = company.EntryGUID.ToString();

                                    //Do we still need to add a patched company to internal collection for linking later? NO - it should have already been retrieved in GetOCMCompanies!
                                    //newCompany = JsonConvert.DeserializeObject<OCMCompany2Value>(content);
                                    //OCMAccounts.Add(newCompany);
                                }
                            }                        
                        }
                        catch (TaskCanceledException e)
                        {
                            Debug.WriteLine("ERROR: " + e.ToString());
                        }
                    }                
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    OnError(null, new HelperEventArgs(String.Format("Unknown error (C) in ImportCompanyAsync for Account '{0}'", company.FullName), HelperEventArgs.EventTypes.Error, ex));
                    //Debug.WriteLine(ex.ToString());
                }
            }
        }      
        private async Task RunCreateCompanyAsync()
        {
            using (Log.VerboseCall())
            {
                using (var db = new MSSampleBusinessEntities())
                {
                    try
                    {
                        db.Database.Connection.ConnectionString = ConnectionString;
                        var accountCount = await db.AccountsFullViews.Where(a => (!a.IsDeletedLocally) && (a.AccountActive)).CountAsync();
                        var allAccounts = from account in db.AccountsFullViews where !account.IsDeletedLocally && account.AccountActive select account;

                        if (TestMode)
                        {
                            NumberToProcess = TestingMaximum;
                            OnStart(null, new HelperEventArgs(String.Format("Importing {0} Accounts of {1} (TESTING MODE)", accountCount, TestingMaximum), false));
                        }
                        else
                        {
                            NumberToProcess = accountCount;
                            OnStart(null, new HelperEventArgs(String.Format("Importing {0} Accounts...", accountCount), false));
                        }
                    
                        int cnt = 0;
                        foreach (var account in allAccounts)
                        {
                            if (TestMode && cnt == TestingMaximum && TestingMaximum > 0)
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
                            OCMCompany2Value ocmCompany= null;
                            string bcmID = "";
                            string companyNameToMatch = account.FullName;

                            ocmCompany = GetOCMCompany(account.EntryGUID.ToString(), UpdateKeyTypes.BCMID);

                            if (ocmCompany == null)
                            {
                                //Look for matching Company on name match
                                //1.0.15 Attempt to match on additional fields as FullName could be blank
                                if (String.IsNullOrEmpty(companyNameToMatch) && !String.IsNullOrEmpty(account.CompanyName))
                                    companyNameToMatch = account.CompanyName;
                                if (String.IsNullOrEmpty(companyNameToMatch) && !String.IsNullOrEmpty(account.FileAs))
                                    companyNameToMatch = account.FileAs;

                                ocmCompany = GetOCMCompany(companyNameToMatch, UpdateKeyTypes.Name);
                            }

                            if (ocmCompany != null)
                            {
                                //TESTED Must distinguish between existing manually created Companies (no BCMID) and those previously imported (with BCMID)

                                bcmID = ocmCompany.GetBCMID();
                                if (!String.IsNullOrEmpty(bcmID))
                                {
                                    //Previously imported - IGNORE
                                    Log.DebugFormat("Company '{0}' previously imported; skipping", ocmCompany.DisplayName);
                                    OnCreateItemComplete(null, new HelperEventArgs(String.Format("Company '{0}' previously imported; skipping", ocmCompany.DisplayName), false));
                                    continue;
                                }
                                else
                                {
                                    //Update existing manually created item
                                    Log.DebugFormat("Updating Company '{0}'", companyNameToMatch);
                                    importMode = ImportModes.Update;
                                }
                            }
                            else
                            {
                                //1.0.15 If a match still wasn't found, create the Company using the Email1Address field - or call it UNKNOWN COMPANY - but throw an error anyway
                                if (String.IsNullOrEmpty(companyNameToMatch) && !String.IsNullOrEmpty(account.Email1Address))
                                {
                                    companyNameToMatch = account.Email1Address;
                                    OnError(null, new HelperEventArgs(String.Format("Could not find a company name for Account with EntryGUID {0} (ContactServiceID: {1}). Using email '{2}' as the company name.", account.EntryGUID, account.ContactServiceID, account.Email1Address), HelperEventArgs.EventTypes.Error));
                                }
                                if (String.IsNullOrEmpty(companyNameToMatch))
                                {
                                    //companyNameToMatch = "UNKNOWN COMPANY"; Create using this name in ImportCompanyAsync
                                    OnError(null, new HelperEventArgs(String.Format("Could not find a company name for Account with EntryGUID {0} (ContactServiceID: {1}). Importing company as 'UNKNOWN COMPANY'.", account.EntryGUID, account.ContactServiceID), HelperEventArgs.EventTypes.Error));
                                }

                                importMode = ImportModes.Create;
                                cnt += 1;
                            }

                            await ImportCompanyAsync(account, importMode, ocmCompany);
                            cnt += 1;
                        }

                        if (!Cancelled)
                        {
                            OnHelperComplete(null, new HelperEventArgs(String.Format("Accounts import complete!{0}Created: {1}{0}Errors: {2}", Environment.NewLine, NumberCreated, NumberOfErrors), false));
                        }
                        else
                        {
                            OnHelperComplete(null, new HelperEventArgs(String.Format("--Accounts import stopped--{0}Created: {1}{0}Errors: {2}", Environment.NewLine, NumberCreated, NumberOfErrors), false));
                        }                    

                    }
                    catch (Exception ex)
                    {
                        OnError(this, new HelperEventArgs(ex.ToString(), HelperEventArgs.EventTypes.FatalError));
                    }
                }
            }
        }
        private async Task RunGetOCMCompaniesAsync()
        {
            using (Log.VerboseCall())
            {
                do
                {
                    Debug.WriteLine("RunGetOCMCompaniesAsync; Calling GetOCMCompaniesAsync()");
                    await GetOCMCompaniesAsync();
                    Debug.WriteLine("GetOCMCompaniesAsync() returned");
                } while (PageSkip > 0);
                Debug.WriteLine("Leaving RunGetOCMCompaniesAsync()");
            }
        }
        public async Task RunGetTemplateAsync()
        {
            using (Log.VerboseCall())
            {
                OnStart(null, new HelperEventArgs("Retrieving Companies template...", HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
                CompanyTemplate = await GetTemplateAsync();
                if (CompanyTemplate != null)
                {
                    OnGetComplete(null, new HelperEventArgs(String.Format("Companies template retrieved with {0} custom fields", CompanyTemplate.Template.FieldList?.Length), HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
                    FieldMappings.OCMAccountFields.CompanyTemplate = CompanyTemplate;
                }
            }
        }

#endregion
    }
}
