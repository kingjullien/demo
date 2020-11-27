using System;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class FacadeParent
    {
        private string _connectionString = null;
        private string _userName = null;
        public FacadeParent(string connectionString, string strUserName = null)
        {
            this._connectionString = connectionString;
            this._userName = Convert.ToString(strUserName);
        }

        public string Connection
        {
            get
            {
                return _connectionString;
            }
        }
        public string UserName
        {
            get
            {
                return _userName;
            }
        }
    }
}
