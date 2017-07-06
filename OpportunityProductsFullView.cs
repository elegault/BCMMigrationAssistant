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
    
    public partial class OpportunityProductsFullView
    {
        public int ContactServiceID { get; set; }
        public string OpportunityType { get; set; }
        public string OpportunityStage { get; set; }
        public Nullable<byte> OpportunityStatus { get; set; }
        public Nullable<System.DateTime> OpportunityCloseDate { get; set; }
        public Nullable<double> Probability { get; set; }
        public string ReferredBy { get; set; }
        public string LeadSource { get; set; }
        public string Competition { get; set; }
        public string OpportunityName { get; set; }
        public Nullable<System.Guid> ParentEntryID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.Guid> ProductLineGUID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> AdjustedPrice { get; set; }
        public Nullable<double> AdjustedPercent { get; set; }
        public Nullable<decimal> ExtendedDiscountAmount { get; set; }
        public Nullable<double> DiscountPercent { get; set; }
        public Nullable<bool> Taxable { get; set; }
        public Nullable<decimal> ExtendedAmount { get; set; }
        public Nullable<decimal> ProductExpectedRevenue { get; set; }
        public Nullable<decimal> ProductGrossMargin { get; set; }
        public Nullable<decimal> ProductExpectedGrossMargin { get; set; }
        public Nullable<System.Guid> EntryGUID { get; set; }
        public Nullable<System.Guid> GrandParentEntryID { get; set; }
        public string ContactNotes { get; set; }
        public bool IsDeletedLocally { get; set; }
        public string AssignedTo { get; set; }
    }
}