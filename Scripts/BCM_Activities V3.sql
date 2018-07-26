SELECT        ContactNamesTable.FullName, ContactNamesTable.FirstName, ContactNamesTable.LastName, CMT.CompanyName, AT.Subject, AT.ActivityType, ActivityNotesTable.ActivityNote, AT.ActivityID, AC.ContactID, CMT.EntryGUID, 
                         AT.ActivityGUID, AT.CreatedOn
FROM            ActivitiesTable AS AT INNER JOIN
                         ActivityContacts AS AC ON AT.ActivityID = AC.ActivityID INNER JOIN
                         ContactMainTable AS CMT ON CMT.ContactServiceID = AC.ContactID INNER JOIN
                         ContactNamesTable ON CMT.ContactServiceID = ContactNamesTable.ContactServiceID INNER JOIN
                         ActivityNotesTable ON AT.ActivityID = ActivityNotesTable.ActivityID
WHERE        (AT.IsDeletedLocally = 0) AND (AT.ActivityType = 14 OR
                         AT.ActivityType = 15 OR
                         AT.ActivityType = 6)