using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MinConfidenceOverrideFacade : FacadeParent
    {
        MinConfidenceOverrideBusiness rep;
        public MinConfidenceOverrideFacade(string connectionString) : base(connectionString) { rep = new MinConfidenceOverrideBusiness(Connection); }


        public List<MinConfidenceOverrideEntity> GetAllMinConfidenceOverrideListPaging(string LOBTag, int SortOrder, int PageNumber, int PageSize, out int TotalRecords)
        {
            return rep.GetAllMinConfidenceOverrideListPaging(LOBTag, SortOrder, PageNumber, PageSize, out TotalRecords);
        }
        // Gets MinConfidenceOverride
        public MinConfidenceOverrideEntity GetAllMinConfidenceOverrideByID(int MinCCId)
        {
            return rep.GetAllMinConfidenceOverrideByID(MinCCId);
        }
        // Inserts or Updates MinCCOverride
        public void InsertOrUpdateMinCCOverride(MinConfidenceOverrideEntity objMinCC)
        {
            rep.InsertOrUpdateMinCCOverride(objMinCC);
        }
        // Deletes MinCCOverride
        public void DeleteMinCCOverride(int Id)
        {
            rep.DeleteMinCCOverride(Id);
        }

    }
}
