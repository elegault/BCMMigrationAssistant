--V3 OpportunityFullView
SELECT Count(*) FROM OpportunitiesAdditionalTable AS oat INNER JOIN
                         ContactMainTable AS cmt ON oat.ContactServiceID = cmt.ContactServiceID LEFT OUTER JOIN
                         ContactLeadInfoTable AS clt ON oat.ContactServiceID = clt.ContactServiceID LEFT OUTER JOIN
                         ContactNamesTable AS cnt ON cmt.ContactServiceID = cnt.ContactServiceID LEFT OUTER JOIN
                             (SELECT        ContactServiceID, EntryGUID, ISNULL(SUM(ExtendedAmount), 0) AS OpportunityTotal, ISNULL(SUM(ProductExpectedRevenue), 0) AS OpportunityExpectedRevenue, ISNULL(SUM(ExtendedDiscountAmount), 0) 
                                                         AS OpportunityTotalDiscount
                               FROM            OpportunityProductsFullView
                               GROUP BY ContactServiceID, EntryGUID) AS oa ON oat.ContactServiceID = oa.ContactServiceID
WHERE        (cmt.IsDeletedLocally = 0)