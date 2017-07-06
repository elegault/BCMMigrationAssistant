using System;
using System.Windows.Forms;
using TracerX;

namespace BCM_Migration_Tool
{
    static class Program
    {
        public static bool LogFileOpened = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());            
//#if DEBUG
//            if (Environment.MachineName == "PRECISION")
//            {
//                Application.Run(new frmLogin());
//                return;
//            }
//#endif

            //Application.Run(new MigrationWizard("", ""));
            InitLogging();
            Application.Run(new MigrationAssistant());
        }
        internal static bool InitLogging()
        {
            if (!LogFileOpened)
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory;

                Logger.DefaultBinaryFile.Name = "BCM_MT_Log";
                Logger.DefaultBinaryFile.Directory = filePath;
                Logger.DefaultBinaryFile.MaxSizeMb = 2; //MB
                Logger.DefaultBinaryFile.CircularStartSizeKb = 0;
                Logger.DefaultBinaryFile.CircularStartDelaySeconds = 0;
                Logger.DefaultBinaryFile.Archives = 1;
                Logger.DefaultBinaryFile.AppendIfSmallerThanMb = 0;

#if DEBUG
                Logger.Root.DebugTraceLevel = TracerX.TraceLevel.Verbose;
                Logger.StandardData.DebugTraceLevel = TracerX.TraceLevel.Verbose;
                LogFileOpened = true;

                Logger.Root.BinaryFileTraceLevel = TracerX.TraceLevel.Verbose;
                Logger.StandardData.BinaryFileTraceLevel = TracerX.TraceLevel.Verbose;

#else
                Logger.Root.TextFileTraceLevel = TracerX.TraceLevel.Off;
                Logger.Root.ConsoleTraceLevel = TracerX.TraceLevel.Off;
                Logger.Root.DebugTraceLevel = TracerX.TraceLevel.Off;

                Logger.Root.BinaryFileTraceLevel = TracerX.TraceLevel.Verbose;
                Logger.StandardData.BinaryFileTraceLevel = TracerX.TraceLevel.Verbose;
                Logger.Xml.Configure("TracerXConfig.xml");
                Logger.DefaultBinaryFile.Password = "";
#endif

                return Logger.DefaultBinaryFile.Open();
            }
            else
            {
                Logger.Root.BinaryFileTraceLevel = TracerX.TraceLevel.Off;
                return false;
            }
        }

    }
}
