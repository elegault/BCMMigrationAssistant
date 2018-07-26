--V3 OpportunityFullView
SELECT        oat.ContactServiceID, oat.OpportunityType, oat.OpportunityStage, oat.OpportunityCloseDate, oat.OpportunityStatus, oat.Probability, oa.OpportunityTotal, oa.OpportunityExpectedRevenue, oa.OpportunityTotalDiscount, 
                         clt.ReferredBy, clt.ReferredEntryId, clt.LeadSource, clt.AreaOfInterest, oat.Competition, cnt.Subject AS OpportunityName, cmt.ParentEntryID, cmt.CreatedBy, cmt.CreatedOn, cmt.ModifiedBy, cmt.ModifiedOn, cmt.AssignedTo, 
                         cmt.IsDeletedLocally, oa.EntryGUID
FROM            OpportunitiesAdditionalTable AS oat INNER JOIN
                         ContactMainTable AS cmt ON oat.ContactServiceID = cmt.ContactServiceID LEFT OUTER JOIN
                         ContactLeadInfoTable AS clt ON oat.ContactServiceID = clt.ContactServiceID LEFT OUTER JOIN
                         ContactNamesTable AS cnt ON cmt.ContactServiceID = cnt.ContactServiceID LEFT OUTER JOIN
                             (SELECT        ContactServiceID, EntryGUID, ISNULL(SUM(ExtendedAmount), 0) AS OpportunityTotal, ISNULL(SUM(ProductExpectedRevenue), 0) AS OpportunityExpectedRevenue, ISNULL(SUM(ExtendedDiscountAmount), 0) 
                                                         AS OpportunityTotalDiscount
                               FROM            OpportunityProductsFullView
                               GROUP BY ContactServiceID, EntryGUID) AS oa ON oat.ContactServiceID = oa.ContactServiceID
WHERE        (cmt.IsDeletedLocally = 0)