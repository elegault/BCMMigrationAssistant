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
    
    public partial class ProjectTemplateTaskExportView
    {
        public System.Guid TemplateTaskGUID { get; set; }
        public System.Guid TemplateGUID { get; set; }
        public string Subject { get; set; }
        public Nullable<int> StartDateOffset { get; set; }
        public Nullable<int> Duration { get; set; }
        public Nullable<bool> AttentionRequired { get; set; }
        public string Priority { get; set; }
        public Nullable<bool> Reminder { get; set; }
        public Nullable<int> ReminderDateOffset { get; set; }
        public Nullable<short> ReminderTime { get; set; }
    }
}