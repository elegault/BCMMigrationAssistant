using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCM_Migration_Tool.Objects
{
    class OpportunityFullViewV3
    {
        public string AreaOfInterest { get; set; }

        public string AssignedTo { get; set; }

        public string Competition { get; set; }

        public int ContactServiceID { get; set; }

        public string CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedOn { get; set; }

        public Nullable<System.Guid> EntryGUID { get; set; }

        public bool IsDeletedLocally { get; set; }

        public string LeadSource { get; set; }

        public string ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedOn { get; set; }

        public Nullable<System.DateTime> OpportunityCloseDate { get; set; }

        public Nullable<decimal> OpportunityExpectedRevenue { get; set; }

        public string OpportunityName { get; set; }

        public string OpportunityStage { get; set; }

        public Nullable<byte> OpportunityStatus { get; set; }

        public Nullable<decimal> OpportunityTotal { get; set; }

        public Nullable<decimal> OpportunityTotalDiscount { get; set; }

        public string OpportunityType { get; set; }

        public Nullable<System.Guid> ParentEntryID { get; set; }

        public Nullable<double> Probability { get; set; }

        public string ReferredBy { get; set; }

        public Nullable<System.Guid> ReferredEntryId { get; set; }
    }
}
