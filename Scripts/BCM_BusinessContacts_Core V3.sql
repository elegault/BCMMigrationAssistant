/*
Use this query to get the list of business contact, its OOB field values and link to company
*/
SELECT 
	   [ContactServiceID]
      ,[Type]
      ,[EntryGUID]
	  ,[FirstName]				-- [OCM] First Name
      ,[LastName]				-- [OCM] Last Name
      ,[MiddleName]				-- [OCM] Middle Name
      ,[FileAs]
      ,[FullName]				-- [OCM] Full Name
      ,[Subject]
      ,[JobTitle]				-- [OCM] Job Title
      ,[Profession]		
      ,[CompanyName]			-- [OCM] Link to Company Name
      ,[Department]
      ,[Birthday]				-- [OCM] Birthday
      ,[WeddingAnniversary]
      ,[ManagerName]
      ,[AssistantName]
      ,[LastAccessTime]
      ,[PrefContactMethod]
      ,[DoNotEmail]
      ,[DoNotFax]
      ,[DoNotCall]
      ,[DoNotSendLetter]
      ,[PrimaryContactGUID]
      ,[PaymentStatus]
      ,[Rating]
      ,[Active]
      ,[CustomerID]
      ,[Territory]
      ,[TypeOfEntity]
      ,[WebAddress]
      ,[AssignedTo]
      ,[IMAddress]
      ,[OfficeLocation]
      ,[ReferredBy]
      ,[ReferredEntryId]
      ,[LeadSource]
      ,[AreaOfInterest]
      ,[WorkAddressStreet]		-- [OCM] Business Address Street
      ,[WorkAddressPOB]			
      ,[WorkAddressCity]		-- [OCM] Business Address City
      ,[WorkAddressState]		-- [OCM] Business Address State
      ,[WorkAddressZip]			-- [OCM] Business Address Postal Code
      ,[WorkAddressCountry]		-- [OCM] Business Address Country
      ,[BusinessAddress]
      ,[PostalAddress]
      ,[Email1Address]			-- [OCM] Email Address
      ,[HomePhoneNum]			-- [OCM] Home Phone
      ,[MobilePhoneNum]			-- [OCM] Mobile Phone
      ,[CompanyMainPhoneNum]	-- [OCM] Business Phone
      ,[PrimaryPhoneNum]		
      ,[ContactNotes]
      ,[DisplaySelectors]
      ,[Nickname]
      ,[Children]
      ,[Hobby]
      ,[Spouse]
      ,[AccountNumber]
      ,[Revenue]
      ,[TickerSymbol]
      ,[Employees]
  FROM [dbo].[ContactFullView] CFV
  WHERE CFV.IsDeletedLocally = 0
  AND CFV.Type=1 -- 1= Business Contact, 2 = Accounts, 3 = Opportunities