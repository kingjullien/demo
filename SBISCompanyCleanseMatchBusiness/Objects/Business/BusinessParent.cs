namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class BusinessParent
    {
        private string _connectionString = null;
        public BusinessParent(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public string Connection
        {
            get
            {
                return _connectionString;
            }
        }
    }
}
