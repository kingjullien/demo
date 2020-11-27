namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class RepositoryParent
    {
        private string _connectionString = null;
        public RepositoryParent(string connectionString)
        {
            this._connectionString = connectionString;
        }

        private SqlHelper _sql = null;
        public string Connection
        {
            get
            {
                return _connectionString;
            }
        }
        public SqlHelper sql
        {
            get
            {
                if (_sql == null)
                {
                    _sql = new SqlHelper(_connectionString);
                }

                return _sql;
            }
        }
    }
}
