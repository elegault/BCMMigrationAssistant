# BCM Migration Assistant
This tool to migrate data from Microsoft Outlook Business Contact Manager (BCM) to Outlook Customer Manager (Office 365) automates migration of business contacts, accounts, and opportunities to Outlook Customer Manager (OCM). 
 
Key features of the BCM Migration Tool:
 
*	Migrate default business contacts, accounts, and opportunity fields to the equivalent Outlook Customer manager fields
*	Map additional BCM fields to Outlook Customer Manager custom fields
*	Automatically migrate BCM opportunity stages and create new Outlook Customer Manager deal stages 
*	Migrate Business Notes and Phone Log history to Outlook Customer Manager's activity timeline
 
To start the migration, please use the following steps:
 
## 1.	Install BCM Migration Assistant tool
From the BCM machine (or the machine that host the SQL database for BCM), download and install tool from https://www.ericlegaultconsulting.com/BCM_Migration_Tool/publish.htm

![install](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/install.png)
  
## 2.	Connect to the BCM database 
 
By default, the tool is configured to use the  local SQL server name. If you are not sure of your BCM database info, please open Outlook and navigate to File > Business Contact Manager  and look for "Manage Database" section below. For example:
 
If the BCM database name is "OCM10\MSSmallBusiness3", then use the following value in the tool:

SQL Server: OCM10\MSSMLBIZ

Database Name: MSSmallBusiness3

![configure](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/configure.png)

After you check the information, click "Test Database Connection" before continuing.
 
## 3.	Connect to Outlook Customer Manager
 
We recommend that you test the migration with a test Outlook Customer Manager account before running the tool while signed into your actual Office 365 production account. Outlook Customer Manager does not provide a functionality to delete migrated data, and you need to delete every migrated item manually in case you need to redo the migration.
 
To create a test account, you can sign up for a 30 day Office 365 Business Premium trial from https://go.microsoft.com/fwlink/p/?LinkID=507653 (please note that after you created an Office 365 account, it may take another 24 hours before Outlook Customer Manager appears on the test account).
 
After you are connected to the database, click the "Login to Office 365" label in the "Connect to Outlook Customer Manager" section and enter your O365 account email to connect when prompted. After you are connected, the account name will be displayed on the upper right corner.

![connect](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/connect.png)
 
## 4.	Map BCM fields to Outlook Customer Manager fields
 
For each entity type (Accounts, Business Contacts, and Opportunities), some of the default BCM fields are automatically mapped to default Outlook Customer Manager fields. 
 
Optionally, you may map additional fields to Outlook Customer Manager custom fields (note: you will need to create the custom field before you run the tool, [refer to this page](https://support.office.com/en-us/article/Customize-Outlook-Customer-Manager-e7411d12-cc64-4793-8211-eda1aaef78a3?ui=en-US&rs=en-US&ad=US) for how to add custom fields in Outlook Customer Manager). 
 
For example, see below if you want to map BCM's DoNotCall field (left column) to a custom field of the same name in Outlook Customer Manager (right column):

![map](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/map.png)
  
## 5.	Migrate to Outlook Customer Manager
 
Click "Start" to begin the migration:

![migrate](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/migrate.png)



Review the migration reports for any errors during the migration.
 
Note: the tool will skip existing records to prevent duplicates. It does not perform an update. If you make an update in BCM, you need to delete the corresponding record in Outlook Customer Manager, restart the tool and repeat the process again.
