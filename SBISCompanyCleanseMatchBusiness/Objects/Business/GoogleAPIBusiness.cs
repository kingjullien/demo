using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class GoogleAPIBusiness : BusinessParent
    {
        GoogleAPIRepository rep;
        public GoogleAPIBusiness(string connectionString) : base(connectionString) { rep = new GoogleAPIRepository(Connection); }

        public GoogleAPIEntity GetGoogleAPIs()
        {
            return rep.GetGoogleAPIs();
        }
        // Inserts/Updates Google API
        public int InsertUpdateGoogleAPI(GoogleAPIEntity obj)
        {
            return rep.InsertUpdateGoogleAPI(obj);
        }
        // Gets Google API
        public GoogleAPIEntity GetGoogleAPIById(int Id)
        {
            return rep.GetGoogleAPIById(Id);
        }
        public string GetDefaultAPIKey()
        {
            return rep.GetDefaultAPIKey();
        }
    }
}