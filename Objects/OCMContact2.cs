using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace BCM_Migration_Tool.Objects
{
    public class Homeaddress
    {
        //This should be empty
    }

    /// <summary>
    /// Returned from GET calls
    /// </summary>
    public class OCMContact2 //was 'Rootobject'
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string odatanextLink { get; set; }

        public OCMContact2Value[] value { get; set; } //was Value[] //was Contact
    }

    public class OCMContact2Value//was Value //was Contact
    {
        public string GetBCMID()
        {
            string result = "";

            try
            {
                if (!String.IsNullOrEmpty(BCMID))
                    return BCMID;
                if (SingleValueExtendedProperties == null)
                    return result;

                foreach (var prop in SingleValueExtendedProperties)
                {
                    if (prop.PropertyId.ToLower() == "string {bc013ba3-3a6d-4826-b0ec-cb703a722b09} name bcmid")
                    {
                        result = prop.Value;
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(String.Format("Error in BCM_Migration_Tool.Objects.OCMContact2Value.GetBCMID(): {0}", ex.Message));
            }

            return result;
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SingleValueExtendedProperty[] SingleValueExtendedProperties;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object AssistantName { get; set; }
        public string BCMID { get; set; } //NOTE Custom prop; set on the object retrieved from the response when creating an item, and used to create activities

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset Birthday { get; set; } //was object[]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Businessaddress BusinessAddress { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object BusinessHomePage { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] BusinessPhones { get; set; } //was object
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object[] Categories { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ChangeKey { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object[] Children { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object CompanyName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Department { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OCMContact.Emailaddress[] EmailAddresses { get; set; } //was object[]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FileAs { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Generation { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GivenName { get; set; } //was object
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Homeaddress HomeAddress { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] HomePhones { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object[] ImAddresses { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Initials { get; set; }
        public string ItemLinkId { get; set; } //NOTE: Custom prop, not returned by any queries but used to stuff in the value after running GetPersona on the contact
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string JobTitle { get; set; } //was object
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastModifiedDateTime { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Manager { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object MiddleName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object MobilePhone1 { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object NickName { get; set; }

        [JsonProperty("@odata.etag")]
        public string odataetag { get; set; }

        [JsonProperty("@odata.id")]
        public string odataid { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object OfficeLocation { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Otheraddress OtherAddress { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParentFolderId { get; set; }
        public string PersonaId { get; set; } //NOTE: Custom prop, not returned by any queries but used to stuff in the value after running GetPersona on the contact
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object PersonalNotes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Profession { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object SpouseName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Surname { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object YomiCompanyName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object YomiGivenName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object YomiSurname { get; set; }
    }

    //public class Businessaddress
    //{
    //    public string Street { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string CountryOrRegion { get; set; }
    //    public string PostalCode { get; set; }
    //}

    public class Otheraddress
    {
        //This should be empty
    }
}
