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
    
    public partial class ProjectTaskTable
    {
        public int ActivityID { get; set; }
        public Nullable<System.DateTime> GmtDueDate { get; set; }
        public Nullable<System.DateTime> GmtStartDate { get; set; }
        public Nullable<bool> Complete { get; set; }
        public Nullable<int> TodoItemFlags { get; set; }
        public byte[] SwappedTodoStore { get; set; }
        public byte[] SwappedTodoData { get; set; }
        public string TodoTitle { get; set; }
        public Nullable<int> TaskStatus { get; set; }
        public Nullable<double> PercentComplete { get; set; }
        public Nullable<System.DateTime> DateCompleted { get; set; }
        public string AssignedTo { get; set; }
        public Nullable<int> TotalWork { get; set; }
        public Nullable<int> ActualWork { get; set; }
        public string Mileage { get; set; }
        public Nullable<bool> AttentionRequired { get; set; }
        public Nullable<System.DateTime> FlagCompleteTime { get; set; }
        public Nullable<System.DateTime> ReplyTime { get; set; }
        public Nullable<System.DateTime> CommonStart { get; set; }
        public Nullable<byte> FlagStatus { get; set; }
    
        public virtual ActivitiesTable ActivitiesTable { get; set; }
    }
}
