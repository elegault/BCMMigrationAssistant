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
    
    public partial class ProductsMasterList
    {
        public int ID { get; set; }
        public System.Guid ProductGUID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal DefaultQty { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<decimal> PurchasePrice { get; set; }
        public Nullable<bool> Taxable { get; set; }
        public string UpcSku { get; set; }
        public string Type { get; set; }
    }
}
