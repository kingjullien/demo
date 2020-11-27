using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class OIUserMatchFilterFacade : FacadeParent
    {
        OIUserMatchFilterBusiness rep;
        public OIUserMatchFilterFacade(string connectionString) : base(connectionString) { rep = new OIUserMatchFilterBusiness(Connection); }


        public int InsertUpdateUserMatchFilter(OIUserMatchFilterEntity obj)
        {
            return rep.InsertUpdateUserMatchFilter(obj);
        }
        public void EnableDisableUserMatchFilter(int FilterId, int UserId, bool Enabled)
        {
            rep.EnableDisableUserMatchFilter(FilterId, UserId, Enabled);
        }
        public void DeleteUserMatchFilter(int FilterId, int UserId)
        {
            rep.DeleteUserMatchFilter(FilterId, UserId);
        }
        public List<OIUserMatchFilterEntity> GetUserMatchFilterList(int UserId)
        {
            return rep.GetUserMatchFilterList(UserId);
        }
        public OIUserMatchFilterEntity GetUserMatchFilterById(int UserId, int FilterId)
        {
            return rep.GetUserMatchFilterById(UserId, FilterId);
        }
    }
}
