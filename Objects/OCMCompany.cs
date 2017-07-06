using Newtonsoft.Json;
using System;

namespace BCM_Migration_Tool.Objects
{
    public partial class OCMCompany
    {
        public class Emailaddress
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Address { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }
        }

        public class Inlinelinks
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Relationship[] Relationships { get; set; }
        }

        public class Relationship
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool Bidirectional { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTime CreationTime { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool Derived { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string DisplayName { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string ItemLinkId { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string ItemType { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string LinkId { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string LinkType { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string SourceMailboxId { get; set; }
        }

        public class Rootobject
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Businessaddress BusinessAddress { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string BusinessHomePage { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object[] BusinessPhones { get; set; } //NOTE Was object; not working? Back to string

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object CustomerBit { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string CustomerStatus { get; set; }
            //public DateTime CreatedDateTime { get; set; }
            //public DateTime LastModifiedDateTime { get; set; }
            //public string ChangeKey { get; set; }
            //public object[] Categories { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string DisplayName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object[] EmailAddresses { get; set; } //NOTE Was object; not working? Back to EmailAddress[]
            //public string odatacontext { get; set; }
            //public string odataid { get; set; }
            //public string odataetag { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Inlinelinks InlineLinks { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object LastLinkedActivityTime { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object LastLinkedActivityType { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Manager { get; set; }

            //public object TickerSymbol { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Notes { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Profession { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public SingleValueExtendedProperty[] SingleValueExtendedProperties { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object SourceMailboxGuid { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string XrmCompanyPeople { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int XrmCompanySize { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string XrmContactId { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int XrmContactType { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string XrmId { get; set; }
        }
    }
}
