/*ContactFullView V3*/
SELECT        cmt.ContactServiceID, cmt.Version, cmt.ContentVersion, cmt.Type, cmt.IsDeletedLocally, cmt.ParentContactServiceID, cmt.EntryGUID, cmt.ParentEntryID, cmt.JobTitle, cmt.Profession, cmt.CompanyName, cmt.Department, 
                         cmt.Birthday, cmt.WeddingAnniversary, cmt.ManagerName, cmt.AssistantName, cmt.CreatedBy, cmt.CreatedOn, cmt.ModifiedBy, cmt.ModifiedOn, cmt.ModifiedOnUTC, cmt.LastAccessTime, cmt.PrefContactMethod, 
                         cmt.DoNotEmail, cmt.DoNotFax, cmt.DoNotCall, cmt.DoNotSendLetter, cmt.PrimaryContactGUID, cmt.PaymentStatus, cmt.Rating, cmt.Active, cmt.CustomerID, cmt.Territory, cmt.TypeOfEntity, cmt.WebAddress, cmt.MessageFlags,
                          cmt.Lead, cmt.AssignedTo, cmt.IMAddress, cmt.OfficeLocation, cmt.BusinessCardDisplayDefinition, clt.ReferredBy, clt.ReferredEntryId, clt.LeadSource, clt.AreaOfInterest, cn.FirstName, cn.LastName, cn.MiddleName, cn.Suffix, 
                         cn.Prefix, cn.FileAs, cn.FullName, cn.Subject, cht.AddressStreet AS HomeAddressStreet, cht.AddressPOB AS HomeAddressPOB, cht.AddressCity AS HomeAddressCity, cht.AddressState AS HomeAddressState, 
                         cht.AddressZip AS HomeAddressZip, cht.AddressCountry AS HomeAddressCountry, cht.HomeAddress, cwt.AddressStreet AS WorkAddressStreet, cwt.AddressPOB AS WorkAddressPOB, cwt.AddressCity AS WorkAddressCity, 
                         cwt.AddressState AS WorkAddressState, cwt.AddressZip AS WorkAddressZip, cwt.AddressCountry AS WorkAddressCountry, cwt.BusinessAddress, coat.AddressStreet AS OtherAddressStreet, 
                         coat.AddressPOB AS OtherAddressPOB, coat.AddressCity AS OtherAddressCity, coat.AddressState AS OtherAddressState, coat.AddressZip AS OtherAddressZip, coat.AddressCountry AS OtherAddressCountry, coat.PostalAddress, 
                         coat.OtherAddress, ce1.EmailAddress AS Email1Address, ce1.EmailDisplayAs AS Email1DisplayAs, ce2.EmailAddress AS Email2Address, ce2.EmailDisplayAs AS Email2DisplayAs, ce3.EmailAddress AS Email3Address, 
                         ce3.EmailDisplayAs AS Email3DisplayAs, cft.HomePhoneNum, cft.WorkPhoneNum, cft.MobilePhoneNum, cft.BusinessFaxNum, cft.BusinessPhone2, cft.CallbackNum, cft.CompanyMainPhoneNum, cft.ISDNNum, cft.OtherFaxNum, 
                         cft.OtherPhoneNum, cft.TelexNum, cft.RadioPhoneNum, cft.PrimaryPhoneNum, cft.PhoneNum1, cft.PhoneNum2, cft.PhoneNum3, cft.PhoneNum4, cft.PhoneNum5, cft.PhoneNum6, cnt.ContactNotes, cnt.CompressedRichText, 
                         cst.MailingAddressIndicator, cst.DisplaySelectors, cat.ContactPicture, cat.BusinessCardPicture, copt.Nickname, copt.Children, copt.Hobby, copt.Spouse, aopt.AccountNumber, aopt.Revenue, aopt.TickerSymbol, aopt.Employees, 
                         crt.ReminderTime, crt.ReminderMinutesBeforeStart, crt.ReminderOverrideDefault, crt.ReminderPlaySound, crt.ReminderSoundFile, crt.FlagDueBy, crt.ReminderSet, crt.UserName, ctdt.FlagStatus, ctdt.FollowUpFlag, 
                         ctdt.TodoItemFlags, ctdt.SwappedTodoStore, ctdt.SwappedTodoData, ctdt.TodoTitle, ctdt.CommonStart, ctdt.CommonEnd, ctdt.StartDate, ctdt.DueDate, ctdt.Complete, ctdt.FlagCompleteTime, cpt.PostalAddressStreet, 
                         cpt.PostalAddressCity, cpt.PostalAddressState, cpt.PostalAddressCountry, cpt.PostalAddressZip, cpt.PostalAddressPOB
FROM            ContactMainTable AS cmt LEFT OUTER JOIN
                         ContactNamesTable AS cn ON cmt.ContactServiceID = cn.ContactServiceID LEFT OUTER JOIN
                         ContactHomeAddressTable AS cht ON cmt.ContactServiceID = cht.ContactServiceID LEFT OUTER JOIN
                         ContactWorkAddressTable AS cwt ON cmt.ContactServiceID = cwt.ContactServiceID LEFT OUTER JOIN
                         ContactOtherAddressTable AS coat ON cmt.ContactServiceID = coat.ContactServiceID LEFT OUTER JOIN
                         ContactEmail1Table AS ce1 ON cmt.ContactServiceID = ce1.ContactServiceID LEFT OUTER JOIN
                         ContactEmail2Table AS ce2 ON cmt.ContactServiceID = ce2.ContactServiceID LEFT OUTER JOIN
                         ContactEmail3Table AS ce3 ON cmt.ContactServiceID = ce3.ContactServiceID LEFT OUTER JOIN
                         ContactPhoneTable AS cft ON cmt.ContactServiceID = cft.ContactServiceID LEFT OUTER JOIN
                         ContactNotesTable AS cnt ON cmt.ContactServiceID = cnt.ContactServiceID LEFT OUTER JOIN
                         ContactSelectorsTable AS cst ON cmt.ContactServiceID = cst.ContactServiceID LEFT OUTER JOIN
                         ContactLeadInfoTable AS clt ON cmt.ContactServiceID = clt.ContactServiceID LEFT OUTER JOIN
                         ContactOnlyPropertiesTable AS copt ON cmt.ContactServiceID = copt.ContactServiceID LEFT OUTER JOIN
                         AccountOnlyPropertiesTable AS aopt ON cmt.ContactServiceID = aopt.ContactServiceID LEFT OUTER JOIN
                         ContactRemindersTable AS crt ON cmt.ContactServiceID = crt.ContactServiceID AND crt.UserName = SUSER_SNAME() LEFT OUTER JOIN
                         ContactTodoTable AS ctdt ON cmt.ContactServiceID = ctdt.ContactServiceID AND ctdt.UserName = SUSER_SNAME() LEFT OUTER JOIN
                         ContactPostalAddressTable AS cpt ON cmt.ContactServiceID = cpt.ContactServiceID LEFT OUTER JOIN
                         ContactAttachmentsTable AS cat ON cmt.ContactServiceID = cat.ContactServiceID
WHERE        (cmt.Type = 1 AND cmt.IsDeletedLocally = 0)