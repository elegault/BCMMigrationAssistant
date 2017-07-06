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
    
    public partial class ContactDetailsTable
    {
        public int ContactServiceID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string WorkAddressCity { get; set; }
        public string WorkAddressCountry { get; set; }
        public string WorkAddressPOB { get; set; }
        public string WorkAddressZip { get; set; }
        public string WorkAddressState { get; set; }
        public string WorkAddressStreet { get; set; }
        public string HomeAddressCity { get; set; }
        public string HomeAddressCountry { get; set; }
        public string HomeAddressPOB { get; set; }
        public string HomeAddressZip { get; set; }
        public string HomeAddressState { get; set; }
        public string HomeAddressStreet { get; set; }
        public string OtherAddressCity { get; set; }
        public string OtherAddressCountry { get; set; }
        public string OtherAddressPOB { get; set; }
        public string OtherAddressZip { get; set; }
        public string OtherAddressState { get; set; }
        public string OtherAddressStreet { get; set; }
        public string PhoneNum1 { get; set; }
        public string BusinessPhone2 { get; set; }
        public string CallbackNum { get; set; }
        public string PhoneNum4 { get; set; }
        public string ISDNNum { get; set; }
        public string PhoneNum5 { get; set; }
        public string PrimaryPhoneNum { get; set; }
        public string RadioPhoneNum { get; set; }
        public string TelexNum { get; set; }
        public string PhoneNum6 { get; set; }
        public byte[] DisplaySelectors { get; set; }
        public string EmailDisplayAs1 { get; set; }
        public string EmailDisplayAs2 { get; set; }
        public string EmailDisplayAs3 { get; set; }
        public string ContactNotes { get; set; }
        public string ReferredBy { get; set; }
        public string LeadSource { get; set; }
        public string PrefContactMethod { get; set; }
        public bool DoNotCall { get; set; }
        public bool DoNotEmail { get; set; }
        public bool DoNotFax { get; set; }
        public bool DoNotSendLetter { get; set; }
        public string AreaOfInterest { get; set; }
        public string Profession { get; set; }
        public string ManagerName { get; set; }
        public string AssistantName { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> WeddingAnniversary { get; set; }
        public string HomeAddress { get; set; }
        public string OtherAddress { get; set; }
        public Nullable<int> EmailListType { get; set; }
        public string AddressBookEmailAddress1 { get; set; }
        public string AddressBookEmailAddress2 { get; set; }
        public string AddressBookEmailAddress3 { get; set; }
        public string AddressBookEmailAddress4 { get; set; }
        public string AddressBookEmailAddress5 { get; set; }
        public string AddressBookEmailAddress6 { get; set; }
        public string EmailAddressType1 { get; set; }
        public string EmailAddressType2 { get; set; }
        public string EmailAddressType3 { get; set; }
        public string EmailAddressType4 { get; set; }
        public string EmailAddressType5 { get; set; }
        public string EmailAddressType6 { get; set; }
        public byte[] AddressBookEntryId1 { get; set; }
        public byte[] AddressBookEntryId2 { get; set; }
        public byte[] AddressBookEntryId3 { get; set; }
        public byte[] AddressBookEntryId4 { get; set; }
        public byte[] AddressBookEntryId5 { get; set; }
        public byte[] AddressBookEntryId6 { get; set; }
        public Nullable<System.DateTime> MessageDeliveryTime { get; set; }
        public string CustomerID { get; set; }
        public string PostalAddressStreet { get; set; }
        public string PostalAddressCity { get; set; }
        public string PostalAddressState { get; set; }
        public string PostalAddressCountry { get; set; }
        public string PostalAddressPOB { get; set; }
        public string AssignedTo { get; set; }
        public string IMAddress { get; set; }
        public string OfficeLocation { get; set; }
        public string Nickname { get; set; }
        public string Children { get; set; }
        public string Hobby { get; set; }
        public string Spouse { get; set; }
        public Nullable<System.Guid> ReferredEntryId { get; set; }
        public byte[] CompressedRichText { get; set; }
        public string User1 { get; set; }
        public string User2 { get; set; }
        public string User3 { get; set; }
        public string User4 { get; set; }
        public Nullable<double> PercentComplete { get; set; }
        public string YomiFirstName { get; set; }
        public string YomiLastName { get; set; }
        public string YomiCompanyName { get; set; }
        public string Territory { get; set; }
        public string TypeOfEntity { get; set; }
        public string AccountNumber { get; set; }
        public string Revenue { get; set; }
        public string TickerSymbol { get; set; }
        public string Employees { get; set; }
        public string YomiAccountName { get; set; }
        public string OpportunityType { get; set; }
        public Nullable<double> Probability { get; set; }
        public string OpportunityStage { get; set; }
        public Nullable<byte> OpportunityStatus { get; set; }
        public Nullable<System.DateTime> OpportunityCloseDate { get; set; }
        public string Competition { get; set; }
        public string PaymentTerms { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<System.DateTime> ProjectStartDate { get; set; }
        public Nullable<System.DateTime> ProjectDueDate { get; set; }
        public string Priority { get; set; }
        public string ProjectStatus { get; set; }
        public string ProjectType { get; set; }
    
        public virtual ContactMainTable ContactMainTable { get; set; }
    }
}
