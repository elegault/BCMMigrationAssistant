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
    using System.Collections.Generic;
    
    public partial class ProjectExportView
    {
        public int ContactServiceID { get; set; }
        public long Version { get; set; }
        public long ContentVersion { get; set; }
        public int Type { get; set; }
        public bool IsDeletedLocally { get; set; }
        public string AssignedTo { get; set; }
        public string Subject { get; set; }
        public string FullName { get; set; }
        public byte[] CompressedRichText { get; set; }
        public Nullable<System.Guid> EntryGUID { get; set; }
        public Nullable<System.Guid> ParentEntryID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOnUTC { get; set; }
        public Nullable<System.DateTime> LastAccessTime { get; set; }
        public Nullable<byte> FlagStatus { get; set; }
        public string FollowUpFlag { get; set; }
        public Nullable<int> TodoItemFlags { get; set; }
        public byte[] SwappedTodoStore { get; set; }
        public byte[] SwappedTodoData { get; set; }
        public string TodoTitle { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<bool> Complete { get; set; }
        public Nullable<System.DateTime> FlagCompleteTime { get; set; }
        public Nullable<System.DateTime> ReminderTime { get; set; }
        public Nullable<int> ReminderMinutesBeforeStart { get; set; }
        public Nullable<bool> ReminderOverrideDefault { get; set; }
        public Nullable<bool> ReminderPlaySound { get; set; }
        public string ReminderSoundFile { get; set; }
        public Nullable<bool> ReminderSet { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> ProjectStartDate { get; set; }
        public Nullable<System.DateTime> ProjectDueDate { get; set; }
        public string ActivityPriority { get; set; }
        public Nullable<double> PercentComplete { get; set; }
        public string ProjectStatus { get; set; }
        public string ProjectType { get; set; }
        public string ParentDisplayName { get; set; }
        public Nullable<int> AttachmentCount { get; set; }
    }
}