using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;

namespace SBISCompanyCleanseMatchFacade.Objects
{


    public class GoogleAPIFacade : FacadeParent
    {
        GoogleAPIBusiness rep;
        public GoogleAPIFacade(string connectionString) : base(connectionString) { rep = new GoogleAPIBusiness(Connection); }
        public GoogleAPIEntity GetGoogleAPIs()
        {
            return rep.GetGoogleAPIs();
        }
        public int InsertUpdateGoogleAPI(GoogleAPIEntity obj)
        {
            return rep.InsertUpdateGoogleAPI(obj);
        }
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
