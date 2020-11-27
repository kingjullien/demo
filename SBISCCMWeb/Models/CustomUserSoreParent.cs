using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class CustomUserSoreParent
    {
        public string _connectionString = null;

        public CustomUserSoreParent(string connectionString)
        {
            this._connectionString = connectionString;

        }

        public string Connection
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

    }
}