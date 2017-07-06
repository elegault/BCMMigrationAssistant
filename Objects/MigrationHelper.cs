using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BCM_Migration_Tool.Properties;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TracerX;
using Task = System.Threading.Tasks.Task;

namespace BCM_Migration_Tool.Objects
{
    static class MigrationHelper
    {
        private static bool _isSharingEnabled = true;
        private static readonly Logger Log = Logger.GetLogger("MigrationHelper");
        #region Properties        
        internal static string CompaniesFolderID { get; set; }
        internal static string ContactsFolderID { get; set; }
        internal static GraphUser CurrentUser { get; set; }
        internal static string CurrentUserID { get; set; }
        internal static string DealsFolderID { get; set; }
        internal static string GroupMailboxID { get; set; }
        internal static bool IsSharingEnabled
        {        
            //HIGH Turn sharing on by default (for now)
            get { return _isSharingEnabled; }
            set { _isSharingEnabled = value; }
        } 
        internal static StartXRMSessionResponse XRMSession { get; set; }
        /// <summary>
        /// The Azure AD instance where you domain is hosted
        /// </summary>
        public static string AADInstance
        {
            //https://login.microsoftonline.com/common
            get { return BCM_Migration_Tool.Properties.Settings.Default.AADInstance; }
        }
        /// <summary>
        /// The authority for authentication; combining the AADInstance and the domain.
        /// </summary>
        public static string Authority
        {
            //e.g. https://login.microsoftonline.com/common/yourdomain.onmicrosoft.com
            get { return string.Format("{0}/{1}/", AADInstance, Domain); }
        }
        /// <summary>
        /// The client Id of your native Azure AD application
        /// </summary>
        public static string ClientId
        {
            get { return BCM_Migration_Tool.Properties.Settings.Default.ClientID; }
        }
        /// <summary>
        /// The Office 365 domain (e.g. contoso.microsoft.com)
        /// </summary>
        public static string Domain
        {
            get { return BCM_Migration_Tool.Properties.Settings.Default.Domain; }
        }
        /// <summary>
        /// The resource identifier for the Microsoft Graph
        /// </summary>
        public static string GraphResource
        {
            get { return "https://outlook.office365.com/"; }
        }
        /// <summary>
        /// The redirect URI specified in the Azure AD application
        /// </summary>
        public static Uri RedirectUri
        {
            //Was urn:ietf:wg:oauth:2.0:oob
            get { return new Uri(Properties.Settings.Default.RedirectURI); }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Get an access token for the Microsoft Graph using ADAL
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken(string loginMode)
        {
            // Create the authentication context (ADAL)
            try
            {
                //Authority e.g.: https://login.microsoftonline.com/common/mydomain.onmicrosoft.com/
                var authenticationContext = new AuthenticationContext(Authority);

                PromptBehavior promptBehavior;

                switch (loginMode)
                {
                    case "Always":
                        promptBehavior = PromptBehavior.Always;
                        break;
                    case "Automatic":
                        promptBehavior = PromptBehavior.Auto;
                        break;
                    case "Refresh":
                        promptBehavior = PromptBehavior.RefreshSession;
                        break;
                    default:
                        promptBehavior = PromptBehavior.RefreshSession;
                        break;
                }

                // Get the access token       
                         
//#if DEBUG
//                //NOTE Easy way to authenticate while testing - set login and password here
//                var authenticationResult = authenticationContext.AcquireToken(GraphResource, ClientId, new UserCredential("user@domain.com", "password"));
//#else
//                var authenticationResult = authenticationContext.AcquireToken(GraphResource, ClientId, RedirectUri, promptBehavior);
//#endif

                var authenticationResult = authenticationContext.AcquireToken(GraphResource, ClientId, RedirectUri, promptBehavior);

                //"The browser based authentication dialog failed to complete. Reason: The server has not found anything matching the requested URI (Uniform Resource Identifier)."
                var accessToken = authenticationResult.AccessToken;                
                Log.InfoFormat("Access token: " + accessToken);
                return accessToken;
            }
            catch (System.Exception ex)
            {
                //Watch for {"authentication_canceled: User canceled authentication"}; or when cancelling final step: {"AADSTS65004: The resource owner or authorization server denied the request.\r\nTrace ID: e5e00775-0f74-4d61-9e0e-38398cd768da\r\nCorrelation ID: edce93a1-20d7-41da-b672-12ef1c2c2644\r\nTimestamp: 2016-12-14 19:33:17Z"}
                Log.Error(ex);
            }

            return null;
        }
        /// <summary>
        /// Prepare an HttpClient with the an authorization header (access token)
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static HttpClient GetHttpClient(string accessToken)
        {
            // Create the HTTP client with the access token
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer",
                    accessToken);
            return httpClient;
        }
        /// <summary>
        /// Get the current user using a prepared HttpClient (with an authorization header)
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public static async Task GetUserAsync(string accessToken)
        {
            // Get and deserialize the user
            try
            {
                HttpClient httpClient = GetHttpClient(accessToken);
                //https://outlook.office365.com/v1.0/me
                //var userResponse = await httpClient.GetStringAsync(GraphResource + GraphVersion + "/me/");
                //string request = String.Format("{0}/me", Properties.Settings.Default.V2EndPoint);
                //var response = await httpClient.GetAsync(GraphResource + GraphVersion + "/me");
                var response = await httpClient.GetAsync(Properties.Settings.Default.V2EndPoint);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(String.Format("Error: {0}", response.ReasonPhrase));
                    return;
                }
                else
                {
                    // Read and deserialize response.                
                    var content = await response.Content.ReadAsStringAsync();
                    MigrationHelper.CurrentUser = JsonConvert.DeserializeObject<GraphUser>(content);

                    //TESTED Must only use Id before the @
                    string id = "";
                    int atIdx = MigrationHelper.CurrentUser.Id.IndexOf("@");
                    id = MigrationHelper.CurrentUser.Id.Substring(0, MigrationHelper.CurrentUser.Id.Length - atIdx - 1);
                    CurrentUserID = id;

                }
                //var user = JsonConvert.DeserializeObject<UserModel>(userResponse);
                //return user;
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
            }
            //return null;
        }
        internal static async Task<string> Login()
        {
            return GetAccessToken(Properties.Settings.Default.LoginMode);
        }
        internal static bool StartXRMSession(string accessToken)
        {
            bool result = false;

            //NOTE: There is a new Exchange SOAP API that must be called before any OCM specific APIs be invoked. This API, called StartXrmSession, is used by Exchange internal mechanisms to enable a given account for OCM – this is what creates the special sharing folders
            //NOTE: The ResponseCode is important – it will contain useful (albeit it slightly cryptic) error codes if anything is wrong. This API is often slow as it triggers a lot of processing within Exchange, especially if this is the first time it has been called for a given user. We used to get HTTP time-out failures, so the API was changed to handle it’s processing asynchronously. Hence, it is relatively common to get a ResponseCode of “ErrorPartialCompletion” which simply means the API didn’t finish yet – it doesn’t mean an actual failure. If you get this in a response, just try calling the API again.
            //NOTE: When the ResponseCode is ‘NoError’ you will find the following in the response:
            //      A) The sharing modern - group’s mailbox ID.
            //      B) The folder-id for the Contact sharing folder
            //      C) The folder-id for the Company sharing folder
            //      D) The folder-id for the Deal sharing folder
            //References to ‘MySalesGroup’ can be fully ignored.     

            using (Log.VerboseCall())
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string url = "https://outlook.office365.com/EWS/Exchange.asmx";

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    httpClient.DefaultRequestHeaders.Add("X-AnchorMailbox", CurrentUser.EmailAddress);
                    var httpContent = new StringContent(Resources.StartXrmSessionXMLRequest, Encoding.UTF8, "text/xml");
                    var response = httpClient.PostAsync(url, httpContent);

                    var content = response.Result.Content.ReadAsStringAsync();
                    // Check status code.
                    if (!response.Result.IsSuccessStatusCode)
                    {
                        //400 Bad Request
                        Log.Error(content);
                        MessageBox.Show("Could not initialize a session with OCM. Please try again later.", "OCM Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception("The response status code was not successful.");                        
                    }

                    XElement responseEnvelope = null;

                    try
                    {
                        responseEnvelope = XElement.Load(new StringReader(content.Result.ToString()));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }

                    if (responseEnvelope == null)
                    {
                        MessageBox.Show("Could not initialize a session with OCM. Please try again later.", "OCM Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Log.Error("Unexpected - null responseEnvelope");
                        return false;
                    }

                    IEnumerable<XElement> errorCodes = from errorCode in responseEnvelope.Descendants
        ("{http://schemas.microsoft.com/exchange/services/2006/messages}ResponseCode")
                                                       select errorCode;
                    foreach (var errorCode in errorCodes)
                    {
                        //"ErrorAllSalesTeamSyncJobCreationFailed"
                        if (errorCode.Value != "NoError")
                        {
                            //responseEnvelope.Value = "Failed to create 'All Sales Team' sync job
                            IEnumerable<XElement> messageNodes = from messageNode in responseEnvelope.Descendants
("{http://schemas.microsoft.com/exchange/services/2006/messages}MessageText")
                                                               select messageNode;
                            foreach (var node in messageNodes)
                            {
                                Log.Error(responseEnvelope.Value, node.Value);//error = node.Value;
                                break;
                            }
                            MessageBox.Show("Could not initialize a session with OCM. Please try again later.", "OCM Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    IEnumerable<XElement> elements = from idElement in
                        responseEnvelope.Descendants
                            ("{http://schemas.microsoft.com/exchange/services/2006/messages}StartXrmSessionResponse")
                        select idElement;
                    IEnumerable<XElement> valueElement = null;

                    try
                    {
                        valueElement = elements.Descendants("{http://schemas.microsoft.com/exchange/services/2006/messages}AllSalesTeamGroupMailboxGuid");
                        GroupMailboxID = valueElement.ElementAt(0).Value.ToString();
                        valueElement = elements.Descendants("{http://schemas.microsoft.com/exchange/services/2006/messages}AllSalesTeamDealsSubFolderId");
                        DealsFolderID = valueElement.ElementAt(0).Value.ToString();
                        valueElement = elements.Descendants("{http://schemas.microsoft.com/exchange/services/2006/messages}AllSalesTeamCompanySubFolderId");
                        CompaniesFolderID = valueElement.ElementAt(0).Value.ToString();
                        valueElement = elements.Descendants("{http://schemas.microsoft.com/exchange/services/2006/messages}AllSalesTeamContactsSubFolderId");
                        ContactsFolderID = valueElement.ElementAt(0).Value.ToString();
                        Log.InfoFormat("GroupMailboxID: {0}; DealsFolderID: {1}; CompaniesFolderID: {2}; ContactsFolderID: {3}", GroupMailboxID, DealsFolderID, CompaniesFolderID, ContactsFolderID);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not initialize a session with OCM. Please try again later.", "OCM Error",
   MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Log.Error(ex);
                        return false;
                    }          
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
            return result;
        }
#endregion
    }
}
