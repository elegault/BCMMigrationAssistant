using System;
using Newtonsoft.Json;

namespace BCM_Migration_Tool.Objects
{
    /// <summary>
    /// Used for POST/PATCH operations
    /// </summary>
    public partial class OCMContact
    {
        public partial class Emailaddress
        {
            public string Address { get; set; }

            public string Name { get; set; }
        }
        public partial class Rootobject
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? Birthday { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Businessaddress BusinessAddress { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string[] BusinessPhones { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object CompanyName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string DisplayName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Emailaddress[] EmailAddresses { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string GivenName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string[] HomePhones { get; set; }            
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get;set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string JobTitle { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object MiddleName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string MobilePhone1 { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object PersonalNotes { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Surname { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Title { get; set; }        
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public SingleValueExtendedProperty[] SingleValueExtendedProperties { get; set; }
        }
    }
}
