/*  Use this query to get BCM Sales Stages (to map to OCM Deal Stages)
Note: CLosed Won and CLosed Lost may be localized in different language but the PickListValueGUID will be the same. 
*/
SELECT [PicklistValueGUID]
      ,[OrderID]
      ,[StringValue]
  FROM [dbo].[PicklistsMasterList]
  WHERE PicklistID = N'2272CC9E-F4B5-4419-B366-28B52FAB2789' -- picklist ID for Opportunity Sales Stage
  AND IsDeleted = 0
  ORDER BY OrderID