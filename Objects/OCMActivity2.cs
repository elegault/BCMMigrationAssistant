using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using TracerX;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCM_Migration_Tool.Objects
{

    public class OCMActivity2 //Was Rootobject
    {
        public bool moreResultsAvailable { get; set; }
        public int count { get; set; }
        public object nextSkip { get; set; }
        public OCMActivity2Value[] result { get; set; } //Was Result
    }

    public class OCMActivity2Value //Was Result
    {
        public string id { get; set; }
        public string subtype { get; set; }
        public string actionVerb { get; set; }
        public string activityItemState { get; set; }
        public string displayName { get; set; }
        public DateTime eventTime { get; set; }
        public string sender { get; set; }
        public string sourceUser { get; set; }
        public object[] toRecipients { get; set; }
        public object subject { get; set; }
        public object preview { get; set; }
        public bool attachments { get; set; }
        public Inlinelinks inlineLinks { get; set; }
        public string modifiedProperties { get; set; }
        public object linkType { get; set; }
        public string linkedEntityNames { get; set; }
        public string[] targetEntities { get; set; }
        public object[] otherRelatedEntities { get; set; }
        public string text { get; set; }
        public Sharedwith[] sharedWith { get; set; }
        public string xrmId { get; set; }
        public int inlineStatusCode { get; set; }
        public object imageUrl { get; set; }
    }

    //public class Inlinelinks
    //{
    //    public Relationship[] relationships { get; set; }
    //}

    //public class Relationship
    //{
    //    public object fromEntityId { get; set; }
    //    public int fromEntityType { get; set; }
    //    public string toEntityId { get; set; }
    //    public string displayName { get; set; }
    //    public int toEntityType { get; set; }
    //    public int relationshipType { get; set; }
    //    public int inlineStatusCode { get; set; }
    //    public object imageUrl { get; set; }
    //}

    public class Sharedwith
    {
        public object parentFolderId { get; set; }
        public string parentItemXrmId { get; set; }
        public string sourceUser { get; set; }
        public string sourceUserDisplayName { get; set; }
        public object parentItemLinkId { get; set; }
        public int inlineStatusCode { get; set; }
        public string id { get; set; }
        public object imageUrl { get; set; }
    }

}
