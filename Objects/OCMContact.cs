using System;
using Newtonsoft.Json;

namespace BCM_Migration_Tool.Objects
{
    /// <summary>
    /// Used for POST/PATCH operations
    /// </summary>
    public partial class OCMContact
    {
        //public partial class Businessaddress
        //{
        //    public object City { get; set; }

        //    public object CountryOrRegion { get; set; }

        //    public object PostalCode { get; set; }

        //    public object State { get; set; }

        //    public object Street { get; set; }
        //}

        public partial class Emailaddress
        {
            public string Address { get; set; }

            public string Name { get; set; }
        }

        //public partial class Homeaddress
        //{
        //    public string City { get; set; }

        //    public object CountryOrRegion { get; set; }

        //    public string PostalCode { get; set; }

        //    public string State { get; set; }

        //    public string Street { get; set; }
        //}

        //public partial class Otheraddress
        //{
        //    public object City { get; set; }

        //    public object CountryOrRegion { get; set; }

        //    public object PostalCode { get; set; }

        //    public object State { get; set; }

        //    public object Street { get; set; }
        //}
        public partial class Rootobject
        {
            //public object AssistantName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset Birthday { get; set; } //Was DateTime?
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Businessaddress BusinessAddress { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string[] BusinessPhones { get; set; }

            //public object[] Categories { get; set; }

            //public object[] Children { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object CompanyName { get; set; }

            //public string Department { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string DisplayName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Emailaddress[] EmailAddresses { get; set; }

            //public string FileAs { get; set; }

            //public object Generation { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string GivenName { get; set; }

            //public Homeaddress HomeAddress { get; set; } 
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string[] HomePhones { get; set; }            
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get;set; }
            //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            //public bool IsCustomer { get; set; }

            //public object Initials { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string JobTitle { get; set; }

            //public object Manager { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object MiddleName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string MobilePhone1 { get; set; }

            //public object NickName { get; set; }

            //public string OfficeLocation { get; set; }

            //public Otheraddress OtherAddress { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object PersonalNotes { get; set; }

            //public object Profession { get; set; }

            //public object SpouseName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Surname { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Title { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public SingleValueExtendedProperty[] SingleValueExtendedProperties { get; set; }
        }
    }
}
