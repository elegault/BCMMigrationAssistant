﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace BCM_Migration_Tool.Objects
{
    public class OCMCompany2
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string odatanextLink { get; set; }

        public OCMCompany2Value[] value { get; set; }
    }

    public class OCMCompany2Value
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
                Debug.WriteLine(String.Format("Error in BCM_Migration_Tool.Objects.OCMCompany2Value.GetBCMID(): {0}", ex.Message));
            }

            return result;
        }
        public string BCMID { get; set; } //NOTE Custom prop; set on the object retrieved from the response when creating an item, and used to create activities
        public Businessaddress BusinessAddress { get; set; }

        public string BusinessHomePage { get; set; } //was object

        public object[] BusinessPhones { get; set; }

        public object[] Categories { get; set; }

        public string ChangeKey { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool? CustomerBit { get; set; }

        public string CustomerStatus { get; set; }

        public string DisplayName { get; set; }

        public object[] EmailAddresses { get; set; }

        public string Id { get; set; }

        public Inlinelinks InlineLinks { get; set; }

        public DateTime? LastLinkedActivityTime { get; set; }

        public object LastLinkedActivityType { get; set; }

        public DateTime LastModifiedDateTime { get; set; }

        public object Manager { get; set; }

        public object Notes { get; set; }

        public string odataetag { get; set; }

        public string odataid { get; set; }

        public object Profession { get; set; }

        public SingleValueExtendedProperty[] SingleValueExtendedProperties { get; set; }

        public object SourceMailboxGuid { get; set; }

        public object TickerSymbol { get; set; }

        public string XrmCompanyPeople { get; set; }

        public int XrmCompanySize { get; set; }

        public string XrmContactId { get; set; }

        public int XrmContactType { get; set; }

        public string XrmId { get; set; }
    }
    //public class Businessaddress
    //{
    //    public string Type { get; set; }
    //    public string Street { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string CountryOrRegion { get; set; }
    //    public string PostalCode { get; set; }
    //}

    //public class Inlinelinks
    //{
    //    public Relationship[] Relationships { get; set; }
    //}

    //public class Relationship
    //{
    //    public string ItemLinkId { get; set; }
    //    public string ItemType { get; set; }
    //    public string LinkId { get; set; }
    //    public string LinkType { get; set; }
    //    public DateTime CreationTime { get; set; }
    //    public string SourceMailboxId { get; set; }
    //    public string DisplayName { get; set; }
    //    public bool Bidirectional { get; set; }
    //    public bool Derived { get; set; }
    //}
}
