SELECT        acct.FileAs, primCt.FileAs AS PrimaryContact, primCt.IsDeletedLocally
FROM            ContactFullView AS acct LEFT OUTER JOIN
                         ContactFullView AS primCt ON acct.PrimaryContactGUID = primCt.EntryGUID LEFT OUTER JOIN
                         ContactPicklistValuesView AS cplv ON acct.ContactServiceID = cplv.ContactServiceID
WHERE        (acct.Type = 2)
ORDER BY acct.FileAs