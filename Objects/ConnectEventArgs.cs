using System;

namespace BCM_Migration_Tool.Objects
{
    public class ConnectEventArgs : EventArgs
    {
        public string UserName { get; }
        public string Password { get; }
        public string DbName { get; }
        public string SqlInstance { get; }

        #region Fields
        public enum AuthenticationTypes
        {
            Windows,
            SQL
        }
        private string msg;
        #endregion
        #region Constructors
        public ConnectEventArgs(string s)
        {
            msg = s;
        }
        //public ConnectEventArgs()
        //{
        //    AuthenticationType = AuthenticationTypes.Windows;
        //}
        //public ConnectEventArgs(AuthenticationTypes authenticationType, string dbName, string sqlInstance)
        //{
        //    AuthenticationType = authenticationType;
        //}
        public ConnectEventArgs(AuthenticationTypes authenticationType, string userName, string password, string dbName, string sqlInstance)
        {
            AuthenticationType = authenticationType;
            UserName = userName;
            Password = password;
            DbName = dbName;
            SqlInstance = sqlInstance;
        }
        #endregion
        #region Properties
        public AuthenticationTypes AuthenticationType { get; }
        public string Message
        {
            get { return msg; }
        }
        #endregion
    }
}