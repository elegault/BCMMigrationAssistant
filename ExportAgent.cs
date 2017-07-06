using System;
using System.Diagnostics;
using System.Linq;

namespace BcmMigrationTool
{
    //HIGH Add logging framework: http://dennis.bloggingabout.net/2009/07/21/quickstart-tutorial-into-enterprise-library-logging/ or http://stackoverflow.com/questions/5057567/how-to-do-logging-in-c
    internal static class ExportAgent
    {        
        internal static MSSampleBusinessEntities db;
        private static void Test()
        {
            //Loop through table rows
            var query = from a in db.ActivitiesTables orderby a.ActivityDate select a;
            foreach (var activity in query)
            {
                Debug.WriteLine(String.Format("Date: {0}", activity.ActivityDate.ToString()));
                activity.ActivityType = 2; //Edit a field value
            }
            db.SaveChanges(); //If edits were made

            //Guid ownerID;
            ////Get a specific row
            //var myActiviy = db.ActivitiesTables.FirstOrDefault(activityRecords => activityRecords.OwnerID == ownerID);
        }
    }
}
