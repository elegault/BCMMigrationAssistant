using Newtonsoft.Json;

namespace BCM_Migration_Tool.Objects
{
    public partial class GraphUser
    {
        [JsonProperty("@odata.context")]
        public string OdataContext;

        [JsonProperty("@odata.id")]
        public string OdataId;

        [JsonProperty("Id")]
        public string Id;

        [JsonProperty("EmailAddress")]
        public string EmailAddress;

        [JsonProperty("DisplayName")]
        public string DisplayName;

        [JsonProperty("Alias")]
        public string Alias;

        [JsonProperty("MailboxGuid")]
        public string MailboxGuid;
    }
}