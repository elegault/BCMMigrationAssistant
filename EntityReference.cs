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
    
    public partial class EntityReference
    {
        public int DataID { get; set; }
        public int ContactServiceID { get; set; }
        public int LinkID { get; set; }
        public int LinkItemType { get; set; }
    
        public virtual ContactMainTable ContactMainTable { get; set; }
        public virtual ContactMainTable ContactMainTable1 { get; set; }
        public virtual EntityTypesTable EntityTypesTable { get; set; }
        public virtual EntityUserField EntityUserField { get; set; }
    }
}
