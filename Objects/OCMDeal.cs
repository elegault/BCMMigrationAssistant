using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace BCM_Migration_Tool.Objects
{
    public class Deal //was Value
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
                Debug.WriteLine(String.Format("Error in BCM_Migration_Tool.Objects.Deal.GetBCMID(): {0}", ex.Message));
            }

            return result;
        }
        public decimal Amount { get; set; } //was int; errors otherwise (e.g. 8000.0)

        public bool AutoShareFutureActivities { get; set; }
        public string BCMID { get; set; } //Custom/internal prop

        public string ChangeKey { get; set; }

        public DateTime CloseTime { get; set; }

        public string Company { get; set; }

        public string CompanyId { get; set; } //Use to store EntryGUID for company for linking

        public object CreatedBy { get; set; }

        public DateTime CreationTime { get; set; }

        public object CurrencyCode { get; set; }

        public string[] Hashtags { get; set; }

        public string Id { get; set; }

        public Inlinelinks InlineLinks { get; set; }

        public DateTime LastLinkedActivityTime { get; set; }

        public object LastLinkedActivityType { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedTime { get; set; }
        //public int LinkType { get; set; } //Custom; maps to BCM Opportunity Type field for determining whether to link to company or contact

        public string Name { get; set; }

        public object Notes { get; set; }

        public string odataid { get; set; }

        public string Owner { get; set; }

        public string OwnerId { get; set; } //Use to store EntryGUID for contact for linking

        public string People { get; set; }

        public string Priority { get; set; }

        public int Probability { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SingleValueExtendedProperty[] SingleValueExtendedProperties { get; set; }

        public object SourceMailbox { get; set; }

        public string SourceMailboxGuid { get; set; }

        public string Stage { get; set; }

        public string Status { get; set; }        
        public object[] XrmDocuments { get; set; }

        public string XrmDocumentsodatacontext { get; set; }

        public string XrmId { get; set; }

        public object XrmSharingSourceUser { get; set; }

        public object XrmSharingSourceUserDisplayName { get; set; }
    }

    public class OCMDeal //was 'Rootobject', and nested within parent OCMDeal
    {
        public string odatacontext { get; set; }

        public string odatanextLink { get; set; }

        public Deal[] value { get; set; } //was Value[] 
    }
}
