/*
Use this query to get the list of all BCM opportunities, its OOB field values and link to contact/company
*/
SELECT  OFV.[ContactServiceID]		
      ,OFV.[OpportunityType]
      ,OFV.[OpportunityStage]					-- [BCM] Deal Stage
      ,OFV.[OpportunityCloseDate]				-- [BCM] Due Date
      ,OFV.[OpportunityStatus]					
      ,OFV.[Probability]						 
      ,OFV.[OpportunityTotal]					-- [BCM] Deal Amount
      ,OFV.[OpportunityExpectedRevenue]
      ,OFV.[OpportunityTotalDiscount]
      ,OFV.[ReferredBy]
      ,OFV.[ReferredEntryId]
      ,OFV.[LeadSource]
      ,OFV.[AreaOfInterest]
      ,OFV.[Competition]
      ,OFV.[OpportunityName]					-- [BCM] Deal Name
      ,OFV.[ParentEntryID] 
      ,OFV.[AssignedTo] 
      ,OFV.[EntryGUID]							-- [BCM] Link to Business Contact or Company
	  ,CFV.FullName
	  ,CFV.Type									-- 1 = business contact, 2= accounts
  FROM [dbo].[OpportunityFullView] OFV
  JOIN [dbo].[ContactFullView] CFV ON CFV.EntryGUID = OFV.ParentEntryID
  WHERE OFV.IsDeletedLocally = 0 AND OFV.[EntryGUID] = '{0}'