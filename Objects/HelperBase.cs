using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BCM_Migration_Tool.Objects
{
    internal class HelperBase
    {
        #region Fields
        internal HttpClient _httpClient = new HttpClient();

        internal enum ImportModes
        {
            Create,
            Update,
            UpdatePreviouslyImportedItem
            //UpdateWithActivities
        }

        internal enum RequestDataFormats
        {
            None,
            JSON,
            XML
        }
        internal enum RequestDataTypes
        {
            Activities,
            Companies,
            Contacts,
            Deals,
            EWS,
            Links,
            Templates
        }
        internal enum UpdateKeyTypes
        {
            BCMID,
            EmailAddress,
            Name
        }
        #endregion
        #region Constructors
        internal HelperBase()
        {
            
        }
        internal HelperBase(string accessToken, string connectionString)
        {
            AccessToken = accessToken;
            ConnectionString = connectionString;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //x-AnchorMailbox
            //NOTE: For optimal performance when using the new Outlook REST endpoint, add an x-AnchorMailbox header for every request and set it to the user's email address. For example: x-AnchorMailbox:john@contoso.com
            _httpClient.DefaultRequestHeaders.Add("X-AnchorMailbox", MigrationHelper.CurrentUser.EmailAddress);
        }
        #endregion
        #region Events
        internal event EventHandler<HelperEventArgs> CreateItemComplete;
        internal event EventHandler<HelperEventArgs> DisplayMessage;
        internal event EventHandler<HelperEventArgs> Error;        
        internal event EventHandler<HelperEventArgs> GetComplete;
        internal event EventHandler<HelperEventArgs> GetItemComplete;
        internal event EventHandler<HelperEventArgs> HelperComplete;
        internal event EventHandler<HelperEventArgs> IncrementProgress;
        internal event EventHandler<HelperEventArgs> PatchComplete;
        internal event EventHandler<HelperEventArgs> Started;
        protected virtual void OnCreateItemComplete(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = CreateItemComplete;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void OnDisplayMessage(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = DisplayMessage;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void OnError(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = Error;
            if (handler != null)
            {
                //System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
                handler(sender, e);
            }
        }
        protected virtual void OnGetComplete(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = GetComplete;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void OnGetItemComplete(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = GetItemComplete;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void OnHelperComplete(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = HelperComplete;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void OnIncrementProgress(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = IncrementProgress;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void OnPatchComplete(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = PatchComplete;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnStart(object sender, HelperEventArgs e)
        {
            EventHandler<HelperEventArgs> handler = Started;
            if (handler != null)
                handler(sender, e);
        }
        #endregion
        #region Properties
        protected internal string AccessToken { get; private set; }
        internal bool Cancelled { get; set; }
        internal string ConnectionString { get; private set; }
        internal int NumberCreated { get; set; }
        internal int NumberOfErrors { get; set; }
        internal int NumberToProcess { get; set; }
        internal int NumberUpdated { get; set; }
        internal int TestingMaximum { get; set; }
        internal bool TestMode { get; set; }
        #endregion
        #region Methods
        internal void Cancel()
        {
            Cancelled = true;
        }

        internal void PrepareRequest(RequestDataTypes requestDataType, RequestDataFormats requestDataFormat)
        {
            //TODO Cleanup enum params - some are not used

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            //NOTE: For optimal performance when using the new Outlook REST endpoint, add an x-AnchorMailbox header for every request and set it to the user's email address. For example: x-AnchorMailbox:john@contoso.com
            _httpClient.DefaultRequestHeaders.Add("X-AnchorMailbox", MigrationHelper.CurrentUser.EmailAddress);

            switch (requestDataType)
            {
                case RequestDataTypes.Activities:
                    _httpClient.DefaultRequestHeaders.Add("Prefer", "exchange.behavior=\"SocialFabricInternal\"");
                    break;
                case RequestDataTypes.Companies:
                    break;
                case RequestDataTypes.Contacts:
                    break;
                case RequestDataTypes.Deals:
                    //REVIEW Needed for Deals, but sample Fiddler requests had this header set for Orgs/Accounts and Contacts
                    _httpClient.DefaultRequestHeaders.Add("Prefer", "exchange.behavior=\"SocialFabricInternal\"");
                    break;
                case RequestDataTypes.Templates:
                    break;
                case RequestDataTypes.Links:
                    break;
            }

            switch (requestDataFormat)
            {
                case RequestDataFormats.JSON:
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    break;
                case RequestDataFormats.XML://NOT USED
                    break;
            }
        }
        #endregion
    }
    public class HelperEventArgs : EventArgs
    {
        #region Fields
        private string msg;
        public enum EventTypes
        {
            Status,
            Error,
            FatalError
        }

        public enum ProcessingModes
        {
            Mapping,
            Migrating
        }
        #endregion
        #region Constructors
        public HelperEventArgs(string s, bool error)
        {
            Error = error;
            msg = s;
            ProcessingMode = ProcessingModes.Migrating;
        }
        public HelperEventArgs(string s, EventTypes eventType)
        {
            if (eventType != EventTypes.Status)
                Error = true;
            msg = s;
            EventType = eventType;
            ProcessingMode = ProcessingModes.Migrating;
        }
        public HelperEventArgs(string s, EventTypes eventType, Exception e)
        {
            Error = true;
            msg = s;
            EventType = eventType;
            ErrorException = e;
            ProcessingMode = ProcessingModes.Migrating;
        }
        public HelperEventArgs(string s, EventTypes eventType, ProcessingModes processingMode)
        {
            Error = true;
            msg = s;
            EventType = eventType;            
            ProcessingMode = processingMode;
        }
        public HelperEventArgs(string s, EventTypes eventType, ProcessingModes processingMode, Exception e)
        {
            Error = true;
            msg = s;
            EventType = eventType;
            ErrorException = e;
            ProcessingMode = processingMode;
        }
        #endregion
        #region Properties
        public bool Error { get; set; }
        public Exception ErrorException { get; private set; }
        public EventTypes EventType { get; set; }
        public string Message
        {
            get { return msg; }
        }
        public ProcessingModes ProcessingMode { get; set; }

        #endregion
    }
}
