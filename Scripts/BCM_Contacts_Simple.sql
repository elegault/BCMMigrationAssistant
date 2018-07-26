/*
Use this query to get the list of business contact, its OOB field values and link to company
*/
SELECT 
      CFV.[Type]
	  , CFV.[FirstName]					-- [OCM] First Name
      , CFV.[LastName]					-- [OCM] Last Name
      , CFV.[MiddleName]				-- [OCM] Middle Name
      , CFV.[FullName]					-- [OCM] Full Name
	   , AFV.[FullName]	As CompanyName	-- [OCM] Link to Company Name *** (changed mapping)
  FROM [dbo].[ContactFullView] CFV
  LEFT JOIN dbo.AccountsFullView AFV on AFV.EntryGUID = CFV.ParentEntryID
  WHERE CFV.IsDeletedLocally = 0
  AND CFV.Type=1 -- 1= Business Contact, 2 = Accounts, 3 = Opportunities