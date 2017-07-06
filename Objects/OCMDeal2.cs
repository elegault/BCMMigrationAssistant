using System;
using Newtonsoft.Json;

namespace BCM_Migration_Tool.Objects
{
    //Used for creating in POST
    public class OCMDeal2
    {
        public class Rootobject
        {
            public Rootobject()
            {
                SingleValueExtendedProperties = new SingleValueExtendedProperty[0];
            }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public decimal? Amount { get; set; } //was float
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Appliedhashtag[] AppliedHashtags { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset CloseTime { get; set; } //was DateTime; was DateTime?
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object CreatedBy { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Id { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Notes { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Owner { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string OwnerId { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Priority { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Int32 Probability { get; set; } //was int; was double?; decimal doesn't work either!

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object SourceMailbox { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object SourceMailboxGuid { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Stage { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Status { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object XrmId { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public SingleValueExtendedProperty[] SingleValueExtendedProperties { get; set; }
        }

        public class Appliedhashtag
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Id { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Application { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Createdby CreatedBy { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object DeepLink { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool IsAutoTagged { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool IsInlined { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Hashtag { get; set; }
        }

        public class Createdby
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public object Name { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Address { get; set; }
        }
    }
}
