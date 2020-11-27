using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class MinConfidenceOverrideBusiness : BusinessParent
    {
        MinConfidenceOverrideRepository rep;
        public MinConfidenceOverrideBusiness(string connectionString) : base(connectionString) { rep = new MinConfidenceOverrideRepository(Connection); }


        public List<MinConfidenceOverrideEntity> GetAllMinConfidenceOverrideListPaging(string LOBTag, int SortOrder, int PageNumber, int PageSize, out int TotalRecords)
        {
            return rep.GetAllMinConfidenceOverrideListPaging(LOBTag, SortOrder, PageNumber, PageSize, out TotalRecords);
        }
        public MinConfidenceOverrideEntity GetAllMinConfidenceOverrideByID(int MinCCId)
        {
            return rep.GetAllMinConfidenceOverrideByID(MinCCId);
        }
        public void InsertOrUpdateMinCCOverride(MinConfidenceOverrideEntity objMinCC)
        {
            rep.InsertOrUpdateMinCCOverride(objMinCC);
        }
        public void DeleteMinCCOverride(int Id)
        {
            rep.DeleteMinCCOverride(Id);
        }
    }
}
