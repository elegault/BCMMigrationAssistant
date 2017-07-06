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
    
    public partial class AccountsFullView
    {
        public int ContactServiceID { get; set; }
        public bool IsDeletedLocally { get; set; }
        public Nullable<System.Guid> EntryGUID { get; set; }
        public string CompanyName { get; set; }
        public string CustomerID { get; set; }
        public string WorkAddressCity { get; set; }
        public string WorkAddressState { get; set; }
        public string WorkAddressStreet { get; set; }
        public string WorkAddressZip { get; set; }
        public string WorkAddressCountry { get; set; }
        public string OtherAddressCity { get; set; }
        public string OtherAddressState { get; set; }
        public string OtherAddressStreet { get; set; }
        public string OtherAddressZip { get; set; }
        public string OtherAddressCountry { get; set; }
        public int Type { get; set; }
        public string WorkPhoneNum { get; set; }
        public string BusinessPhone2 { get; set; }
        public string CompanyMainPhoneNum { get; set; }
        public string BusinessFaxNum { get; set; }
        public string ReferredBy { get; set; }
        public Nullable<System.Guid> ReferredEntryId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string WebAddress { get; set; }
        public string Email1Address { get; set; }
        public string Email1DisplayAs { get; set; }
        public string Email2Address { get; set; }
        public string Email2DisplayAs { get; set; }
        public string Email3Address { get; set; }
        public string Email3DisplayAs { get; set; }
        public Nullable<System.Guid> PrimaryContactGUID { get; set; }
        public Nullable<System.Guid> ParentEntryID { get; set; }
        public string FileAs { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> LastAccessTime { get; set; }
        public Nullable<bool> DoNotEmail { get; set; }
        public Nullable<bool> DoNotFax { get; set; }
        public Nullable<bool> DoNotCall { get; set; }
        public Nullable<bool> DoNotSendLetter { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOnUTC { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ContactNotes { get; set; }
        public byte[] CompressedRichText { get; set; }
        public string AccountPaymentStatus { get; set; }
        public Nullable<short> AccountPaymentStatusOrderId { get; set; }
        public string AccountRating { get; set; }
        public Nullable<short> AccountRatingOrderId { get; set; }
        public bool AccountActive { get; set; }
        public string AccountTerritory { get; set; }
        public Nullable<short> AccountTerritoryOrderId { get; set; }
        public string TypeOfEntity { get; set; }
        public Nullable<short> TypeOfEntityOrderID { get; set; }
        public string LeadSource { get; set; }
        public Nullable<short> LeadSourceOrderID { get; set; }
        public string PrefContactMethod { get; set; }
        public Nullable<short> PrefContactMethodOrderID { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactFullName { get; set; }
        public Nullable<System.Guid> PrimaryContactEntryGUID { get; set; }
        public string PrimaryContactWorkStreet { get; set; }
        public string PrimaryContactWorkCity { get; set; }
        public string PrimaryContactWorkState { get; set; }
        public string PrimaryContactWorkZip { get; set; }
        public string PrimaryContactWorkCountry { get; set; }
        public string PrimaryContactEmailAddress { get; set; }
        public string PrimaryContactWorkPhone { get; set; }
        public string PrimaryContactBusinessFax { get; set; }
        public Nullable<System.Guid> PrimaryContactParentEntryID { get; set; }
        public string PrimaryContactNotes { get; set; }
    }
}