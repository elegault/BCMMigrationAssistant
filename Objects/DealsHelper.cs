using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using BcmMigrationTool;
using BCM_Migration_Tool.Properties;
using Newtonsoft.Json;
using TracerX;

namespace BCM_Migration_Tool.Objects
{
    class DealsHelper: HelperBase
    {
        private static readonly Logger Log = Logger.GetLogger("DealsHelper");

        #region Constructors
        internal DealsHelper(string accessToken, string connectionString) : base(accessToken, connectionString)
        {
        }
        #endregion
        #region Fields
        internal enum DeleteDealsOptions
        {
            All,
            Private,
            Shared                       
        }
        #endregion

        #region Events

        internal event EventHandler<EventArgs> GetTemplateComplete;
        internal event EventHandler<EventArgs> UpdateTemplateComplete;

        protected virtual void OnGetTemplateComplete(object sender, EventArgs e)
        {            
            EventHandler<EventArgs> handler = GetTemplateComplete;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void OnUpdateTemplateComplete(object sender, EventArgs e)
        {
            EventHandler<EventArgs> handler = UpdateTemplateComplete;
            if (handler != null)
                handler(sender, e);
        }
        #endregion

        //MSSampleBusinessEntities db = new MSSampleBusinessEntities();
        //internal List<PicklistsMasterList> DealStages { get; set; }
        #region Properties

        public List<string> BCMOpportunities { get; set; }
        private List<string> _dealStages;
        internal List<string> DealStages
        {
            get
            {
                if (_dealStages == null) _dealStages = GetDealStages();
                return _dealStages;
            }
            set
            {
                _dealStages = value;
            }
        }
        internal OCMDealTemplate.Rootobject DealTemplate { get; set; }
        internal static List<Deal> OCMDeals { get; set; }
        internal static List<Deal> OCMDealsCreated { get; set; }
        private int PageSkip { get; set; }

        #endregion

        #region Methods
        private async Task<bool> CreateDealLink(string itemLinkID, string xrmID, FieldMappings.OCMDataSetTypes linkType)
        {
            string xmlRequest = Resources.CreateXrmGraphRelationshipRequest;

            Uri uri = new Uri(Settings.Default.EWSEndPoint);

            xmlRequest = xmlRequest.Replace("{FROMENTITYID}", xrmID);
            xmlRequest = xmlRequest.Replace("{FROMENTITYTYPE}", "XrmDeal");
            xmlRequest = xmlRequest.Replace("{TOENTITYID}", itemLinkID);
            xmlRequest = xmlRequest.Replace("{TOENTITYTYPE}", linkType == FieldMappings.OCMDataSetTypes.Accounts ? "XrmOrganization" : "Person");
            xmlRequest = xmlRequest.Replace("{LINKTYPE}", linkType == FieldMappings.OCMDataSetTypes.Accounts ? "CustomerOf" : "PointsOfContactFor");

            try
            {
                PrepareRequest(RequestDataTypes.Links, RequestDataFormats.XML);
                using (var response = await _httpClient.PostAsync(uri, new StringContent(xmlRequest, Encoding.UTF8, "text/xml")))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        OnError(null,
                            new HelperEventArgs(String.Format("ERROR: {0}", content), HelperEventArgs.EventTypes.Error));
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
        internal async Task CreateDealLinks()
        {
            using (Log.VerboseCall())
            {
                NumberToProcess = OCMDeals.Count;
                OnStart(null, new HelperEventArgs("Linking deals...", HelperEventArgs.EventTypes.Status));
                foreach (var deal in OCMDeals)
                {
                    try
                    {
                        int linkType = 0;
                        string entityID = "";
                        string entityName = "";

                        if (String.IsNullOrEmpty(deal.BCMID))
                            continue; //Skip non-imported deals

                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            string sql = Resources.BCM_Opportunity_Core;
                            sql = sql.Replace("{0}", deal.BCMID);
                            using (SqlCommand com = new SqlCommand(sql, con))
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
                                                linkType = Convert.ToInt32(reader["Type"]);
                                                //entityID = Convert.ToString(reader["EntryGUID"]);
                                                entityID = Convert.ToString(reader["ParentEntryID"]);
                                                //contactServiceID = Convert.ToInt32(reader["ContactServiceID"]);
                                                entityName = Convert.ToString(reader["FullName"]);
                                                break;
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

                        //BUGFIXED Wasn't create links because InlineLinks is always null until AFTER we create links
                        //if (deal.InlineLinks != null)
                        //HIGHz do we need to look for an existing link? I don't think so...
                        //OCMCompany2Value company = AccountsHelper.GetOCMCompany(deal.Name?.ToString(),
                        //    UpdateKeyTypes.Name);
                        //if (company == null)
                        //{
                        //    //??
                        //    continue;
                        //}
                        //bool existingLink = false;

                        //if (company.InlineLinks?.Relationships != null)
                        //    foreach (var link in company.InlineLinks?.Relationships)
                        //    {
                        //        //TESTz Match on ID BUGz Wrong match? do on link.DisplayName to contact.DisplayName
                        //        if (link.ItemLinkId == deal.ItemLinkId)
                        //        {
                        //            existingLink = true;
                        //        }
                        //        if (link.DisplayName == deal.DisplayName)
                        //        {
                        //            existingLink = true;
                        //        }
                        //    }
                        //if (existingLink || String.IsNullOrEmpty(company.XrmId))
                        //    continue;
                        OCMContact2Value contact = null;
                        OCMCompany2Value company = null;
                        string itemLinkID = "";
                        switch (linkType)
                        {
                            case 1:
                                //Contact
                                try
                                {
                                    contact = ContactsHelper.OCMContacts.FirstOrDefault(contacts => contacts.BCMID == entityID);
                                    if (contact == null)
                                        contact = ContactsHelper.OCMContacts.FirstOrDefault(contacts => contacts.DisplayName == entityName);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex);   
                                }

                                if (contact != null)
                                {
                                    itemLinkID = contact.ItemLinkId;
                                }
                                break;
                            case 2:
                                //Company
                                try
                                {
                                    company = AccountsHelper.GetOCMCompany(entityID, UpdateKeyTypes.BCMID);
                                    if (company != null)
                                    {
                                        itemLinkID = company.XrmId;
                                    }
                                    else
                                    {
                                        company = AccountsHelper.GetOCMCompany(entityName, UpdateKeyTypes.Name);
                                        if (company != null)
                                        {
                                            itemLinkID = company.XrmId;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(ex);
                                }
                                break;
                        }

                        if (String.IsNullOrEmpty(itemLinkID))
                        {
                            Log.ErrorFormat("Could not retrieve OCM entity '{0}' for linking (Type: {1}; ID: {2})", entityName, linkType, entityID);
                            OnError(null, new HelperEventArgs(String.Format("ERROR: Could not retrieve OCM entity '{0}' for linking (Type: {1})", entityName, linkType), HelperEventArgs.EventTypes.Error));
                            continue;
                        }
                        Relationship link = null;
                        try
                        {
                            //TESTED Look for existing link.
                            if (deal.InlineLinks != null)
                            {
                                if (deal.InlineLinks.Relationships != null)
                                {
                                    link = deal.InlineLinks.Relationships.FirstOrDefault(links => links.ItemLinkId == itemLinkID);                            
                                }
                            }                        
                        }
                        catch (Exception)
                        { }

                        if (link != null)
                        {
                            //Existing link found - skip
                            OnDisplayMessage(null, new HelperEventArgs(String.Format("Deal '{0}' already linked to entity '{1}'; skipping", deal.Name, entityName), HelperEventArgs.EventTypes.Status));
                            OnIncrementProgress(null, new HelperEventArgs("", HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Migrating));
                            continue;
                        }

                        bool linked = await CreateDealLink(itemLinkID, deal.XrmId, linkType == 1 ? FieldMappings.OCMDataSetTypes.BusinessContacts : FieldMappings.OCMDataSetTypes.Accounts);
                        if (linked)
                        {
                            OnDisplayMessage(null,
                                new HelperEventArgs(
                                    String.Format("Deal '{0}' linked to entity '{1}'", deal.Name, entityName), HelperEventArgs.EventTypes.Status));
                            OnIncrementProgress(null, new HelperEventArgs("", HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Migrating));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        OnError(null, new HelperEventArgs(String.Format("Unknown error in CreateDealLinks: {0})", ex.ToString()), HelperEventArgs.EventTypes.Error));                    
                    }

                }
                OnHelperComplete(null, new HelperEventArgs(String.Format("{0} deals processed for linking", OCMDeals.Count), HelperEventArgs.EventTypes.Status));

            }
        }
        internal async Task CreateDeals()
        {
            Cancelled = false;
            await RunCreateDealsAsync();
        }

        private async Task<bool> DeleteDeal(string ID)
        {
            bool result = false;

            try
            {
                Uri uri;
                uri = new Uri(String.Format("{0}/XrmDeals('{1}')", Properties.Settings.Default.BetaEndPoint, ID));
                PrepareRequest(RequestDataTypes.Deals, RequestDataFormats.None);
                using (var response = await _httpClient.DeleteAsync(uri))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        result = true;
                    }
                    else
                    {
                        Log.Warn("Error deleting deal ID '{0}': {1}", ID, content);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return result;
        }
        //internal async Task DeleteDeals(string ids)
        //{
        //    try
        //    {
        //        char delim = ';';
        //        string[] idVals = ids.Split(delim);
        //        foreach (var id in idVals)
        //        {
        //            DeleteDeal(id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //    }
        //}
        internal async Task<int> DeleteDeals(DeleteDealsOptions options, DateTime dealCreatedOn)
        {
            int cnt = 0;

            try
            {
                OCMDeals = null;
                await GetOCMDealsAsync();
                //if (OCMDeals == null)
                //{
                //    await GetOCMDealsAsync();
                //}

                List<Deal> dealsToDelete = null;
//#if DEBUG
//                int dayOfYear = 1;
//                dealsToDelete = OCMDeals.Where(deals => deals.CreationTime.DayOfYear == 0) as List<Deal>;
//#else
//                dealsToDelete = OCMDeals;
//#endif
                if (dealCreatedOn != DateTime.MinValue)
                {
                    dealsToDelete = new List<Deal>();
                    //Why does this return null??
                    //dealsToDelete = OCMDeals.Where(deals => deals.CreationTime.DayOfYear == dealCreatedOn.DayOfYear) as List<Deal>;
                    foreach (var deal in OCMDeals)
                    {
                        if (deal.CreationTime.DayOfYear == dealCreatedOn.DayOfYear)
                            dealsToDelete.Add(deal);
                    }
                }
                else
                {
                    dealsToDelete = OCMDeals;
                }

                for (int i = dealsToDelete.Count -1; i >= 0; i--)
                {
                    Deal deal = dealsToDelete[i];
                    Debug.Print("Deleting deal '{0}'", deal.Name);
                    switch (options)
                    {
                        case DeleteDealsOptions.All:
                            
                            if (await DeleteDeal(deal.Id))
                            {
                                cnt += 1;
                            }
                            break;
                        case DeleteDealsOptions.Private:
                            Guid sourceMailboxGuid = new Guid(deal.SourceMailboxGuid);
                            if (sourceMailboxGuid == Guid.Empty)
                            {
                                if (await DeleteDeal(deal.Id))
                                {
                                    cnt += 1;
                                }
                            }
                            break;
                        case DeleteDealsOptions.Shared:
                            Guid sourceMailboxGuid2 = new Guid(deal.SourceMailboxGuid);
                            if (sourceMailboxGuid2 != Guid.Empty)
                            {
                                if (await DeleteDeal(deal.Id))
                                {
                                    cnt += 1;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            OCMDeals = null;
            return cnt;
        }
        private async Task ImportDealAsync(OpportunityFullView opportunity, ImportModes importMode, Deal existingDeal = null)
        {
            OCMDeal2.Rootobject ocmDeal = new OCMDeal2.Rootobject();
            //bool sharingEnabled = false; 
            SingleValueExtendedProperty[] customProps = null;

            //REVIEW Remove sharing override after testing?
            //sharingEnabled = false; //MigrationHelper.IsSharingEnabled;

            ocmDeal.SingleValueExtendedProperties = null;//HACK So it can get created or else object reference not set error on server

            if (importMode == ImportModes.UpdatePreviouslyImportedItem)
                goto setProperties; //No need to create sharing or BCMID props on previously imported item

            //HIGH Setting extended props according to MS sample fails - invalid custom property iD. Now this is attempted in the PatchDeal method after the deal is created, but it still isn't working - and not being called for now
            //These were all of the attempts to get the BCM ID prop to stick before a PATCH call:
            //addProp = new SingleValueExtendedProperty();
            ////PropertyId=String {1a417774-4779-47c1-9851-e42057495fca} Name ae8b5199-c02d-4008-aa8a-decabbf04c14
            //addProp.PropertyId = "String {1a417774-4779-47c1-9851-e42057495fca} Name ae8b5199-c02d-4008-aa8a-decabbf04c14";
            //addProp.Value = null;
            //ocmDeal.SingleValueExtendedProperties[0] = addProp;
            //addProp = new SingleValueExtendedProperty();
            ////PropertyId=Double {1a417774-4779-47c1-9851-e42057495fca} Name f70a39a4-6bed-4aeb-80dd-afcad36e6991
            //addProp.PropertyId = "String {1a417774-4779-47c1-9851-e42057495fca} Name f70a39a4-6bed-4aeb-80dd-afcad36e6991";
            //addProp.Value = "0";
            //ocmDeal.SingleValueExtendedProperties[1] = addProp;
            //addProp = new SingleValueExtendedProperty();
            ////PropertyId=String {1a417774-4779-47c1-9851-e42057495fca} Name e26bbdc5-3895-44f6-a076-16407fada65e
            //addProp.PropertyId = "String {1a417774-4779-47c1-9851-e42057495fca} Name e26bbdc5-3895-44f6-a076-16407fada65e";
            //addProp.Value = null;
            //ocmDeal.SingleValueExtendedProperties[2] = addProp;
            //addProp = new SingleValueExtendedProperty();
            ////PropertyId=SystemTime {1a417774-4779-47c1-9851-e42057495fca} Name 3403026b-c077-450f-b18d-cd266167f604
            //addProp.PropertyId = "String {1a417774-4779-47c1-9851-e42057495fca} Name 3403026b-c077-450f-b18d-cd266167f604";
            ////Value=0001-01-01T00:00:00Z
            //addProp.Value = "0001-01-01T00:00:00Z";
            //ocmDeal.SingleValueExtendedProperties[3] = addProp;
            //Bug Setting BCMID prop: {"error":{"code":"ErrorInternalServerError","message":"Invalid custom property id"}}
            //SingleValueExtendedProperty bcmIDProp = new SingleValueExtendedProperty();
            //bcmIDProp.PropertyId = String.Format("String {0} Name BCMID", FieldMappings.BCMIDPropertyGUID);
            //bcmIDProp.Value = opportunity.EntryGUID.ToString();
            //ocmDeal.SingleValueExtendedProperties[0] = bcmIDProp;

            //NOTE: To share, just set SourceMailboxGuid prop!!!
            //if (sharingEnabled)
            //{
            //    //TESTED · When sharing any item, the following properties must be set: 
            //    //Set “XrmSharingSourceUserDisplayName” with the display name of the current user
            //    //e.g. <ExtendedFieldURI PropertySetId="1a417774-4779-47c1-9851-e42057495fca" PropertyName="XrmSharingSourceUserDisplayName" PropertyType="String" xmlns="http://schemas.microsoft.com/exchange/services/2006/messages" />                
            //    //Set “XrmSharingSourceUser” with the Object - Id(AAD ID) of the current user                
            //    //e.g. <ExtendedFieldURI PropertySetId="1a417774-4779-47c1-9851-e42057495fca" PropertyName="XrmSharingSourceUser" PropertyType="String" xmlns="http://schemas.microsoft.com/exchange/services/2006/messages" />

            //    addProp = new SingleValueExtendedProperty();
            //    addProp.PropertyId = "String 1a417774-4779-47c1-9851-e42057495fca Name XrmSharingSourceUserDisplayName";
            //    addProp.Value = MigrationHelper.CurrentUser.DisplayName;
            //    //ocmDeal.SingleValueExtendedProperties[1] = addProp;
            //    ocmDeal.SingleValueExtendedProperties[0] = addProp; //Index - 1 if not setting BCMID

            //    addProp = new SingleValueExtendedProperty();
            //    addProp.PropertyId = "String 1a417774-4779-47c1-9851-e42057495fca Name XrmSharingSourceUser";
            //    addProp.Value = MigrationHelper.CurrentUserID;
            //    //ocmDeal.SingleValueExtendedProperties[2] = addProp;
            //    ocmDeal.SingleValueExtendedProperties[1] = addProp; //Index - 1 if not setting BCMID                
            //}

            //=========================================================================================
            //Manual mappings
            //=========================================================================================
            setProperties:

            if (FieldMappings.BCMOpportunityFields.BCMFields == null) //Why would this be null?
                goto populateDetails;
            if (FieldMappings.BCMOpportunityFields.BCMFields.MappedFields.Count == 0) //Just the BCMID, no custom fields
                goto populateDetails;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //HIGH Filter SQL query by Opp GUID instead of looping
                using (SqlCommand com = new SqlCommand(Resources.BCM_CustomFields_Opportunities, con))
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
                                    string oppID = Convert.ToString(reader["EntryGUID"]);

                                    if (!String.IsNullOrEmpty(oppID) && oppID != opportunity.EntryGUID.ToString())
                                        continue;

                                    foreach (var bcmField in FieldMappings.BCMOpportunityFields.BCMFields.MappedFields)
                                    {
                                        if (bcmField.OCMFieldMapping == null)
                                        {
                                            Log.ErrorFormat("Unknown error for opportunity'{0}'", opportunity.OpportunityName);
                                            OnError(null, new HelperEventArgs(String.Format("Unknown error (D) in ImportDealAsync for opportunity'{0}'", opportunity.OpportunityName), HelperEventArgs.EventTypes.Error));
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

                                                    if (bcmField.OCMFieldMapping.FieldType == FieldMappings.OCMField.OCMFieldTypes.Date)
                                                    {
                                                        //TESTED Convert to proper date format
                                                        DateTime convertedDate = DateTime.Parse(valStr);
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
                                    }
                                    break;//we found the Opportunity and are done
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Log.Error(ex);
                            OnError(null, new HelperEventArgs(String.Format("Unknown error (A) in ImportDealAsync for Deal '{0}'", opportunity.OpportunityName), HelperEventArgs.EventTypes.Error, ex));
                        }
                    }
                }
            }

            try
            {
                //TESTED Now loop through MappedFields and find non-custom BCM fields; set values of these fields
                foreach (var bcmField in FieldMappings.BCMOpportunityFields.BCMFields.MappedFields)
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
                        PropertyInfo prop = opportunity.GetType().GetProperty(bcmField.Name, BindingFlags.Public | BindingFlags.Instance);

                        string val = "";

                        try
                        {
                            val = prop.GetValue(opportunity).ToString();
                        }
                        catch (Exception e)
                        {}                        
                        if (!String.IsNullOrEmpty(val))
                        {
                            SingleValueExtendedProperty prop2 = new SingleValueExtendedProperty();
                            prop2.PropertyId = String.Format("{0} {1} Name {2}",
                                bcmField.OCMFieldMapping.DataTypeLabelForJSON, "{1a417774-4779-47c1-9851-e42057495fca}",
                                bcmField.OCMFieldMapping.PropertyID);
                            //prop2.Value = val;
                            if (bcmField.OCMFieldMapping.FieldType == FieldMappings.OCMField.OCMFieldTypes.Date)
                            {
                                //TESTED Convert to proper date format
                                //prop2.Value = (DateTimeOffset) val;
                                DateTime convertedDate = DateTime.Parse(val);
                                //Convert.ToDateTime(reader["CreatedOn"]).ToString("yyyy-MM-ddTHH:MM:ssZ");
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
                ocmDeal.SingleValueExtendedProperties = customProps;                     
            
            //=========================================================================================


            //=========================================================================================
            //Automatic mappings
            //=========================================================================================
            populateDetails:

            try
            {
                //Set default properties
                OCMDeal2.Appliedhashtag appliedhashtag = new OCMDeal2.Appliedhashtag();                
                OCMDeal2.Createdby createdBy =  new OCMDeal2.Createdby();

                createdBy.Address = MigrationHelper.CurrentUser.EmailAddress;
                createdBy.Name = MigrationHelper.CurrentUser.DisplayName;            
                appliedhashtag.CreatedBy = createdBy;
                appliedhashtag.Application = "Outlook";
                //TESTED NOTE Hashtags cannot have dashes; semi-colons, commas, spaces or periods or a 500 Internal Server error will be thrown when creating the item!
                string hashTag = opportunity.OpportunityName.Replace(" ", "");
                hashTag = hashTag.Replace(".", "");
                hashTag = hashTag.Replace(",", "");
                hashTag = hashTag.Replace("-", "");
                hashTag = hashTag.Replace(";", "");

                appliedhashtag.Hashtag = String.Format("#{0}Deal", hashTag);

                //ocmDeal.Id = opportunity.ParentEntryID; //Id doesn't need to be set
                ocmDeal.AppliedHashtags = new OCMDeal2.Appliedhashtag[1];
                ocmDeal.AppliedHashtags[0] = appliedhashtag; 
                               
                //TESTED Get Contact/Account name and ID
                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        string sql = Resources.BCM_Opportunity_Core;
                        sql = sql.Replace("{0}", opportunity.EntryGUID.ToString());
                        using (SqlCommand com = new SqlCommand(sql, con))
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
                                            ocmDeal.OwnerId = Convert.ToString(reader["EntryGUID"]);
                                            ocmDeal.Owner = Convert.ToString(reader["FullName"]);                                            
                                            break;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Log.Error(e);
                                    OnError(null, new HelperEventArgs("Unexpected error in ImportDealAsync", HelperEventArgs.EventTypes.Error, e));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    OnError(null, new HelperEventArgs(String.Format("Could not retrieve owner details for Opportunity '{1}' (ID = {0})'", opportunity.ContactServiceID, opportunity.OpportunityName), HelperEventArgs.EventTypes.Error, ex));
                    return;
                }

                if (existingDeal != null)
                {
                    //NOTE: no longer used because we are skipping existing deals
                    //"Seed" object with existing values; they will be overwritten below if they are not empty/null
                    ocmDeal.Name = existingDeal.Name;
                    ocmDeal.Priority = existingDeal.Priority;
                    ocmDeal.Status = existingDeal.Status;
                    ocmDeal.Amount = existingDeal.Amount;
                    ocmDeal.CloseTime = existingDeal.CloseTime;
                    ocmDeal.Probability = existingDeal.Probability;
                }

                //Set BCM Properties
                //TESTED Skip null value (do not update existing non-empty value with an empty/null value from BCM)
                if (!String.IsNullOrEmpty(opportunity.OpportunityName))
                    ocmDeal.Name = opportunity.OpportunityName;

                ocmDeal.Priority = "Normal";

                //Get ID for Open stage? In-Progress stage?? Or by OpportunityStage value?
                //string stageID = DealTemplate.GetInProgressStageID();
                string stageName = opportunity.OpportunityStage;
                string stageID = DealTemplate.GetStageIDByName(stageName);
                if (String.IsNullOrEmpty(stageID))
                {
                    Log.WarnFormat("Stage '{0}' for opportunity '{1}' does not exist or is marked as deleted; setting to 'In-progress'", stageName, opportunity.OpportunityName);
                    OnDisplayMessage(null, new HelperEventArgs(String.Format("Stage '{0}' for opportunity '{1}' does not exist or is marked as deleted; setting to 'In-progress'", stageName, opportunity.OpportunityName), HelperEventArgs.EventTypes.Error));
                    //return;
                    NumberOfErrors += 1;
                    stageID = "99f9d045-5a46-403b-86e2-008cb5b1d67b";
                }
                if (!String.IsNullOrEmpty(stageName))
                    ocmDeal.Stage = stageID;

                //A

                //Use Opportunity name to lookup an existing account, skip if a deal with the same name already exists or opportunity with empty name (see report below)
                //a. Use Opportunity name to lookup an existing account, skip if a deal with the same name already exists or opportunity with empty name
                //b. Link to business contact (set business contact to empty with email does not exists; cross-reference with report - see PDF)
                //c.If deal stage does not exist in OCM (either because deal stage > 10 or the opportunity was using deleted deal stage), then Migrate Deal and their activity history (see mapping in PDF)
                //c1. set the OCM deal stage = "In-progress" (note that the string could be localized, need to use the stage ID)
                //c2. Report the opportunity name and the "unknown" BCM opportunity stage (See PDF #E.3.d.)

                //NOTE auto map stage values: Closed Won -->  Won; Closed Lost --> Lost; Custom stage -> custom stage                
                //NOTE It is important to maintain the mapping of Stage and Status when working with Deals. Meaning, if changing a Deal's Stage from In-Progress to Won it is important to also change the Status from Open to Success in the same PATCH operation.

                if (stageName == "Closed Won")
                    ocmDeal.Stage = "41bb9228-43d5-4c57-9054-fb4f1576deea"; //Use ID to bypass localized strings
                if (stageName == "Closed Lost")
                    ocmDeal.Stage = "fc020cd9-69c5-4465-9677-7612903d7875"; //Use ID to bypass localized strings
                ocmDeal.Status = DealTemplate.GetStageStatusByStageID(ocmDeal.Stage);

                //BUGFIXED When OpportunityTotal = 0.0000: Nullable object must have a value TESTED Works just setting it to 0.0000
                //if (opportunity.OpportunityTotal.Value != 0)

                try
                {
                    ocmDeal.Amount = opportunity.OpportunityTotal;
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }
                try
                {
                    if (opportunity.OpportunityCloseDate != null)
                    {
                        if (opportunity.OpportunityCloseDate.Value != null) //BUGFIXED When date is null: Nullable object must have a value
                            ocmDeal.CloseTime = (DateTimeOffset) opportunity.OpportunityCloseDate.Value;
                    } 
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }
                try
                {
                    if (opportunity.Probability != null)
                    {
                        if (opportunity.Probability.Value != 0)
                            ocmDeal.Probability = Convert.ToInt32(opportunity.Probability.Value);   //REVIEW Conversion loses decimal values 
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                OnError(null, new HelperEventArgs(String.Format("Unknown error (B) in CreateDealAsync for Opportunity '{0}'", opportunity.OpportunityName), HelperEventArgs.EventTypes.Error, ex));
            }

            if (importMode == ImportModes.Create)
            {
                //TESTED Set mailbox guid to share the deal
                ocmDeal.SourceMailboxGuid = MigrationHelper.GroupMailboxID;
            }

            //=====================================================================================

            var json = JsonConvert.SerializeObject(ocmDeal);

            try
            {
                Uri uri;

                if (importMode == ImportModes.Create)
                {
                    uri = new Uri(String.Format("{0}/XrmDeals?%24expand=AppliedHashtags", Properties.Settings.Default.BetaEndPoint));

                    //To make a Deal shared: make a REST call to ‘/move’ with the mailbox guid of the modern-group, not the folder-id.
                    //When sharing any item, the following properties must be set: 
                    //Set “XrmSharingSourceUserDisplayName” with the display name of the current user
                    //Set “XrmSharingSourceUser” with the Object - Id(AAD ID) of the current user
                    //NOTE: The above instructions are incorrect. To share a deal, just set the SourceMailboxGuid prop on the deal object! Do it above before serializing

                    Log.VerboseFormat("Posting to {1}:{0}", json, uri);

                    PrepareRequest(RequestDataTypes.Deals, RequestDataFormats.JSON);
                    _httpClient.DefaultRequestHeaders.Add("X-DealTemplateVersion", DealTemplate.Version.ToString());
                    //https://outlook.office.com/api/beta/Me/XrmDeals?%24expand=AppliedHashtags
                    //BUGFIXED "{\"error\":{\"code\":\"RequestBodyRead\",\"message\":\"Cannot convert the literal '0.9' to the expected type 'Edm.Int32'.\"}}"            
                    //BUGFIXED {"error":{"code":"RequestBroker-ParseUri","message":"Could not find a property named 'AppliedHashtags' on type 'Microsoft.OutlookServices.XrmDeal'."}}

                    //BUGFIXED ErrorInternalServerError: "Invalid custom property id\"; happens when trying to add BCMID custom prop. Can add it with a subsequent PATCH request with the BCM ID - as below

                    using (var response = await _httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json")))
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        // Check status code.
                        if (!response.IsSuccessStatusCode)
                        {
                            //BUGWATCH with hashtag InterestedinMountainBikes47ofthenewmodelMountain400andMountain400WDeal
                            Log.ErrorFormat("ERROR creating deal '{1} (Hashtag: {2})': {0}", content, opportunity.OpportunityName, ocmDeal.AppliedHashtags[0].Hashtag);
                            OnError(null,
                                new HelperEventArgs(String.Format("ERROR creating deal '{1} (Hashtag: {2})': {3}:{0}", content, opportunity.OpportunityName, ocmDeal.AppliedHashtags[0].Hashtag, response.ReasonPhrase),
                                    HelperEventArgs.EventTypes.Error));
                            NumberOfErrors += 1;
                        }
                        else
                        {
                            //TESTED Get response with deal details and add to in-memory collection
                            Log.DebugFormat("Created Deal '{0}' (Hashtag: {1})", opportunity.OpportunityName, ocmDeal.AppliedHashtags[0].Hashtag);
                            OnCreateItemComplete(null, new HelperEventArgs(String.Format("Created Deal '{0}' (Hashtag: {1})", opportunity.OpportunityName, ocmDeal.AppliedHashtags[0].Hashtag), false));
                            NumberCreated += 1;

                            try
                            {
                                Deal newDeal = JsonConvert.DeserializeObject<Deal>(content);
                                newDeal.BCMID = opportunity.EntryGUID.ToString();
                                OCMDeals.Add(newDeal);
                                if (OCMDealsCreated == null)
                                    OCMDealsCreated = new List<Deal>();
                                OCMDealsCreated.Add(newDeal);

                                //HIGH Bypass patching deal to store the custom BCMID prop and value, until the bug with SingleValueExtendedProperties on deals is resolved
                                //await PatchDeal(ocmDeal, newDeal.Id, newDeal.BCMID);
                            }
                            catch (System.Exception ex)
                            {
                                Log.Error(ex);
                                OnError(null,new HelperEventArgs(String.Format("ERROR sharing '{1}': {0}", content, opportunity.OpportunityName),
                                        HelperEventArgs.EventTypes.Error));
                            }
                        }
                    }
                }
                else
                {
                    //NOTE: Not updating existing deals
                    //uri = new Uri(String.Format("{0}/XrmDeals('{1}')?%24expand=AppliedHashtags", Properties.Settings.Default.BetaEndPoint, existingDeal.Id));

                    //HttpMethod method = new HttpMethod("PATCH");
                    //HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    //HttpRequestMessage request = new HttpRequestMessage(method, uri) { Content = httpContent };

                    ////Debug.WriteLine(String.Format("Patching to {1}:{2}{0}{2}Token:{2}{3}", json, uri, Environment.NewLine, AccessToken));

                    //PrepareRequest(RequestDataTypes.Deals, RequestDataFormats.JSON);
                    //using (var response = await _httpClient.SendAsync(request))
                    //{
                    //    //Debug.WriteLine(response);
                    //    if (!response.IsSuccessStatusCode) // Check status code.
                    //    {
                    //        var content = await response.Content.ReadAsStringAsync();
                    //        OnError(null, new HelperEventArgs(String.Format("ERROR updating '{1}': {0}", content, opportunity.OpportunityName), HelperEventArgs.EventTypes.Error));
                    //        NumberOfErrors += 1;
                    //    }
                    //    else
                    //    {                            
                    //        OnPatchComplete(null, new HelperEventArgs(String.Format("Updated Deal '{0}'...", opportunity.OpportunityName), false));
                    //        NumberUpdated += 1;
                    //        //TESTED Do we need to set the BCMID on an existing deal? It is null, so do it anyway
                    //        existingDeal.BCMID = opportunity.EntryGUID.ToString();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                OnError(null, new HelperEventArgs(String.Format("Unknown error (C) in CreateDealAsync for Contact '{0}'", opportunity.OpportunityName), HelperEventArgs.EventTypes.Error, ex));
            }
        }
        internal async Task GetBCMOpportunities()
        {            
            try
            {
                OnStart(null, new HelperEventArgs("Getting BCM Opportunities data", HelperEventArgs.EventTypes.Status));

                using (var context = new MSSampleBusinessEntities())
                {
                    context.Database.Connection.ConnectionString = ConnectionString;

                    //NOTE Can also use BCM_Opportunity_Core.sql in Resources instead of EF
                    var opportunities = context.OpportunityFullViews.Where(a => (!a.IsDeletedLocally)).ToList();

                    BCMOpportunities = new List<string>();

                    Log.InfoFormat("Found {0} BCM Opportunities:", opportunities.Count());                  ;
                    OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} BCM Opportunities:", opportunities.Count()), HelperEventArgs.EventTypes.Status));

                    foreach (var opp in opportunities)
                    {
                        Log.InfoFormat("-{0}", opp.OpportunityName);
                        BCMOpportunities.Add(opp.OpportunityName);
                        OnGetItemComplete(null, new HelperEventArgs(String.Format(" -'{0}'", opp.OpportunityName), HelperEventArgs.EventTypes.Status));
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                OnError(null, new HelperEventArgs(String.Format("Error in DealsHelper.GetBCMOpportunities: {0}", ex.Message), HelperEventArgs.EventTypes.Error));
            }
        }
        internal List<string> GetDealStages()
        {
            List<string> result = new List<string>();

            try
            {
                using (var context = new MSSampleBusinessEntities())
                {
                    context.Database.Connection.ConnectionString = ConnectionString;

                    //SqlQuery: https://msdn.microsoft.com/en-us/library/jj592907(v=vs.113).aspx
                    var dealStages = context.PicklistsMasterLists.SqlQuery("SELECT [PicklistID], [PicklistValueGUID], [OrderID], [StringValue], [IsDeleted] FROM [dbo].[PicklistsMasterList] WHERE PicklistID = N'2272CC9E-F4B5-4419-B366-28B52FAB2789' AND IsDeleted = 0 ORDER BY OrderID").ToList();
                    //NOTE: For some reason running the query in SQL Server Object Explorer works fine without having to include every column in the SELECT clause

                    Log.InfoFormat("{0} BCM deal stages: ", dealStages.Count);
                    foreach (var dealStage in dealStages)
                    {
                        Log.InfoFormat("-{0}", dealStage.StringValue);
                        result.Add(dealStage.StringValue);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                OnError(null, new HelperEventArgs("Unknown error in GetDealStages", HelperEventArgs.EventTypes.Error, ex));
            }

            return result;
        }
        private Deal GetOCMDeal(string val, UpdateKeyTypes keyType)
        {
            if (OCMDeals == null)
                return null;

            try
            {
                switch (keyType)
                {
                    case UpdateKeyTypes.BCMID:
                        foreach (var deal in OCMDeals)
                        {
                            if (deal.SingleValueExtendedProperties != null)
                            {
                                foreach (var prop in deal.SingleValueExtendedProperties)
                                {
                                    //"String {bc013ba3-3a6d-4826-b0ec-cb703a722b09} Name BCMID"
                                    if (prop.PropertyId.ToLower() == "string {bc013ba3-3a6d-4826-b0ec-cb703a722b09} name bcmid" && prop.Value == val)
                                    {
                                        return deal;
                                    }
                                }
                            }
                        }
                        break;
                    case UpdateKeyTypes.Name:
                        var mydeal = OCMDeals.FirstOrDefault(deals => deals.Name == val);
                        return mydeal;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return null;
        }
        internal async Task GetOCMDeals()
        {
            using (Log.VerboseCall())
            {
                OCMDeals = null;
                do
                {
                    Debug.WriteLine("GetOCMDeals: calling GetOCMDealsAsync");
                    await GetOCMDealsAsync();
                    Debug.WriteLine("GetOCMDealsAsync returned");
                } while (PageSkip > 0);

                Log.InfoFormat("Found {0} existing OCM Deals:", OCMDeals?.Count);
                
                OnGetComplete(null, new HelperEventArgs(String.Format("Found {0} existing OCM Deals", OCMDeals?.Count), HelperEventArgs.EventTypes.Status));

                //foreach (var item in OCMDeals)
                //{
                //    string bcmID = item.GetBCMID();                
                //    if (!String.IsNullOrEmpty(bcmID)) //Only output existing deals to log window
                //        OnGetItemComplete(null, new HelperEventArgs(String.Format(" -{3}'{0}': {2}{1}", item.Name, !String.IsNullOrEmpty(bcmID) ? String.Format(" (BCM ID: {0})", bcmID) : "", item.Company, !String.IsNullOrEmpty(bcmID) ? "*" : ""), HelperEventArgs.EventTypes.Status));
                //}
                Debug.WriteLine("Leaving GetOCMDeals");
            }
        }
        private async Task GetOCMDealsAsync()
        {
            try
            {
                //NOTE Requests for Deals must have the following HTTP header present: Prefer: exchange.behavior=”SocialFabricInternal”
                //NOTE Requests for Deals need $expand=AppliedHashtags for the AppliedHashags property to be visible.
                string request = String.Format("{0}/XrmDeals?%24expand=AppliedHashtags", Properties.Settings.Default.BetaEndPoint);

                //Debug.WriteLine("Request to : " + request);

                if (PageSkip == 0)
                {
                    request = String.Format("{0}/XrmDeals?$expand=AppliedHashtags&$top=50", Properties.Settings.Default.BetaEndPoint);
                }
                else
                {
                    request = String.Format("{0}/XrmDeals?$expand=AppliedHashtags&$top=50&$skip={1}", Properties.Settings.Default.BetaEndPoint, PageSkip);
                }

                PrepareRequest(RequestDataTypes.Deals, RequestDataFormats.None);
                using (var response = await _httpClient.GetAsync(request))
                {
                    // Check status code.
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Log.ErrorFormat("Error for request {0}: {1}", request, responseContent);
                        OnError(null, new HelperEventArgs(String.Format("ERROR in GetOCMDealsAsync(): {0}", responseContent), HelperEventArgs.EventTypes.Error));
                        NumberOfErrors += 1;
                        return;
                    }

                    // Read and deserialize response.                
                    var content = await response.Content.ReadAsStringAsync();
                    OCMDeal deals = null;

                    //BUGFIXED Neither of these deserialize properly 
                    //jsonResponse = JsonConvert.DeserializeObject<OCMDeal.Rootobject>(content);
                    deals = JsonConvert.DeserializeObject<OCMDeal>(content);
                    //BUGFIXED +		ex	{"Input string '8000.0' is not a valid integer. Path 'value[0].Amount', line 1, position 705."}	System.Exception {Newtonsoft.Json.JsonReaderException}
                    if (OCMDeals == null)
                    {
                        OCMDeals = deals.value.ToList();
                    }
                    else
                    {
                        if (deals.value.Length > 0)
                            OCMDeals.AddRange(deals.value.ToList());
                    }

                    if (deals.value.Length < 50)
                    {
                        //No more items to get
                        PageSkip = 0;
                        Log.InfoFormat("Fetched {0} OCM Deals", OCMDeals.Count);
                        OnGetItemComplete(null, new HelperEventArgs(String.Format("Fetched {0} OCM Deals", OCMDeals.Count), HelperEventArgs.EventTypes.Status));
                    }
                    else
                    {
                        PageSkip += 50;
                    }
                }
            }
            catch (System.Exception ex)
            {                
                Log.Error(ex);
                OnError(null, new HelperEventArgs(String.Format("ERROR in GetOCMDealsAsync: {0}", ex.Message), HelperEventArgs.EventTypes.Error));
                NumberOfErrors += 1;
            }
        }
        private async Task<OCMDealTemplate.Rootobject> GetTemplateAsync()
        {
            using (Log.VerboseCall())            
            {
                try
                {
                    PrepareRequest(RequestDataTypes.Templates, RequestDataFormats.None);
                    using (
                        var response =
                            await _httpClient.GetAsync(
                                "https://outlook.office.com/api/beta/me/XrmDealTemplate('DealTemplate')"))
                    {
                        // Check status code.
                        if (!response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            Log.Error(content);
                            OnError(null,
                                new HelperEventArgs(String.Format("ERROR '{0}': {1}", response.ReasonPhrase, content),
                                    HelperEventArgs.EventTypes.Error, HelperEventArgs.ProcessingModes.Mapping));
                            NumberOfErrors += 1;
                            return null;
                        }
                        else
                        {
                            // Read and deserialize response.                
                            var content2 = await response.Content.ReadAsStreamAsync();
                            var content3 = await response.Content.ReadAsStringAsync();
                            var jsonSerializer = new DataContractJsonSerializer(typeof(OCMDealTemplate.Rootobject));
                            OCMDealTemplate.Rootobject jsonResponse = jsonSerializer.ReadObject(content2) as OCMDealTemplate.Rootobject;

                            if (jsonResponse == null)
                                return null;

                            Log.InfoFormat("Deals template version {1} retrieved with {2} stage statuses and {0} stages: {3}", jsonResponse.Template?.FieldList?.Length, jsonResponse.Template?._Version, jsonResponse.Template?.StatusList.Count(), content3);
                            OnGetComplete(null, new HelperEventArgs(String.Format("Deals template version {1} retrieved with {2} stage statuses and {0} stages", jsonResponse.Template?.FieldList?.Length, jsonResponse.Template?._Version, jsonResponse.Template?.StatusList.Count()), HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
                            return jsonResponse;
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnError(null, new HelperEventArgs(String.Format("ERROR in GetTemplateAsync: {0}", ex.Message), HelperEventArgs.EventTypes.Error, HelperEventArgs.ProcessingModes.Mapping));
                    NumberOfErrors += 1;
                    return null;
                }
            }
        }

        private async Task PatchDeal(OCMDeal2.Rootobject ocmDeal, string Id, string bcmID)
        {
            try
            {
                SingleValueExtendedProperty[] customProps = null;

                ocmDeal.SingleValueExtendedProperties = new SingleValueExtendedProperty[1];
                //Add BCMID prop
                SingleValueExtendedProperty bcmIDProp = new SingleValueExtendedProperty();
                bcmIDProp.PropertyId = String.Format("String {0} Name BCMID", FieldMappings.BCMIDPropertyGUID);
                bcmIDProp.Value = bcmID;
                ocmDeal.SingleValueExtendedProperties[0] = bcmIDProp;

                var json = JsonConvert.SerializeObject(ocmDeal);
                Uri uri;
                uri = new Uri(String.Format("{0}/XrmDeals('{1}')?%24expand=AppliedHashtags", Properties.Settings.Default.BetaEndPoint, Id));

                HttpMethod method = new HttpMethod("PATCH");
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(method, uri) { Content = httpContent };

                ////Debug.WriteLine(String.Format("Patching to {1}:{2}{0}{2}Token:{2}{3}", json, uri, Environment.NewLine, AccessToken));

                PrepareRequest(RequestDataTypes.Deals, RequestDataFormats.JSON);
                _httpClient.DefaultRequestHeaders.Add("X-DealTemplateVersion", DealTemplate.Version.ToString());
                using (var response = await _httpClient.SendAsync(request))
                {
                    //Debug.WriteLine(response);
                    if (!response.IsSuccessStatusCode) // Check status code.
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        //BUG "{\"error\":{\"code\":\"ErrorInternalServerError\",\"message\":\"Invalid custom property id\"}}". Outstanding issue being unable to add custom properties to Deals
#if DEBUG
                        Log.WarnFormat("ERROR patching deal '{1}' for BCM ID: {0}", content, ocmDeal.Name);
#else
                        Log.ErrorFormat("ERROR patching deal '{1}' for BCM ID: {0}", content, ocmDeal.Name);
#endif
#if !DEBUG
                        OnError(null, new HelperEventArgs(String.Format("ERROR patching '{1}': {0}", content, ocmDeal.Name), HelperEventArgs.EventTypes.Error));
#endif
                        NumberOfErrors += 1;
                    }
                    else
                    {
                        Log.DebugFormat("Deal '{0}' updated with BCMID {1})", ocmDeal.Name, bcmID);
                        OnPatchComplete(null, new HelperEventArgs(String.Format("Updated Deal '{0}'...", ocmDeal.Name), false));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private async Task RunCreateDealsAsync()
        {
            using (Log.VerboseCall())
            {
                using (var context = new MSSampleBusinessEntities())
                {
                    try
                    {
                        context.Database.Connection.ConnectionString = ConnectionString;
                        var deals = from mydeal in context.OpportunityFullViews where mydeal.IsDeletedLocally == false select mydeal;

                        if (TestMode)
                        {
                            NumberToProcess = TestingMaximum;
                            OnStart(null, new HelperEventArgs(String.Format("Importing Opportunities (max {0} - TESTING MODE)", TestingMaximum), false));
                        }
                        else
                        {
                            NumberToProcess = deals.Count();
                            OnStart(null, new HelperEventArgs("Importing Opportunities...", false));
                        }

                        int cnt = 0;
                        Log.InfoFormat("Importing {0} Opportunities", deals.Count());

                        foreach (var deal in deals)
                        {
                            try
                            {
                                if (TestMode && cnt == TestingMaximum)
                                    break;
                                if (Cancelled)
                                {
                                    Log.Warn("Cancelled!");
                                    break;
                                }

                                //TESTED Use Opportunity name to lookup an existing account, skip if a deal with the same name already exists or opportunity with empty name

                                if (String.IsNullOrEmpty(deal.OpportunityName))
                                {
                                    Log.WarnFormat("Skipping opportunity with blank name (ID: {0})", deal.EntryGUID);
                                    OnDisplayMessage(null, new HelperEventArgs(String.Format("Skipping opportunity with blank name (ID: {0})", deal.EntryGUID), false));
                                    continue;
                                }

                                ImportModes importMode;

                                //Can't search by BCMID as we are not able to add that as a custom prop; must search by name
                                //Deal ocmDeal = GetOCMDeal(deal.EntryGUID.ToString(), UpdateKeyTypes.BCMID);

                                Deal ocmDeal = null;

                                //Look for match on name
                                ocmDeal = GetOCMDeal(deal.OpportunityName, UpdateKeyTypes.Name);

                                if (ocmDeal != null)
                                {
                                    //Must distinguish between existing manually created Companies(no BCMID) and those previously imported (with BCMID)
                                    string bcmID = ocmDeal.GetBCMID();
                                    if (!String.IsNullOrEmpty(bcmID))
                                    {
                                        //Previously imported - IGNORE. NO! UpdateTemplate
                                        //OnCreateItemComplete(null, new HelperEventArgs(String.Format("Deal '{0}' previously imported; skipping", ocmDeal.Name), false));
                                        //continue;
                                        importMode = ImportModes.UpdatePreviouslyImportedItem;
                                        Log.DebugFormat("Updating previously imported deal '{0}'", ocmDeal.Name);                                        
                                    }
                                    else
                                    {
                                        //Update existing manually created item; NO - ignore non-imported deals
                                        //importMode = ImportModes.Update;
                                        Log.DebugFormat("Deal '{0}' already exists; skipping", ocmDeal.Name);
                                        OnCreateItemComplete(null, new HelperEventArgs(String.Format("Deal '{0}' already exists; skipping", ocmDeal.Name), false));
                                        continue;
                                    }                                    
                                }
                                else
                                {
                                    importMode = ImportModes.Create;
                                }

                                await ImportDealAsync(deal, importMode, ocmDeal);
                                cnt += 1;
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex);
                                OnError(null, new HelperEventArgs(String.Format("Unknown error in RunCreateDealAsync: {0})", ex.ToString()), HelperEventArgs.EventTypes.Error));
                            }
                        }

                        //Log created deals
                        if (OCMDealsCreated != null)
                        {
                            string[] ids = new string[OCMDealsCreated.Count];
                            int idx = 0;
                            Log.DebugFormat("Created {0} deals:", OCMDealsCreated.Count);
                            foreach (var newDeal in OCMDealsCreated)
                            {
                                Log.DebugFormat("{0};{1}", newDeal.Name, newDeal.Id);
                                ids[idx] = newDeal.Id;
                                idx += 1;
                            }
                            string idsJoined = string.Join(";", ids);
                            Log.InfoFormat("Created deal IDs: {0}", idsJoined);
                        }

                        if (!Cancelled)
                        {
                            OnHelperComplete(null, new HelperEventArgs(String.Format("Opportunities import complete!{0}Created: {1}{0}Errors: {2}", Environment.NewLine, NumberCreated, NumberOfErrors), false));
                        }
                        else
                        {
                            OnHelperComplete(null, new HelperEventArgs(String.Format("Opportunities import stopped{0}Created: {1}{0}Errors: {2}", Environment.NewLine, NumberCreated, NumberOfErrors), false));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        OnError(this, new HelperEventArgs(ex.ToString(), HelperEventArgs.EventTypes.FatalError));
                    }
                }
            }
        }
        public async Task RunGetTemplateAsync()
        {
            // Get template.
            OnStart(null, new HelperEventArgs("Retrieving Deals template...", HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
            DealTemplate = await GetTemplateAsync();
            FieldMappings.OCMDealFields.DealTemplate = DealTemplate;

            //Debug.WriteLine("Stages: " + DealTemplate.Template.StatusList.Count());
            //OnDisplayMessage(null, new HelperEventArgs(String.Format("Deals template retrieved! Version: {0}; {1} stage categories", DealTemplate.Version, DealTemplate.Template.StatusList.Count()), HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
            try
            {
                Log.InfoFormat("{0} statuses:", DealTemplate.Template.StatusList.Length);
                foreach (var status in DealTemplate.Template.StatusList)
                {
                    Log.InfoFormat("Stage category: {0} (Id: {1}); {2} stages:", status.Label, status.Id, status.Stages.Length);
                    OnDisplayMessage(null, new HelperEventArgs(String.Format("Stage category: {0} (Id: {1})", status.Label, status.Id), HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
                    foreach (var stage in status.Stages)
                    {
                        Log.InfoFormat("-{0} (Id: {1}; Deleted: {2})", stage.Label, stage.Id, stage.Deleted);
                        OnDisplayMessage(null, new HelperEventArgs(String.Format(" - {0} (Id: {1}; Deleted: {2})", stage.Label, stage.Id, stage.Deleted), HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Mapping));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        private async Task UpdateTemplateAsync()
        {
            // Serialize the deal template
            using (Log.VerboseCall())
            {
                var json = JsonConvert.SerializeObject(DealTemplate);

                try
                {
                    Uri uri = new Uri("https://outlook.office.com/api/beta/me/XrmDealTemplate('DealTemplate')");
                    HttpMethod method = new HttpMethod("PATCH");
                    HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpRequestMessage request = new HttpRequestMessage(method, uri) { Content = httpContent };

                    PrepareRequest(RequestDataTypes.Deals, RequestDataFormats.JSON);

                    try
                    {
                        using (var response = await _httpClient.SendAsync(request))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                OnUpdateTemplateComplete(null, new EventArgs());
                            }
                            else
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                Log.Error(responseContent);
                            }
                        }
                    }
                    catch (TaskCanceledException e)
                    {

                        Log.Error(e);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    OnError(null, new HelperEventArgs(String.Format("Unknown error in UpdateTemplateAsync:{0}", ex.ToString()), HelperEventArgs.EventTypes.Error, ex));
                }
            }
        }
        internal async Task UpdateTemplate()
        {
            //NOTE: When updating a Template, it is recommended to increase the version by 1. When changing the version it is necessary to set the outer Version and inner _Version to the same number.
            //NOTE: The ordering of Stages is important - the order in this array drives the order in OCM UX.
            //NOTE: Users are able to re - order Stages in OCM UX. Templates are updated with this ordering.
            //NOTE: use only the Stage ID with Deal CRUD operations, not Label.

            //TESTED Should only update ALL templates when there is a change!
            try
            {
                OCMDealTemplate.Statuslist openStatusList;
                OCMDealTemplate.Stage[] ocmStages;

                openStatusList = DealTemplate.Template.StatusList.FirstOrDefault(statuses => statuses.Label == "Open");

                int idx = openStatusList.Stages.Length;
                ocmStages = openStatusList.Stages; //Get existing stages

                //Make room for new stages in array
                //EL 2017-10-16 Must also make sure that a stage with the same name doesn't already exist - so remove any stages that do from the list 

                List<string> newStages = new List<string>();
                //EL Copy BCM stages to new list that we will remove duplicates from when we iterate and compare against the OCM list (there's probably a better way to do this)

                OnDisplayMessage(null, new HelperEventArgs(String.Format("Found {0} BCM sales stages:", DealStages.Count), HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Migrating));
                foreach (string stage in DealStages)
                {
                    OnDisplayMessage(null, new HelperEventArgs(String.Format("-{0}", stage), HelperEventArgs.EventTypes.Status, HelperEventArgs.ProcessingModes.Migrating));
                    newStages.Add(stage);
                }

                //Now loop through our original read-only list of BCM stages and look for a matching OCM stage by name
                foreach (string stage in DealStages)
                {
                    OCMDealTemplate.Stage ocmStage = openStatusList.Stages.FirstOrDefault(ocmStages2 => ocmStages2.Label == stage);
                    if (ocmStage != null)
                    {
                        //The stage already exists in OCM; remove it from our copy of the BCM list
                        newStages.Remove(stage);
                    }
                }

                //Need to count non-DELETED stages? No - throws error

                if (ocmStages.Length + newStages.Count > 10)
                {
                    //HIGH Any OCM Status cannot have more than 10 stages. We either need to get these OCM stages first to display to the user before we update them if choices are to be made for which to exclude. Will confirm with Welly
                    Log.WarnFormat("{0} new deal {1} would exceed the maximum of 10 per-status - ignoring extra stages", newStages.Count, newStages.Count == 1 ? "stage" : "stages");
                    OnDisplayMessage(null, new HelperEventArgs(String.Format("{0} new deal {1} would exceed the maximum of 10 per-status - ignoring extra stages", newStages.Count, newStages.Count == 1 ? "stage" : "stages"), HelperEventArgs.EventTypes.Error, HelperEventArgs.ProcessingModes.Migrating));
                    OnUpdateTemplateComplete(null, new EventArgs()); //REVIEW Change event to handle error message/condition?
                    return;
                }

                Array.Resize(ref ocmStages, ocmStages.Length + newStages.Count);

                Log.InfoFormat("Adding {0} new stages to template:", newStages.Count);
                foreach (string stage in newStages)
                {
                    OCMDealTemplate.Stage ocmStage = new OCMDealTemplate.Stage();
                    ocmStage.Id = Guid.NewGuid().ToString();
                    ocmStage.Label = stage;
                    ocmStage.Deleted = false;
                    ocmStages[idx] = ocmStage; //Add to indexed element of the array
                    idx += 1;
                    Log.InfoFormat("-{0}", stage);
                }

                openStatusList.Stages = ocmStages;
                Log.InfoFormat("Updating template to version {0} from {1}", DealTemplate.Version + 1, DealTemplate.Version);
                DealTemplate.Version += 1;                
                DealTemplate.Template._Version = DealTemplate.Version;
                await UpdateTemplateAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                OnError(null, new HelperEventArgs(String.Format("Unknown error in DealsHelper.UpdateTemplate:{0}", ex.ToString()), HelperEventArgs.EventTypes.Error, ex));                
            }            
        }

#endregion
    }
}
