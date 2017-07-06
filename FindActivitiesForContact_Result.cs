//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BcmMigrationTool
{
    using System;
    
    public partial class FindActivitiesForContact_Result
    {
        public System.Guid ActivityBodyGUID { get; set; }
        public System.Guid ActivityLineGUID { get; set; }
        public int ActivityType { get; set; }
        public string Subject { get; set; }
        public Nullable<System.DateTime> ActivityDate { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.Guid> OwnerID { get; set; }
        public Nullable<long> Version { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ActivityStartTime { get; set; }
        public Nullable<int> ActivityDuration { get; set; }
        public string PhoneNumCalled { get; set; }
        public string FromName { get; set; }
        public string ToNames { get; set; }
        public string CCNames { get; set; }
        public string ActivityStatus { get; set; }
        public string ActivityPriority { get; set; }
        public Nullable<byte> ActivityPercentComplete { get; set; }
        public string ActivityLocation { get; set; }
        public string LinkToOriginal { get; set; }
        public Nullable<short> LinkToOriginalType { get; set; }
        public Nullable<System.DateTime> ActivitySentTime { get; set; }
        public string ActivityNote { get; set; }
        public bool IsDeletedLocally { get; set; }
        public bool IsLinkDeletedLocally { get; set; }
    }
}
