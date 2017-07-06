using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BCM_Migration_Tool.Properties;
using Newtonsoft.Json;
using TracerX;

namespace BCM_Migration_Tool.Objects
{
    internal class ActivitiesHelper: HelperBase
    {
        private static readonly Logger Log = Logger.GetLogger("ActivitiesHelper");
        #region Constructors
        internal ActivitiesHelper(string accessToken, string connectionString)
            : base(accessToken, connectionString)
        { }
        #endregion
        #region Properties
        //internal List<ActivitiesTable> BCMActivities { get; set; }
        #endregion
        #region Methods
        internal async Task<int> CreateActivity(string bcmEntityGUID, string itemID, string displayName, FieldMappings.BCMDataSetTypes entityType)
        {
            //NOTE: OCMCompany use XrmId; other types use the ItemLinkID
            //NOTE Use ItemLinkId from Persona for Contact activities

            int result = 0;

            try
            {            
                string sql = String.Format("{0} AND CMT.EntryGUID = '{1}'", Resources.BCM_Activities, bcmEntityGUID);
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (
                        SqlCommand com =
                            new SqlCommand(sql, con)
                    )
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
                                        OCMActivity ocmActivity = new OCMActivity();

                                        try
                                        {
                                            ocmActivity.ActionVerb = "Post";
                                            //ocmActivity.DisplayName = Convert.ToString(reader["EntityName"]);
                                            ocmActivity.DisplayName = displayName;
                                            //BUGFIXED: {"error":{"code":"RequestBodyRead","message":"Cannot convert the literal '11/21/2016 6:26:28 PM' to the expected type 'Edm.DateTimeOffset'."}}
                                            //Use format as per: EventTime=2016-11-30T19:00:00Z
                                            string activityDate = Convert.ToDateTime(reader["CreatedOn"])
                                                .ToString("yyyy-MM-ddTHH:MM:ssZ");
                                            ocmActivity.EventTime = activityDate; //REVIEW Is CreatedOn needed for activities?

                                            //TESTED Date format 2016-12-08T18:59:53.463Z                                                                                      
                                            //ModifiedProperties=[{"key":"IPM.Contact","value":{"key":"2bf753a8-9cb4-4617-ab52-a6aa1a268f80","value":""}}]
                                            //ModifiedProperties=[{"key":"IPM.Contact.Company","value":{"key":"ad83d70c-16aa-42e6-a546-b817678faf14","value":""}}]

                                            string keyType = "";
                                            switch (entityType)
                                            {
                                                case FieldMappings.BCMDataSetTypes.Accounts:
                                                    keyType = "IPM.Contact.Company";
                                                    break;
                                                case FieldMappings.BCMDataSetTypes.BusinessContacts:
                                                    keyType = "IPM.Contact";
                                                    break;
                                                case FieldMappings.BCMDataSetTypes.Opportunities:
                                                    keyType = "IPM.XrmProject.Deal";
                                                    break;
                                            }
                                            //string keyProp = String.Format("[{{\"key\":\"{0}\",\"value\":{{\"key\":\"{1}\",\"value\":\"\"}}]", entityType == FieldMappings.BCMDataSetTypes.Accounts ? "IPM.Contact.Company" : "IPM.Contact", entityType == FieldMappings.BCMDataSetTypes.Accounts ? itemID : bcmEntityGUID);

                                            //BUGFIXED Wrong format!
                                            string keyProp = "";// = String.Format("[{{\"key\":\"{0}\",\"value\":{{\"key\":\"{1}\",\"value\":\"\"}}]", keyType, itemID);
                                            keyProp = String.Format("[{{\"key\":\"{0}\",\"value\":{{\"key\":\"{1}\",\"value\":\"\"}}}}]", keyType, itemID);

                                            //"[{\"key\":\"IPM.Contact\",\"value\":{\"key\":\"AAQkAGUzNmEzYTBmLTI1NDItNGE0My1iZDk5LWFkMDgxODI3YWNlOQAQAP1JgjndmL1PlZaF3njr2AA=\",\"value\":\"\"}]"

                                            ocmActivity.ModifiedProperties = keyProp;
                                            ocmActivity.SourceUser = MigrationHelper.CurrentUser.Id;
                                            //TESTED Verify SourceUser value source

                                            string subType = "";
                                            switch (Convert.ToString(reader["ActivityType"]))
                                            {
                                                case "14":
                                                    subType = "TextPost";
                                                    break;
                                                case "15":
                                                    subType = "CallLog";
                                                    break;
                                                default:
                                                    subType = "MeetingLog";
                                                    break;
                                            }
                                            ocmActivity.Subtype = subType;
                                            ocmActivity.Text = Convert.ToString(reader["Subject"]);
                                            if (!String.IsNullOrEmpty(Convert.ToString(reader["ActivityNote"])))
                                                ocmActivity.Text = String.Concat(ocmActivity.Text, Environment.NewLine,
                                                    Convert.ToString(reader["ActivityNote"]));

                                            var json = JsonConvert.SerializeObject(ocmActivity);

                                            Uri uri = new Uri(String.Format("{0}/XrmActivityStreams", Properties.Settings.Default.BetaEndPoint));

                                            Log.VerboseFormat("Posting to {1}: {0}", json, uri);
                                            PrepareRequest(RequestDataTypes.Activities, RequestDataFormats.JSON);                                                    
                                            using (var response = await _httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json")))
                                            {
                                                //BUGFIXED {"error":{"code":"ErrorInternalServerError","message":"ItemLinkId (AAMkAGUzNmEzYTBmLTI1NDItNGE0My1iZDk5LWFkMDgxODI3YWNlOQBGAAAAAACK2VEhi72QSaw_u0XV7xUHBwCMotTyA3QkQ7TPAmcrRt4FAAAALwVNAAAuH-1UA8tzTYD5jbYriaIUAANZ-MJ5AAA=) specified in ModifiedProperties for Activity Post linking is not of Guid format."}}

                                                //BUGFIXED Creating activities: {"error":{"code":"ErrorInternalServerError","message":"An error occured : Failed to create 'ActivityPertainsTo' link. Error: Failed to retrieve ToNode: Person (Guid: c30bf69f-9f87-4711-a581-f4e04a25a08f).\r\n"}}
                                                //For: {"Id":null,"Subtype":"TextPost","ActionVerb":"Post","DisplayName":"Mr. Robert E. Ahlering","EventTime":"2016-11-21T17:11:08Z","SourceUser":"1320a731-7b00-45de-a94f-320a646be41c@09c30aae-5947-41b4-b312-3886ba8f95f3","ModifiedProperties":"[{\"key\":\"IPM.Contact\",\"value\":{\"key\":\"c30bf69f-9f87-4711-a581-f4e04a25a08f\",\"value\":\"\"}]","Text":"Note Title 1\r\n11/21/2016 4:59:37 PM:This is a note detail 1A\r\n11/21/2016 4:59:59 PM:This is note detail 1B\r\n"}

                                                //Nick's example:
                                                //{ "Id":null,"Subtype":"CallLog","ActionVerb":"Post","DisplayName":"Armando Pinto (XRM)","EventTime":"2017-05-01T23:00:00Z","SourceUser":"9cb807e1-2cda-49da-bfeb-e0c4d2986fc4","InlineLinks":{ "Relationships":[]},"ModifiedProperties":"[{\"key\":\"IPM.Contact\",\"value\":{\"key\":\"b4103d19-f80b-479d-b21a-f0f697825fcc\",\"value\":\"\"}}]","LinkType":"","LinkedEntityNames":"","TargetEntities":[],"OtherRelatedEntities":[],"Text":"a call log","XrmId":null} 

                                                // Check status code.
                                                if (!response.IsSuccessStatusCode)
                                                {
                                                    var content = await response.Content.ReadAsStringAsync();
                                                    Log.ErrorFormat("ERROR creating activity '{1}': {0}", content, ocmActivity.Text);
                                                    OnError(null, new HelperEventArgs(String.Format("ERROR creating activity '{1}': {0}", content, ocmActivity.Text), HelperEventArgs.EventTypes.Error));
                                                    NumberOfErrors += 1;
                                                }
                                                else
                                                {
                                                    Log.DebugFormat("Imported {0} ({2}) activity for '{1}'...", ocmActivity.Subtype, ocmActivity.DisplayName, ocmActivity.ActionVerb);
                                                    OnCreateItemComplete(null, new HelperEventArgs(String.Format("Imported {0} activity for '{1}'...", ocmActivity.Subtype, ocmActivity.DisplayName), false));
                                                    NumberCreated += 1;
                                                    result += 1;
                                                }
                                            }                                                
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Log.Error(ex);
                                            NumberOfErrors += 1;
                                            //OnError(null, new HelperEventArgs( String.Format("Unknown error (C) in CreateActivity for item '{0}'", displayName), HelperEventArgs.EventTypes.Error, ex));
                                        }
                                    }
                                }
                                else
                                {
                                    //Log.VerboseFormat("No results for query: {0}", sql);
                                    //leave; no activities to create
                                    return 0;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                Log.Error(ex);
                                NumberOfErrors += 1;
                                //OnError(null, new HelperEventArgs( String.Format("Unknown error (A) in CreateActivity for item '{0}'", displayName), HelperEventArgs.EventTypes.Error, ex));
                            }
                        }
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                NumberOfErrors += 1;
                //OnError(null, new HelperEventArgs(String.Format("Unknown error (B) in CreateActivity for item '{0}'", displayName), HelperEventArgs.EventTypes.Error, ex));
            }

            return result;
        }

        internal async Task CreateCompanyActivities()
        {
            using (Log.VerboseCall())
            {
                int numCreated = 0;
                int totalCreated = 0;
                int errors = 0;

                try
                {
                    NumberToProcess = AccountsHelper.OCMAccounts.Count;
                    //Log.InfoFormat("Importing ");
                    OnStart(null, new HelperEventArgs(String.Format("Looking for activities to import from {0} accounts", AccountsHelper.OCMAccounts.Count), HelperEventArgs.EventTypes.Status));

                    foreach (OCMCompany2Value company in AccountsHelper.OCMAccounts)
                    {
                        try
                        {
                            //Note that BCMID on existing companies are the same as the BCM Account GUID. Also make sure we ignore non-imported companies? Or handle in CreateActivity(); if no activities record in DB, none will be created anyway
                            //REVIEW Check for existing activities per-company?? see if /api/beta/Me/XrmActivityStreams can go a GET with XrmId as param; do within CreateActivity?
                            string bcmID = "";
                            bcmID = company.GetBCMID();
                            if (!String.IsNullOrEmpty(bcmID))
                            {
                                numCreated = await CreateActivity(bcmID, company.XrmId, company.DisplayName,
                                    FieldMappings.BCMDataSetTypes.Accounts);
                                totalCreated += numCreated;
                            }
                            else
                            {
                                Log.VerboseFormat("Not creating activity (no BCM ID for company '{0}' (ID: {1}))", company.DisplayName, company.Id);
                                //Not an error!
                                //OnError(null, new HelperEventArgs(String.Format("No BCM ID for company '{0}' (XRM ID: {1})", company.DisplayName, company.XrmId), HelperEventArgs.EventTypes.Error));
                                //errors += 1;
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            NumberOfErrors += 1;
                            Log.ErrorFormat("Unknown error creating activity for company '{0}' (ID: {1}): {2}", company.DisplayName, company.Id, ex);
                            //OnError(null, new HelperEventArgs(String.Format("Unknown error in CreateCompanyActivities for company '{0}' (XRM ID: {1}): {2}", company.DisplayName, company.XrmId, ex.ToString()), HelperEventArgs.EventTypes.Error));
                        }
                    }

                    Log.InfoFormat("{0} total company activities created ({1} errors).", totalCreated, errors);

                }
                catch (Exception ex)
                {
                    NumberOfErrors += 1;
                    Log.Error(ex);
                }

                OnHelperComplete(null, new HelperEventArgs(String.Format("{0} total company activities created ({1} errors).", totalCreated, errors), HelperEventArgs.EventTypes.Status));
            }
        }
        internal async Task CreateContactActivities()
        {
            using (Log.VerboseCall())
            {
                int numCreated = 0;
                int totalCreated = 0;
                int errors = 0;

                try
                {
                    NumberToProcess = ContactsHelper.OCMContacts.Count;
                    OnStart(null, new HelperEventArgs(String.Format("Looking for activities to import from {0} contacts", ContactsHelper.OCMContacts.Count), HelperEventArgs.EventTypes.Status));

                    foreach (OCMContact2Value contact in ContactsHelper.OCMContacts)
                    {
                        try
                        {
                            //NOTE that BCMID on existing companies are the same as the BCM Account GUID. Also make sure we ignore non-imported companies? Or handle in CreateActivity(); if no activities record in DB, none will be created anyway
                            //NOTE Check for existing activities per-company; see if /api/beta/Me/XrmActivityStreams can go a GET with XrmId as param; do within CreateActivity?
                            string bcmEntityGUID = "";
                            bcmEntityGUID = contact.GetBCMID();

                            //NOTE Use Persona ID, NOT BCMID!!!! Contact.Id is good? Wait - CreateActivity says use ItemLInkID from Persona for Contact activities
                            if (!String.IsNullOrEmpty(bcmEntityGUID))
                            {
                                if (String.IsNullOrEmpty(contact.ItemLinkId))
                                {
                                    Log.ErrorFormat("Not creating activity (no ItemLinkId for company '{0}' (ID: {1}))", contact.DisplayName, contact.Id);
                                    OnError(null, new HelperEventArgs(String.Format("No linking ID for contact '{0}'", contact.DisplayName), HelperEventArgs.EventTypes.Error));
                                    errors += 1;
                                    continue;
                                }
                                //BUGWATCH Previously imported contacts have a blank ItemLinkId; perhaps running FindPeople quickly after CreateContacts doesn't allow for a refresh??
                                numCreated = await CreateActivity(bcmEntityGUID, contact.ItemLinkId, contact.DisplayName,
                                    FieldMappings.BCMDataSetTypes.BusinessContacts);
                                totalCreated += numCreated;
                            }
                            else
                            {
                                Log.VerboseFormat("Not creating activity (no BCM ID for contact '{0}' (ID: {1}))", contact.DisplayName, contact.Id);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.ErrorFormat("Unknown error creating activity for contact '{0}' (ID: {1}): {2}", contact.DisplayName, contact.Id, ex);
                            NumberOfErrors += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    NumberOfErrors += 1;
                    Log.Error(ex);
                }
                OnHelperComplete(null, new HelperEventArgs(String.Format("{0} total contact activities created ({1} errors).", totalCreated, errors), HelperEventArgs.EventTypes.Status));
            }
        }
        internal async Task CreateDealActivities()
        {
            using (Log.VerboseCall())
            {
                int numCreated = 0;
                int totalCreated = 0;
                int errors = 0;

                try
                {
                    NumberToProcess = DealsHelper.OCMDeals.Count;
                    OnStart(null, new HelperEventArgs(String.Format("Importing activities from {0} deals", DealsHelper.OCMDeals.Count), HelperEventArgs.EventTypes.Status));

                    foreach (Deal deal in DealsHelper.OCMDealsCreated) //HIGH Switch to OCMDeals if BCMID can be stored on deals after creating them? If we do I believe we will be skipping previously imported deals...that might be desirable...
                    {
                        try
                        {
                            string bcmEntityGUID = "";
                            bcmEntityGUID = deal.GetBCMID();

                            if (!String.IsNullOrEmpty(bcmEntityGUID))
                            {
                                if (String.IsNullOrEmpty(deal.XrmId))
                                {
                                    Log.ErrorFormat("Not creating activity (no ItemLinkId for company '{0}' (ID: {1}))", deal.Name, deal.Id);
                                    OnError(null, new HelperEventArgs(String.Format("No linking ID for deal '{0}'", deal.Name), HelperEventArgs.EventTypes.Error));
                                    errors += 1;
                                    continue;
                                }
                                numCreated = await CreateActivity(bcmEntityGUID, deal.XrmId, deal.Name, FieldMappings.BCMDataSetTypes.Opportunities);
                                totalCreated += numCreated;
                            }
                            else
                            {
                                Log.VerboseFormat("Not creating activity (no BCM ID for deal '{0}' (ID: {1}))", deal.Name, deal.Id);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.ErrorFormat("Unknown error creating activity for deal '{0}' (ID: {1}): {2}", deal.Name, deal.Id, ex);
                            NumberOfErrors += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    NumberOfErrors += 1;
                    Log.Error(ex);
                }
                OnHelperComplete(null, new HelperEventArgs(String.Format("{0} total deal activities created ({1} errors).", totalCreated, errors), HelperEventArgs.EventTypes.Status));
            }
        }
        #endregion
    }
}
