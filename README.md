**(You can also view this on the blog for Eric Legault Consulting Inc./Rockin' Software at http://www.ericlegaultconsulting.com/blog/?p=3044)**

# BCM Migration Assistant
The Business Contact Manager (BCM) Migration Assistant tool automates migration of business contacts, accounts, and opportunities to Outlook Customer Manager (OCM). 
 
Key features of the BCM Migration Tool:
 
*	Migrates default business contacts, accounts, and opportunity fields to the equivalent Outlook Customer Manager fields
*	Ability to map default and CUSTOM BCM fields to Outlook Customer Manager fields
*	Automatically migrates BCM opportunity stages and creates new Outlook Customer Manager deal stages 
*	Migrates Business Notes and Phone Log history to Outlook Customer Manager's activity timeline
* Automatically links imported Contacts to Companies, and Deals to Companies and Contacts

![overview](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/overview.png)

## FAQ

**Q**: Do I need Outlook 2016 or the Outlook Customer Manager add-in installed to use the tool?

**A**: No. You just need to login with the tool using an Office 365 account in a subscription that has Outlook Customer Manager enabled.

**Q**: Do I need to run the tool on a PC with BCM installed or on the BCM server?

**A**: No. You can run the tool on any PC that has access to the BCM database on your network while logged into Windows as an authorized BCM user.

**Q**: Will the tool create duplicates of existing items or will it update them?

**A**: If an OCM Company or Contact exists with the same Display Name value in the equivalent BCM Account or Contact Full Name field, they will be UPDATED. The relevant fields will be updated with non-empty values from BCM

**NOTE**: Existing Deals are currently NOT being updated. You may wish to subscribe to the [relevant issue](https://github.com/elegault/BCMMigrationAssistant/issues/2) to be notified when this logic changes.

**Q**: What happens if I run the migration again?

**A**: Previously imported items will NOT be updated, but existing items will be updated again if changes have been made in BCM.

**Q**: Is the tool free?

**A**: Yes. However, the tool is currently registered as an Office 365 application associated with **Eric Legault Consulting Inc./Rockin' Software** and hosted in that company's Office 365 tenant. This application will only work in its current state as long as it continues to be hosted in that tenant but there are **_no guarantees that the application will continue to work due to unforeseen circumstances with the application registration or the Office 365 subscription, or that it will continue to be available for installation from the URL published below_**. The code can always be changed to use a different application and tenant ID for a different Office 365 account, or compiled and distributed through other means.

## Custom Migration Services

If you would like the migration to be done for you, or customized to accommodate special business scenarios or to export to a different data format, please contact [**Rockin' Software**](https://www.rockinsoftware.rocks) by [email](mailto:eric@ericlegault.com?Subject=BCM), [web form](https://eric273.typeform.com/to/o3dYnb) or [book an appointment online](https://outlook.office365.com/owa/calendar/EricLegaultConsultingInc@ericlegault.com/bookings/).
 
To start the migration, please use the following steps:
 
## 1.	Install the BCM Migration Assistant tool
From the BCM machine (or the machine that host the SQL database for BCM), download and install the tool from https://www.ericlegaultconsulting.com/BCM_Migration_Tool/publish.htm

![install](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/install2.png)
  
## 2.	Connect to the BCM database 
 
By default, the tool is configured to use the  local SQL server name. If you are not sure of your BCM database info, please open Outlook and navigate to File > Business Contact Manager  and look for "Manage Database" section below. For example:
 
If the BCM database name is "OCM10\MSSmallBusiness3", then use the following value in the tool:

**SQL Server**: OCM10\MSSMLBIZ

**Database Name**: MSSmallBusiness3

![configure](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/configure.png)

After you check the information, click "Test Database Connection" before continuing.
 
## 3.	Connect to Outlook Customer Manager
 
We recommend that you test the migration with a test Outlook Customer Manager account before running the tool while signed into your actual Office 365 production account. Outlook Customer Manager does not provide a functionality to delete migrated data, and you need to delete every migrated item manually in case you need to redo the migration.
 
To create a test account, you can sign up for a 30 day Office 365 Business Premium trial from https://go.microsoft.com/fwlink/p/?LinkID=507653 (please note that after you created an Office 365 account, it may take another 24 hours before Outlook Customer Manager appears on the test account).
 
After you are connected to the database, click the "Login to Office 365" label in the "Connect to Outlook Customer Manager" section and enter your O365 account email to connect when prompted. After you are connected, the account name will be displayed on the upper right corner.

![connect](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/connect.png)
 
## 4.	Map BCM fields to Outlook Customer Manager fields
 
For each entity type (Accounts, Business Contacts, and Opportunities), some of the default BCM fields are automatically mapped to default Outlook Customer Manager fields. 
 
Optionally, you may map additional fields to Outlook Customer Manager custom fields (note: you will need to create the custom field before you run the tool ([refer to this page](https://techcommunity.microsoft.com/t5/Outlook-Blog/Make-Outlook-Customer-Manager-your-own/ba-p/81208) for information on how to add custom fields in Outlook Customer Manager). 
 
For example, see below if you want to map BCM's DoNotCall field (left column) to a custom field of the same name in Outlook Customer Manager (right column):

![map](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/map.png)
  
## 5.	Migrate to Outlook Customer Manager
 
Click "Start" to begin the migration:

![migrate](https://www.ericlegaultconsulting.com/BCM_Migration_Tool/git_images/migrate.png)



Review the migration reports for any errors during the migration.
