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
    
    public partial class ContactsUpdatesTable
    {
        public System.Guid EntryGUID { get; set; }
        public Nullable<System.DateTime> UpdateStart { get; set; }
        public bool IsDeletedLocally { get; set; }
        public Nullable<System.Guid> ParentEntryID { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string JobTitle { get; set; }
        public string Email1Address { get; set; }
        public string Email2Address { get; set; }
        public string Email3Address { get; set; }
        public string BusinessTelephoneNumber { get; set; }
        public string HomeTelephoneNumber { get; set; }
        public string MobileTelephoneNumber { get; set; }
        public string BusinessFaxNumber { get; set; }
        public string HomeFaxNumber { get; set; }
        public string OtherFaxNumber { get; set; }
    }
}
