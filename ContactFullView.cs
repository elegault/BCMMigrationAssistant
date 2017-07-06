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
    
    public partial class ContactFullView
    {
        public int ContactServiceID { get; set; }
        public long Version { get; set; }
        public long ContentVersion { get; set; }
        public int Type { get; set; }
        public Nullable<int> IrisSubType { get; set; }
        public bool IsDeletedLocally { get; set; }
        public Nullable<int> ParentContactServiceID { get; set; }
        public Nullable<System.Guid> EntryGUID { get; set; }
        public Nullable<System.Guid> ParentEntryID { get; set; }
        public string JobTitle { get; set; }
        public string Profession { get; set; }
        public string CompanyName { get; set; }
        public string Department { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> WeddingAnniversary { get; set; }
        public string ManagerName { get; set; }
        public string AssistantName { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOnUTC { get; set; }
        public Nullable<System.DateTime> LastAccessTime { get; set; }
        public string PrefContactMethod { get; set; }
        public Nullable<bool> DoNotEmail { get; set; }
        public Nullable<bool> DoNotFax { get; set; }
        public Nullable<bool> DoNotCall { get; set; }
        public Nullable<bool> DoNotSendLetter { get; set; }
        public Nullable<System.Guid> PrimaryContactGUID { get; set; }
        public string PaymentStatus { get; set; }
        public string Rating { get; set; }
        public bool Active { get; set; }
        public string CustomerID { get; set; }
        public string Territory { get; set; }
        public string TypeOfEntity { get; set; }
        public string WebAddress { get; set; }
        public Nullable<int> MessageFlags { get; set; }
        public string AssignedTo { get; set; }
        public string IMAddress { get; set; }
        public string OfficeLocation { get; set; }
        public byte[] BusinessCardDisplayDefinition { get; set; }
        public string ReferredBy { get; set; }
        public Nullable<System.Guid> ReferredEntryId { get; set; }
        public string LeadSource { get; set; }
        public string AreaOfInterest { get; set; }
        public string LeadScore { get; set; }
        public bool LeadScoreCalculated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string Prefix { get; set; }
        public string FileAs { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string HomeAddressStreet { get; set; }
        public string HomeAddressPOB { get; set; }
        public string HomeAddressCity { get; set; }
        public string HomeAddressState { get; set; }
        public string HomeAddressZip { get; set; }
        public string HomeAddressCountry { get; set; }
        public string HomeAddress { get; set; }
        public string WorkAddressStreet { get; set; }
        public string WorkAddressPOB { get; set; }
        public string WorkAddressCity { get; set; }
        public string WorkAddressState { get; set; }
        public string WorkAddressZip { get; set; }
        public string WorkAddressCountry { get; set; }
        public string BusinessAddress { get; set; }
        public string OtherAddressStreet { get; set; }
        public string OtherAddressPOB { get; set; }
        public string OtherAddressCity { get; set; }
        public string OtherAddressState { get; set; }
        public string OtherAddressZip { get; set; }
        public string OtherAddressCountry { get; set; }
        public string OtherAddress { get; set; }
        public string PostalAddress { get; set; }
        public string Email1Address { get; set; }
        public string Email1DisplayAs { get; set; }
        public string Email2Address { get; set; }
        public string Email2DisplayAs { get; set; }
        public string Email3Address { get; set; }
        public string Email3DisplayAs { get; set; }
        public string HomePhoneNum { get; set; }
        public string WorkPhoneNum { get; set; }
        public string MobilePhoneNum { get; set; }
        public string BusinessFaxNum { get; set; }
        public string BusinessPhone2 { get; set; }
        public string CallbackNum { get; set; }
        public string CompanyMainPhoneNum { get; set; }
        public string ISDNNum { get; set; }
        public string OtherFaxNum { get; set; }
        public string OtherPhoneNum { get; set; }
        public string TelexNum { get; set; }
        public string RadioPhoneNum { get; set; }
        public string PrimaryPhoneNum { get; set; }
        public string PhoneNum1 { get; set; }
        public string PhoneNum2 { get; set; }
        public string PhoneNum3 { get; set; }
        public string PhoneNum4 { get; set; }
        public string PhoneNum5 { get; set; }
        public string PhoneNum6 { get; set; }
        public string ContactNotes { get; set; }
        public byte[] CompressedRichText { get; set; }
        public Nullable<byte> MailingAddressIndicator { get; set; }
        public byte[] DisplaySelectors { get; set; }
        public string Nickname { get; set; }
        public string Children { get; set; }
        public string Hobby { get; set; }
        public string Spouse { get; set; }
        public string AccountNumber { get; set; }
        public string Revenue { get; set; }
        public string TickerSymbol { get; set; }
        public string Employees { get; set; }
        public Nullable<System.DateTime> ReminderTime { get; set; }
        public Nullable<System.DateTime> AutoReminderTime { get; set; }
        public Nullable<int> ReminderMinutesBeforeStart { get; set; }
        public Nullable<bool> ReminderOverrideDefault { get; set; }
        public Nullable<bool> ReminderPlaySound { get; set; }
        public string ReminderSoundFile { get; set; }
        public Nullable<System.DateTime> FlagDueBy { get; set; }
        public Nullable<bool> ReminderSet { get; set; }
        public string UserName { get; set; }
        public Nullable<byte> FlagStatus { get; set; }
        public string FollowUpFlag { get; set; }
        public Nullable<int> TodoItemFlags { get; set; }
        public byte[] SwappedTodoStore { get; set; }
        public byte[] SwappedTodoData { get; set; }
        public string TodoTitle { get; set; }
        public Nullable<System.DateTime> CommonStart { get; set; }
        public Nullable<System.DateTime> CommonEnd { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> AutoDueDate { get; set; }
        public Nullable<bool> Complete { get; set; }
        public Nullable<System.DateTime> FlagCompleteTime { get; set; }
        public string PostalAddressStreet { get; set; }
        public string PostalAddressCity { get; set; }
        public string PostalAddressState { get; set; }
        public string PostalAddressCountry { get; set; }
        public string PostalAddressZip { get; set; }
        public string PostalAddressPOB { get; set; }
        public Nullable<int> AttachmentCount { get; set; }
        public Nullable<bool> HasPicture { get; set; }
        public string YomiFirstName { get; set; }
        public string YomiLastName { get; set; }
        public string YomiCompanyName { get; set; }
        public string YomiAccountName { get; set; }
    }
}
