using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using BcmMigrationTool;
using BCM_Migration_Tool.Controls;
using BCM_Migration_Tool.Objects;
using BCM_Migration_Tool.Properties;
using Microsoft.Exchange.WebServices.Auth.Validation;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.VisualBasic;
using TracerX;

namespace BCM_Migration_Tool
{
    public partial class MigrationAssistant : Form
    {
        private static readonly Logger Log = Logger.GetLogger("MigrationAssistant");
        private string _accessToken;
        #region Constructors
        public MigrationAssistant()
        {
            InitializeComponent();
            navigationBar1.ConnectClicked += navigationBar1_ConnectClicked;
            navigationBar1.MigrateClicked += navigationBar1_MigrateClicked;
            navigationBar1.MapClicked += navigationBar1_MapClicked;
            navigationBar1.ConfigClicked += navigationBar1_ConfigClicked;
            connect1.LoginClicked += connect1_LoginClicked;
            connect1.AuthenticationChanged += connect1_AuthenticationChanged;
            connect1.TestDBConnectionClicked += connect1_TestDBConnectionClicked;
            migrate1.StartClicked += migrate1_StartClicked;
            migrate1.StopClicked += migrate1_StopClicked;
            configure1.cmdDeleteDeals.Click += cmdDeleteDeals_Click;            
            configure1.chkEnableTestMode.CheckedChanged += chkEnableTestMode_CheckedChanged;
        }
        #endregion

        #region Properties
        public string AccessToken
        {
            get { return _accessToken; }
            set
            {
                Log.InfoFormat("Token set: {0} (old: {1})", value, _accessToken);
                if (accountsHelper != null)
                    accountsHelper.AccessToken = value;
                if (activitiesHelper != null)
                    activitiesHelper.AccessToken = value;
                if (contactsHelper != null)
                    contactsHelper.AccessToken = value;
                if (dealsHelper != null)
                    dealsHelper.AccessToken = value;
                _accessToken = value;                
            }
        }

        private AccountsHelper accountsHelper { get; set; }
        private ActivitiesHelper activitiesHelper { get; set; }
        private ContactsHelper contactsHelper { get; set; }
        private DealsHelper dealsHelper { get; set; }
        //private AccountsHelper accountsHelper { get { return MigrationHelper.accountsHelper; } }
        private string ConnectionString { get; set; }        
        private bool MappingsInitialized { get; set; }
        private bool MigrateClicked { get; set; }
        #endregion

        #region Form Events
        private void MigrationAssistant_Load(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                navigationBar1.BringToFront();
                LoadSettings();
#if !DEBUG
                ToggleTestModeControls(false);
#endif

                //#if !DEBUG
                //                numericUpDown1.Visible = false;
                //                chkGetOnly.Visible = false;
                //                chkTestMode.Visible = false;
                //                chkDebugMode.Visible = false;
                //#endif
#if DEBUG
                //TestDBConnection();
                configure1.chkEnableTestMode.Checked = true;
                
#endif
            }
        }
        #endregion
        #region Methods
        private void InitializeHelpers()
        {
            using (Log.VerboseCall())
            {
                if (accountsHelper == null)
                {
                    accountsHelper = new AccountsHelper(AccessToken, ConnectionString);
                    //MigrationHelper.accountsHelper .AccessToken = AccessToken;

                    accountsHelper.CreateItemComplete += accountsHelper_CreateItemComplete;
                    accountsHelper.GetComplete += accountsHelper_GetComplete;
                    accountsHelper.GetItemComplete += accountsHelper_GetItemComplete;
                    accountsHelper.PatchComplete += accountsHelper_PatchComplete;
                    accountsHelper.Error += accountsHelper_Error;
                    accountsHelper.Started += accountsHelper_Started;
                    accountsHelper.HelperComplete += accountsHelper_HelperComplete;
                    accountsHelper.DisplayMessage += accountsHelper_DisplayMessage;
                    accountsHelper.IncrementProgress += accountsHelper_IncrementProgress;
                }
                if (contactsHelper == null)
                {
                    contactsHelper = new ContactsHelper(AccessToken, ConnectionString);
                    contactsHelper.CreateItemComplete += contactsHelper_CreateItemComplete;
                    contactsHelper.GetComplete += contactsHelper_GetComplete;
                    contactsHelper.GetItemComplete += contactsHelper_GetItemComplete;
                    contactsHelper.PatchComplete += contactsHelper_PatchComplete;
                    contactsHelper.Error += contactsHelper_Error;
                    contactsHelper.Started += contactsHelper_Started;
                    contactsHelper.HelperComplete += contactsHelper_HelperComplete;
                    contactsHelper.DisplayMessage += contactsHelper_DisplayMessage;
                    contactsHelper.IncrementProgress += contactsHelper_IncrementProgress;
                }
                if (dealsHelper == null)
                {
                    dealsHelper = new DealsHelper(AccessToken, ConnectionString);
                    dealsHelper.CreateItemComplete += dealsHelper_CreateItemComplete;
                    dealsHelper.GetComplete += dealsHelper_GetComplete;
                    dealsHelper.GetItemComplete += dealsHelper_GetItemComplete;
                    dealsHelper.PatchComplete += dealsHelper_PatchComplete;
                    dealsHelper.Error += dealsHelper_Error;
                    dealsHelper.Started += dealsHelper_Started;
                    dealsHelper.HelperComplete += dealsHelper_HelperComplete;
                    dealsHelper.DisplayMessage += dealsHelper_DisplayMessage;
                    dealsHelper.IncrementProgress += dealsHelper_IncrementProgress;
                }
                if (activitiesHelper == null)
                {
                    activitiesHelper = new ActivitiesHelper(AccessToken, ConnectionString);
                    activitiesHelper.CreateItemComplete += activitiesHelper_CreateItemComplete;
                    activitiesHelper.GetComplete += activitiesHelper_GetComplete;
                    activitiesHelper.GetItemComplete += activitiesHelper_GetItemComplete;
                    activitiesHelper.PatchComplete += activitiesHelper_PatchComplete;
                    activitiesHelper.Error += activitiesHelper_Error;
                    activitiesHelper.Started += activitiesHelper_Started;
                    activitiesHelper.HelperComplete += activitiesHelper_HelperComplete;
                    activitiesHelper.DisplayMessage += activitiesHelper_DisplayMessage;
                }
            }
        }

        private void LoadSettings()
        {
            using (Log.VerboseCall())
            {
                try
                {
                    configure1.txtDomain.Text = Settings.Default.Domain;
                    configure1.txtClientID.Text = Settings.Default.ClientID;
                    configure1.txtRedirectURI.Text = Settings.Default.RedirectURI;
                    connect1.txtUserName.Text = Settings.Default.DBUser;
                    connect1.txtDatabase.Text = Settings.Default.BCMDBName;
                    connect1.cboAuthentication.Text = Settings.Default.DBAuthMode;
                    connect1.cboLoginOptions.Text = Settings.Default.LoginMode;
                    connect1.txtServer.Text = Settings.Default.DBInstance;                
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(entryAssembly.Location);
                    lblVersion.Text = String.Format("Version {0}", fvi.FileVersion);
#if DEBUG
//connect1.txtDatabase.Text = "AQUARIASERVERDB2011";
//connect1.txtServer.Text = ".\\SQLEXPRESS";
#endif
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    MessageBox.Show("Error in BCM_Migration_Tool.MigrationAssistant.LoadSettings()", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task MigrateAccounts()
        {
            using (Log.VerboseCall())
            {
                //Initialize class earlier so we can get the custom fields from the templates for mappings
                if (accountsHelper == null)
                {
                    accountsHelper = new AccountsHelper(AccessToken, ConnectionString);
                    accountsHelper.CreateItemComplete += accountsHelper_CreateItemComplete;
                    accountsHelper.GetItemComplete += accountsHelper_GetItemComplete;
                    accountsHelper.GetComplete += accountsHelper_GetComplete;
                    accountsHelper.PatchComplete += accountsHelper_PatchComplete;
                    accountsHelper.Error += accountsHelper_Error;
                    accountsHelper.Started += accountsHelper_Started;
                    accountsHelper.HelperComplete += accountsHelper_HelperComplete;
                }
                accountsHelper.TestMode = chkTestMode.Checked;
                accountsHelper.TestingMaximum = (int)numericUpDownMaxRecords.Value;
                accountsHelper.RESTRetryMax = (int)numericUpDownRetryMax.Value;
                accountsHelper.RESTRetryDelay = (int)numericUpDownRetryDelay.Value * 1000;
                accountsHelper.LogRecordNames = configure1.chkLogRecordNames.Checked;
                accountsHelper.FullRESTLogging = configure1.chkFullRESTLogging.Checked;
                activitiesHelper.RESTRetryMax = (int)numericUpDownRetryMax.Value;
                activitiesHelper.RESTRetryDelay = (int)numericUpDownRetryDelay.Value * 1000;
                activitiesHelper.LogRecordNames = configure1.chkLogRecordNames.Checked;
                activitiesHelper.FullRESTLogging = configure1.chkFullRESTLogging.Checked;

                migrate1.UpdateStatus("Migrating Accounts...");

                Debug.WriteLine("Calling GetBCMAccounts");
                migrate1.progressBar1.PerformStep();

                //Load BCM Accounts
                await accountsHelper.GetBCMAccounts();

                migrate1.progressBar1.PerformStep();
                Debug.WriteLine("GetBCMAccounts returned; calling GetOCMCompanies");

                //Load existing OCM Company contacts
                await accountsHelper.GetOCMCompanies();

                migrate1.progressBar1.PerformStep();

                if (!chkGetOnly.Checked)
                {
                    Debug.WriteLine("GetOCMCompanies returned; calling CreateCompanies");
                    
                    //Load import Accounts as OCM Companies
                    await accountsHelper.CreateCompanies();
                    
                    Debug.WriteLine("CreateCompanies returned; calling CreateCompanyActivities");

                    //Import activities from Accounts
                    await activitiesHelper.CreateCompanyActivities();

                    Debug.WriteLine("CreateCompanyActivities returned");
                }
                else
                {
                    Debug.WriteLine("GetOCMCompanies returned");
                }
            }
        }
        private async Task MigrateBusinessContacts()
        {
            using (Log.VerboseCall())
            {
                try
                {
                    if (contactsHelper == null)
                    {
                        contactsHelper = new ContactsHelper(AccessToken, ConnectionString);
                        contactsHelper.CreateItemComplete += contactsHelper_CreateItemComplete;
                        contactsHelper.GetComplete += contactsHelper_GetComplete;
                        contactsHelper.GetItemComplete += contactsHelper_GetItemComplete;
                        contactsHelper.PatchComplete += contactsHelper_PatchComplete;
                        contactsHelper.Error += contactsHelper_Error;
                        contactsHelper.Started += contactsHelper_Started;
                        contactsHelper.HelperComplete += contactsHelper_HelperComplete;
                    }
                    contactsHelper.TestMode = chkTestMode.Checked;
                    contactsHelper.TestingMaximum = (int)numericUpDownMaxRecords.Value;
                    contactsHelper.RESTRetryMax = (int)numericUpDownRetryMax.Value;
                    contactsHelper.RESTRetryDelay = (int)numericUpDownRetryDelay.Value * 1000;
                    contactsHelper.LogRecordNames = configure1.chkLogRecordNames.Checked;
                    contactsHelper.FullRESTLogging = configure1.chkFullRESTLogging.Checked;

                    activitiesHelper.RESTRetryMax = (int)numericUpDownRetryMax.Value;
                    activitiesHelper.RESTRetryDelay = (int)numericUpDownRetryDelay.Value * 1000;
                    activitiesHelper.LogRecordNames = configure1.chkLogRecordNames.Checked;
                    activitiesHelper.FullRESTLogging = configure1.chkFullRESTLogging.Checked;

                    migrate1.UpdateStatus("Migrating Contacts...");

                    Debug.WriteLine("Calling GetBCMContacts");
                    migrate1.progressBar1.Value = 0;
                    migrate1.progressBar1.PerformStep();

                    //Load BCM Contacts from database
                    await contactsHelper.GetBCMContacts();

                    migrate1.progressBar1.PerformStep();
                    Debug.WriteLine("GetBCMContacts returned; calling GetOCMContacts");

                    //Load existing OCM Contacts
                    await contactsHelper.GetOCMContacts();

                    migrate1.progressBar1.PerformStep();
                    if (!chkGetOnly.Checked)
                    {
                        Debug.WriteLine("GetOCMContacts returned; calling CreateContacts");
                        
                        //Import Contacts
                        await contactsHelper.CreateContacts();

                        Debug.WriteLine("CreateContacts returned; calling FindPeople");
                        migrate1.progressBar1.Value = 0;
                        migrate1.progressBar1.Maximum = 100;
                        migrate1.progressBar1.PerformStep();

                        //Load Contacts again but via EWS to get ItemLinkID values for use in linking Contacts to Companies
                        await contactsHelper.FindPeople(); //TESTED New FindPeople call that retrieves ItemLinkID as well!
                        
                        Debug.WriteLine("FindPeople returned; calling CreateContactActivities");
                        migrate1.progressBar1.Value = 0;
                        migrate1.progressBar1.PerformStep();

                        //Create activities
                        await activitiesHelper.CreateContactActivities();

                        migrate1.progressBar1.Value = 0;
                        migrate1.progressBar1.PerformStep();
                        Debug.WriteLine("CreateContactActivities returned; calling CreateCompanyLinks");

                        //Link Contacts to Companies
                        await contactsHelper.CreateCompanyLinks(); //HIGH Use progress bar during linking

                        Debug.WriteLine("CreateCompanyLinks returned");
                    }
                    else
                    {
                        Debug.WriteLine("GetOCMContacts returned; calling FindPeople");
                        //Must run to get ItemLinkID even if not creating Contacts; needed for creating links on Deals if we are importing those
                        await contactsHelper.FindPeople();
                        Debug.WriteLine("FindPeople returned");
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
        private async Task MigrateOpportunities()
        {
            using (Log.VerboseCall())
            {
                if (dealsHelper == null)
                {
                    dealsHelper = new DealsHelper(AccessToken, ConnectionString);
                    dealsHelper.CreateItemComplete += dealsHelper_CreateItemComplete;
                    dealsHelper.GetComplete += dealsHelper_GetComplete;
                    dealsHelper.GetItemComplete += dealsHelper_GetItemComplete;
                    dealsHelper.PatchComplete += dealsHelper_PatchComplete;
                    dealsHelper.Error += dealsHelper_Error;
                    dealsHelper.Started += dealsHelper_Started;
                    dealsHelper.HelperComplete += dealsHelper_HelperComplete;
                }
                dealsHelper.TestMode = chkTestMode.Checked;
                dealsHelper.TestingMaximum = (int) numericUpDownMaxRecords.Value;
                dealsHelper.RESTRetryMax = (int)numericUpDownRetryMax.Value;
                dealsHelper.RESTRetryDelay = (int)numericUpDownRetryDelay.Value * 1000;
                dealsHelper.LogRecordNames = configure1.chkLogRecordNames.Checked;
                dealsHelper.FullRESTLogging = configure1.chkFullRESTLogging.Checked;
                activitiesHelper.RESTRetryMax = (int)numericUpDownRetryMax.Value;
                activitiesHelper.RESTRetryDelay = (int)numericUpDownRetryDelay.Value * 1000;
                activitiesHelper.LogRecordNames = configure1.chkLogRecordNames.Checked;
                activitiesHelper.FullRESTLogging = configure1.chkFullRESTLogging.Checked;

                migrate1.progressBar1.Value = 0;
                migrate1.progressBar1.PerformStep();

                migrate1.UpdateStatus("Migrating Opportunities...");

                Debug.WriteLine("Calling GetBCMOpportunities");
                await dealsHelper.GetBCMOpportunities();
                migrate1.progressBar1.PerformStep();
                Debug.WriteLine("GetBCMOpportunities returned; calling GetOCMDeals");
                await dealsHelper.GetOCMDeals();
                migrate1.progressBar1.PerformStep();
                Debug.WriteLine("GetOCMDeals returned");

                if (!chkGetOnly.Checked)
                {
                    //if (chkUpdateTemplate.Checked)
                    //{

                    //}
                    Debug.WriteLine("Calling UpdateTemplate");
                    await dealsHelper.UpdateTemplate();
                    migrate1.progressBar1.Value = 0;
                    migrate1.progressBar1.PerformStep();
                    Debug.WriteLine("UpdateTemplate returned; calling CreateDeals");
                    await dealsHelper.CreateDeals();
                    migrate1.progressBar1.Value = 0;
                    migrate1.progressBar1.PerformStep();
                    Debug.WriteLine("CreateDeals returned; calling GetBCMOpportunities");
                    await dealsHelper.CreateDealLinks();
                    migrate1.progressBar1.Value = 0;
                    migrate1.progressBar1.PerformStep();
                    Debug.WriteLine("CreateDealLinks returned; calling CreateDealActivities");
                    await activitiesHelper.CreateDealActivities();
                    Debug.WriteLine("CreateDealActivities returned");
                }
            }
        }

        ///// <summary>
        ///// Use to refresh expired tokens. Technically this is not using the refresh token, as ADAL is fine with just using AcquireToken (in GetAccessToken)
        ///// </summary>
        //public bool GetAuthToken()//HIGH Not implemented
        //{
        //    using (Log.VerboseCall())
        //    {
        //        bool result = false;
        //        AuthenticationResult authenticationResult = MigrationHelper.GetAccessToken();

        //        if (authenticationResult != null)
        //        {
        //            Log.InfoFormat(
        //                "Access token retrieved: {0} (AccessTokenType: {1}; ExpiresOn: {2}; IdToken: {3}; RefreshToken: {4}; TenantID: {5}; User: {6}",
        //                authenticationResult.AccessToken, authenticationResult.AccessTokenType, authenticationResult.ExpiresOn, authenticationResult.IdToken,
        //                authenticationResult.RefreshToken, authenticationResult.TenantId, authenticationResult.UserInfo?.GivenName);
        //            AccessToken = authenticationResult.AccessToken;
        //            result = true;
        //            //Do we need to do this too?
        //            //await MigrationHelper.GetUserAsync(token.Result);

        //            if (MigrationHelper.CurrentUser != null)
        //            {
        //            }
        //        }
        //        else
        //        {
        //            Log.Error("Unable to retrieve access token!");
        //        }
        //        return result;
        //    }
        //}

        private void SaveSettings()
        {
            using (Log.VerboseCall())
            {
                try
                {
                    Settings.Default.RedirectURI = configure1.txtRedirectURI.Text;
                    Settings.Default.ClientID = configure1.txtClientID.Text;
                    Settings.Default.Domain = configure1.txtDomain.Text;
                    Settings.Default.DBUser = connect1.txtUserName.Text;
                    Settings.Default.BCMDBName = connect1.txtDatabase.Text;
                    Settings.Default.DBAuthMode = connect1.cboAuthentication.Text;
                    Settings.Default.LoginMode = connect1.cboLoginOptions.Text;
                    Settings.Default.DBInstance = connect1.txtServer.Text;
                    Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    MessageBox.Show("Error in BCM_Migration_Tool.MigrationAssistant.SaveSettings()", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void TestDBConnection()
        {
            using (Log.VerboseCall())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string connectionString = connect1.GetConnectionString();
                    using (var context = new MSSampleBusinessEntities())
                    {
                        //Test DB connection by getting Sharing setting
                        context.Database.Connection.ConnectionString = connectionString;
                        var sharing = context.OrgTables.ToList();
                        Cursor.Current = Cursors.Default;
                        Log.InfoFormat("Connected to DB: {0}", connectionString);
                        MessageBox.Show("Database connection successful!", "Connection Test Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connect1.lblConnectionString.Text = "DB connection string: " + connectionString;
                        connect1.lblLogin.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(ex.Message, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TestWhatever()
        {
            //using (var context = new MSSampleBusinessEntities())
            //{
            //    context.Database.Connection.ConnectionString = ConnectionString;

            //    var contacts = from mycontact in context.ContactFullViews where !mycontact.IsDeletedLocally && mycontact.Type == 1 select mycontact;
            //    var contacts2 = (from a in context.ContactFullViews join c in context.AccountsFullViews on a.ParentEntryID equals c.EntryGUID select new { CompanyName = c.FullName, Contact = a });
            //}
        }
#endregion
#region Control Events
        private void chkEnableTestMode_CheckedChanged(object sender, EventArgs e)
        {
            ToggleTestModeControls(configure1.chkEnableTestMode.Checked);
        }

        private async void cmdDeleteDeals_Click(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                if (String.IsNullOrEmpty(configure1.cboDeleteDealsOptions.Text))
                {
                    MessageBox.Show("Please select a delete mode from the dropdown.", "Invalid Option", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete the specified deals?", "Delete Deals?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                //string dealIDs = "";

                //dealIDs = Microsoft.VisualBasic.Interaction.InputBox("Enter semi-colon delimited list of deal IDs:",
                //    "Delete Deals");
                //if (String.IsNullOrEmpty(dealIDs))
                //    return;

                //await dealsHelper.DeleteDeals(dealIDs);
                int cnt = 0;
                DateTime createdOn = DateTime.MinValue;

                if (configure1.chkDealCreationDate.Checked)
                    createdOn = configure1.dateTimePicker1.Value;

                switch (configure1.cboDeleteDealsOptions.Text)
                {
                    case "All":
                        cnt = await dealsHelper.DeleteDeals(DealsHelper.DeleteDealsOptions.All, createdOn);
                        break;
                    case "Private":
                        cnt = await dealsHelper.DeleteDeals(DealsHelper.DeleteDealsOptions.Private, createdOn);
                        break;
                    case "Shared":
                        cnt = await dealsHelper.DeleteDeals(DealsHelper.DeleteDealsOptions.Shared, createdOn);
                        break;
                }

                Log.InfoFormat("Deleted {0} deals (Option: {1})", cnt, configure1.cboDeleteDealsOptions.Text);
                MessageBox.Show(String.Format("{0} deals were deleted.", cnt), "Delete Deals Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void connect1_AuthenticationChanged(object sender, ConnectEventArgs e)
        {
            //NOTE This isn't doing anything...yet
            using (Log.VerboseCall())
            {
                switch (e.AuthenticationType)
                {
                    case ConnectEventArgs.AuthenticationTypes.Windows:
                        break;
                    case ConnectEventArgs.AuthenticationTypes.SQL:
                        break;
                }
            }
        }
        private async void connect1_LoginClicked(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                if (String.IsNullOrEmpty(configure1.txtDomain.Text) || String.IsNullOrEmpty(configure1.txtClientID.Text) ||
                    String.IsNullOrEmpty(configure1.txtRedirectURI.Text))
                {
                    MessageBox.Show("Please fill in all application configuration details.", "Invalid Configuration",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);                
                    return;
                }

                SaveSettings();
                //Task<string> token = MigrationHelper.Login();                                
                //if (!String.IsNullOrEmpty(token.Result))
                //{                
                //    AccessToken = token.Result;
                //    //Debug.WriteLine("Access Token: " + AccessToken);
                //    connect1.Connected = true;
                //    //Prevent clicking Map or Migrate if connection details not configured
                //    navigationBar1.MapEnabled = true;
                //    //configure1.cmdDeleteDeals.Enabled = true;

                //    await MigrationHelper.GetUserAsync(token.Result);

                //    if (MigrationHelper.CurrentUser != null)
                //    {
                //        lblCurrentUser.Text = String.Format("Signed in as: {0}", MigrationHelper.CurrentUser.EmailAddress);
                //        lblCurrentUser.Visible = true;
                //    }
                //}

                AuthenticationResult authenticationResult = MigrationHelper.GetAccessToken(false);
                if (authenticationResult != null && !String.IsNullOrEmpty(authenticationResult.AccessToken))
                {
                    AccessToken = authenticationResult.AccessToken;
                    connect1.Connected = true;
                    //Prevent clicking Map or Migrate if connection details not configured
                    navigationBar1.MapEnabled = true;

                    await MigrationHelper.GetUserAsync(AccessToken);

                    if (MigrationHelper.CurrentUser != null)
                    {
                        lblCurrentUser.Text = String.Format("Signed in as: {0}", MigrationHelper.CurrentUser.EmailAddress);
                        lblCurrentUser.Visible = true;
                    }
                }
            }
        }

        private void connect1_TestDBConnectionClicked(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                TestDBConnection();
            }
        }

        private void lblWebSite_Click(object sender, EventArgs e)
        {
            using (Log.InfoCall())
            {
                Process.Start("http://www.rockinsoftware.rocks");
            }
        }

        private async void migrate1_StartClicked(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                try
                {
                    migrate1.cmdStart.Enabled = false;
                    navigationBar1.ConfigureEnabled = false;

                    Cursor.Current = Cursors.WaitCursor;

                    ToggleTestModeControls(false);

                    migrate1.progressBar1.PerformStep();
                    migrate1.progressBar1.PerformStep();
                    migrate1.UpdateText("Starting import...");
                    migrate1.UpdateStatus("Initializing...");
                    if (MigrationHelper.StartXRMSession(AccessToken) == false)
                    {
                        Log.Error("StartXRMSession returned false");                        
                        return;
                    }

                    Log.InfoFormat("AADInstance: {0}", Properties.Settings.Default.AADInstance);
                    Log.InfoFormat("BCMDBName: {0}", Properties.Settings.Default.BCMDBName);
                    Log.InfoFormat("BetaEndPoint: {0}", Properties.Settings.Default.BetaEndPoint);
                    Log.InfoFormat("ClientID: {0}", Properties.Settings.Default.ClientID);
                    Log.InfoFormat("DBAuthMode: {0}", Properties.Settings.Default.DBAuthMode);
                    Log.InfoFormat("DBInstance: {0}", Properties.Settings.Default.DBInstance);
                    Log.InfoFormat("DBUser: {0}", Properties.Settings.Default.DBUser);
                    Log.InfoFormat("Domain: {0}", Properties.Settings.Default.Domain);
                    Log.InfoFormat("EWSEndPoint: {0}", Properties.Settings.Default.EWSEndPoint);
                    Log.InfoFormat("LoginMode: {0}", Properties.Settings.Default.LoginMode);
                    Log.InfoFormat("RedirectURI: {0}", Properties.Settings.Default.RedirectURI);
                    Log.InfoFormat("V2EndPoint: {0}", Properties.Settings.Default.V2EndPoint);

                    migrate1.progressBar1.PerformStep();
                    migrate1.UpdateText("OCM system initialized...");

                    //Get sharing state:
                    //Set Shared = True (if DBType = 1), False (I DBType=0) SELECT [DbType] FROM [dbo].[OrgTable]
                    //HIGH Default sharing to true for now; ignore DB setting = Convert.ToBoolean(sharing[0].DbType);
                    MigrationHelper.IsSharingEnabled = true;

                    if (chkTestMode.Checked == false)
                    {
                        Debug.WriteLine("Calling MigrateAccounts");
                        await MigrateAccounts();
                        Debug.WriteLine("MigrateAccounts returned; calling MigrateBusinessContacts");
                        await MigrateBusinessContacts();
                        Debug.WriteLine("MigrateBusinessContacts returned; calling MigrateOpportunities");
                        await MigrateOpportunities();
                        Debug.WriteLine("MigrateAccounts returned");
                        migrate1.UpdateText("***********MIGRATION COMPLETE***********");
                        migrate1.UpdateText(String.Format("Companies created: {0}", accountsHelper.NumberCreated));
                        migrate1.UpdateText(String.Format("Companies updated: {0}", accountsHelper.NumberUpdated));
                        migrate1.UpdateText(String.Format("Company import errors: {0}", accountsHelper.NumberOfErrors));
                        migrate1.UpdateText(String.Format("Contacts created: {0}", contactsHelper.NumberCreated));
                        migrate1.UpdateText(String.Format("Contacts updated: {0}", contactsHelper.NumberUpdated));
                        migrate1.UpdateText(String.Format("Contact import errors: {0}", contactsHelper.NumberOfErrors));
                        migrate1.UpdateText(String.Format("Deals created: {0}", dealsHelper.NumberCreated));
                        migrate1.UpdateText(String.Format("Deals updated: {0}", dealsHelper.NumberUpdated));
                        migrate1.UpdateText(String.Format("Deal import errors: {0}", dealsHelper.NumberOfErrors));
                    }
                    else
                    {
                        if (chkLBDebugMode.CheckedItems.Contains("Accounts"))
                        {
                            Debug.WriteLine("Calling MigrateAccounts");
                            await MigrateAccounts();
                            Debug.WriteLine("MigrateAccounts returned");
                        }
                        if (chkLBDebugMode.CheckedItems.Contains("Contacts"))
                        {
                            Debug.WriteLine("Calling MigrateBusinessContacts");
                            await MigrateBusinessContacts();
                            Debug.WriteLine("MigrateBusinessContacts returned");
                        }
                        if (chkLBDebugMode.CheckedItems.Contains("Opportunities"))
                        {
                            Debug.WriteLine("Calling MigrateOpportunities");
                            await MigrateOpportunities();
                            Debug.WriteLine("MigrateOpportunities returned");
                        }
                        if (chkLBDebugMode.CheckedItems.Contains("Deal Stages"))
                        {
                            await dealsHelper.UpdateTemplate();
                        }

                        migrate1.UpdateText("***********MIGRATION COMPLETE***********");
                        migrate1.UpdateText(String.Format("Companies created: {0}", accountsHelper.NumberCreated));
                        migrate1.UpdateText(String.Format("Companies updated: {0}", accountsHelper.NumberUpdated));
                        migrate1.UpdateText(String.Format("Company import errors: {0}", accountsHelper.NumberOfErrors));
                        migrate1.UpdateText(String.Format("Contacts created: {0}", contactsHelper.NumberCreated));
                        migrate1.UpdateText(String.Format("Contacts updated: {0}", contactsHelper.NumberUpdated));
                        migrate1.UpdateText(String.Format("Contact import errors: {0}", contactsHelper.NumberOfErrors));
                        migrate1.UpdateText(String.Format("Deals created: {0}", dealsHelper.NumberCreated));
                        migrate1.UpdateText(String.Format("Deals updated: {0}", dealsHelper.NumberUpdated));
                        migrate1.UpdateText(String.Format("Deal import errors: {0}", dealsHelper.NumberOfErrors));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                finally
                {
                    migrate1.cmdStart.Enabled = true;
                    navigationBar1.ConfigureEnabled = true;

                    Log.InfoFormat("Companies created: {0}", accountsHelper.NumberCreated);
                    Log.InfoFormat("Companies updated: {0}", accountsHelper.NumberUpdated);
                    Log.InfoFormat("Company import errors: {0}", accountsHelper.NumberOfErrors);
                    Log.InfoFormat("Contacts created: {0}", contactsHelper.NumberCreated);
                    Log.InfoFormat("Contacts updated: {0}", contactsHelper.NumberUpdated);
                    Log.InfoFormat("Contact import errors: {0}", contactsHelper.NumberOfErrors);
                    Log.InfoFormat("Deals created: {0}", dealsHelper.NumberCreated);
                    Log.InfoFormat("Deals updated: {0}", dealsHelper.NumberUpdated);
                    Log.InfoFormat("Deal import errors: {0}", dealsHelper.NumberOfErrors);

                    migrate1.progressBar1.Value = 0;
                    migrate1.UpdateStatus("DONE!");
                    Cursor.Current = Cursors.Default;

                    ToggleTestModeControls(configure1.chkEnableTestMode.Checked);

                }
            }
        }

        private void ToggleTestModeControls(bool enable)
        {
            numericUpDownMaxRecords.Visible = enable;
            chkGetOnly.Visible = enable;
            chkLBDebugMode.Visible = enable;
            chkTestMode.Visible = enable;
            lblMaxRecords.Visible = enable;
            lblRESTRetries.Visible = enable;
            lblRetryDelay.Visible = enable;
            numericUpDownMaxRecords.Visible = enable;
            numericUpDownRetryDelay.Visible = enable;
            numericUpDownRetryMax.Visible = enable;
        }

        private async void migrate1_StopClicked(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                //BUG Cancelling doesn't "cancel" anything!
                accountsHelper.Cancel();
                contactsHelper.Cancel();
                dealsHelper.Cancel();
                activitiesHelper.Cancel();

                ToggleTestModeControls(configure1.chkEnableTestMode.Checked);
                migrate1.cmdStart.Enabled = true;
                navigationBar1.ConfigureEnabled = true;
            }
        }

        private void navigationBar1_ConfigClicked(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                tabControl1.SelectTab(tabPageConfig);
            }
        }

        private void navigationBar1_ConnectClicked(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                tabControl1.SelectTab(tabPageConnect);
            }
        }

        private async void navigationBar1_MapClicked(object sender, EventArgs e)
        {
            using (Log.VerboseCall())
            {
                navigationBar1.MigrateEnabled = true;
                configure1.cmdDeleteDeals.Enabled = true;
                ConnectionString = connect1.GetConnectionString();
                tabControl1.SelectTab(tabPageMap);
                if (!MappingsInitialized)
                {
                    InitializeHelpers();

                    map1.tabControl1.SelectTab(3);
                    map1.UpdateText("Please wait while your Outlook Customer Manager templates are retrieved...");
                    await accountsHelper.RunGetTemplateAsync();
                    await contactsHelper.RunGetTemplateAsync();
                    await dealsHelper.RunGetTemplateAsync();
                    //await opp
                    map1.tabControl1.SelectTab(0);
                    map1.ConnectionString = ConnectionString;
                    map1.InitializeFieldMappings();
                    MappingsInitialized = true;
                }
            }
        }

        private void navigationBar1_MigrateClicked(object sender, EventArgs e)
        {
            //TESTED Log mappings

            using (Log.VerboseCall())
            {
                try
                {
                    if (FieldMappings.BCMAccountFields.BCMFields != null)
                    {
                        Log.InfoFormat("Mapped BCMAccountFields: {0}",
                            FieldMappings.BCMAccountFields.BCMFields.MappedFields.Count);
                        foreach (var field in FieldMappings.BCMAccountFields.BCMFields.MappedFields)
                        {
                            Log.InfoFormat("Name: {0}; Type: {1}; IsCustom: {3}; FieldMappingType: {4}; Mapped: {5}",
                                field.Name, field.FieldType, field.Id, field.IsCustom, field.FieldMappingType,
                                field.OCMFieldMapping == null ? false : true);
                            if (field.OCMFieldMapping != null)
                            {
                                Log.InfoFormat(
                                    "Mapping: {0}; Type: {1}; IsCustom: {3}; FieldMappingType: {4}; OCMDataSetType: {5}; PropertyID: {6}",
                                    field.OCMFieldMapping.Name, field.OCMFieldMapping.FieldType,
                                    field.OCMFieldMapping.Id, field.OCMFieldMapping.IsCustom,
                                    field.OCMFieldMapping.FieldMappingType, field.OCMFieldMapping.OCMDataSetType,
                                    field.OCMFieldMapping.PropertyID);
                            }
                        }
                    }

                    if (FieldMappings.BCMBusinessContactFields.BCMFields != null)
                    {
                        Log.InfoFormat("Mapped BCMBusinessContactFields: {0}", FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields.Count);
                        foreach (var field in FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields)
                        {
                            Log.InfoFormat("Name: {0}; Type: {1}; IsCustom: {3}; FieldMappingType: {4}; Mapped: {5}", field.Name, field.FieldType, field.Id, field.IsCustom, field.FieldMappingType, field.OCMFieldMapping == null ? false : true);
                            if (field.OCMFieldMapping != null)
                            {
                                Log.InfoFormat("Mapping: {0}; Type: {1}; IsCustom: {3}; FieldMappingType: {4}; OCMDataSetType: {5}; PropertyID: {6}", field.OCMFieldMapping.Name, field.OCMFieldMapping.FieldType, field.OCMFieldMapping.Id, field.OCMFieldMapping.IsCustom, field.OCMFieldMapping.FieldMappingType, field.OCMFieldMapping.OCMDataSetType, field.OCMFieldMapping.PropertyID);
                            }
                        }
                    }

                    if (FieldMappings.BCMOpportunityFields.BCMFields != null)
                    {
                        Log.InfoFormat("Mapped BCMOpportunityFields: {0}", FieldMappings.BCMOpportunityFields.BCMFields.MappedFields.Count);
                        foreach (var field in FieldMappings.BCMOpportunityFields.BCMFields?.MappedFields)
                        {
                            Log.InfoFormat("Name: {0}; Type: {1}; IsCustom: {3}; FieldMappingType: {4}; Mapped: {5}", field.Name, field.FieldType, field.Id, field.IsCustom, field.FieldMappingType, field.OCMFieldMapping == null ? false : true);
                            if (field.OCMFieldMapping != null)
                            {
                                Log.InfoFormat("Mapping: {0}; Type: {1}; IsCustom: {3}; FieldMappingType: {4}; OCMDataSetType: {5}; PropertyID: {6}", field.OCMFieldMapping.Name, field.OCMFieldMapping.FieldType, field.OCMFieldMapping.Id, field.OCMFieldMapping.IsCustom, field.OCMFieldMapping.FieldMappingType, field.OCMFieldMapping.OCMDataSetType, field.OCMFieldMapping.PropertyID);
                            }
                        }
                    }                                   
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }

                if (map1.ValidateMappings() == false)
                {
                    //FieldMappings.BCMDataSetTypes bcmDataSetType;
                    //string invalidMappings;
                    Log.Warn("Invalid mappings detected");
                    MessageBox.Show("You have one or more invalid mappings. Please ensure both source and destination fields are set for every custom mapping, or remove the invalid mappings.", "Invalid Mappings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    navigationBar1.NavigationBarState = NavigationBar.NavigationBarStates.Map;
                    return;
                }

                if (MigrateClicked == false)
                {
                    migrate1.txtLog.Text = map1.txtLog.Text;
                    MigrateClicked = true;
                }
                ConnectionString = connect1.GetConnectionString();
                tabControl1.SelectTab(tabPageMigrate);
            }
        }
#endregion
#region Object Events
        private void accountsHelper_CreateItemComplete(object sender, HelperEventArgs e)
        {
            if (migrate1.progressBar1.Value != migrate1.progressBar1.Maximum)
                migrate1.progressBar1.PerformStep();
            migrate1.UpdateText(e.Message);
        }

        private void accountsHelper_DisplayMessage(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }

        private void accountsHelper_Error(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }
        private void accountsHelper_GetComplete(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }

        private void accountsHelper_GetItemComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void accountsHelper_HelperComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }

        private void accountsHelper_IncrementProgress(object sender, HelperEventArgs e)
        {
            migrate1.progressBar1.PerformStep();
        }
        private void accountsHelper_PatchComplete(object sender, HelperEventArgs e)
        {
            if (migrate1.progressBar1.Value != migrate1.progressBar1.Maximum)
                migrate1.progressBar1.PerformStep();
            migrate1.UpdateText(e.Message);
        }
        private void accountsHelper_Started(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    migrate1.progressBar1.Value = 0;
                    migrate1.progressBar1.Maximum = contactsHelper.NumberToProcess;
                    if (migrate1.progressBar1.Maximum == 0)
                        migrate1.progressBar1.Maximum = 100;
                    break;
            }
        }
        private void activitiesHelper_CreateItemComplete(object sender, HelperEventArgs e)
        {
            if (migrate1.progressBar1.Value != migrate1.progressBar1.Maximum)
                migrate1.progressBar1.PerformStep();
            migrate1.UpdateText(e.Message);
        }

        private void activitiesHelper_DisplayMessage(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }
        private void activitiesHelper_Error(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void activitiesHelper_GetComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void activitiesHelper_GetItemComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void activitiesHelper_HelperComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void activitiesHelper_PatchComplete(object sender, HelperEventArgs e)
        {
            if (migrate1.progressBar1.Value != migrate1.progressBar1.Maximum)
                migrate1.progressBar1.PerformStep();
            migrate1.UpdateText(e.Message);
        }
        private void activitiesHelper_Started(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void contactsHelper_CreateItemComplete(object sender, HelperEventArgs e)
        {
            if (migrate1.progressBar1.Value != migrate1.progressBar1.Maximum)
                migrate1.progressBar1.PerformStep();
            migrate1.UpdateText(e.Message);
        }

        private void contactsHelper_DisplayMessage(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }
        private void contactsHelper_Error(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }
        private void contactsHelper_GetComplete(object sender, HelperEventArgs e)
        {
            //if (!e.Error)
            //    FieldMappings.OCMBusinessContactFields.ContactTemplate = contactsHelper.ContactTemplate;
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }

        private void contactsHelper_GetItemComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void contactsHelper_HelperComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void contactsHelper_IncrementProgress(object sender, HelperEventArgs e)
        {
            migrate1.progressBar1.PerformStep();
        }
        private void contactsHelper_PatchComplete(object sender, HelperEventArgs e)
        {
            if (migrate1.progressBar1.Value != migrate1.progressBar1.Maximum)
                migrate1.progressBar1.PerformStep();
            migrate1.UpdateText(e.Message);
        }
        private void contactsHelper_Started(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    migrate1.progressBar1.Value = 0;
                    migrate1.progressBar1.Maximum = contactsHelper.NumberToProcess;
                    if (migrate1.progressBar1.Maximum == 0)
                        migrate1.progressBar1.Maximum = 100;
                    break;
            }
        }
        private void dealsHelper_CreateItemComplete(object sender, HelperEventArgs e)
        {
            if (migrate1.progressBar1.Value != migrate1.progressBar1.Maximum)
                migrate1.progressBar1.PerformStep();
            migrate1.UpdateText(e.Message);
        }

        private void dealsHelper_DisplayMessage(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }

        private void dealsHelper_Error(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }
        private void dealsHelper_GetComplete(object sender, HelperEventArgs e)
        {
            //if (!e.Error)
            //    FieldMappings.OCMDealFields.DealTemplate = dealsHelper.DealTemplate;
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    break;
            }
        }

        private void dealsHelper_GetItemComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }
        private void dealsHelper_HelperComplete(object sender, HelperEventArgs e)
        {
            migrate1.UpdateText(e.Message);
        }

        private void dealsHelper_IncrementProgress(object sender, HelperEventArgs e)
        {
            migrate1.progressBar1.PerformStep();
        }

        private void dealsHelper_PatchComplete(object sender, HelperEventArgs e)
        {
            if (migrate1.progressBar1.Value != migrate1.progressBar1.Maximum)
                migrate1.progressBar1.PerformStep();
            migrate1.UpdateText(e.Message);
        }
        private void dealsHelper_Started(object sender, HelperEventArgs e)
        {
            switch (e.ProcessingMode)
            {
                case HelperEventArgs.ProcessingModes.Mapping:
                    map1.UpdateText(e.Message);
                    break;
                case HelperEventArgs.ProcessingModes.Migrating:
                    migrate1.UpdateText(e.Message);
                    migrate1.progressBar1.Value = 0;
                    migrate1.progressBar1.Maximum = dealsHelper.NumberToProcess;
                    if (migrate1.progressBar1.Maximum == 0)
                        migrate1.progressBar1.Maximum = 100;
                    break;
            }
        }
        #endregion

        private void lblVersion_Click(object sender, EventArgs e)
        {

        }
    }
}
