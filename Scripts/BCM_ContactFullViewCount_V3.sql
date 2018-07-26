/*ContactFullView V3*/
SELECT Count(*)        
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