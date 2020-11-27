using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class DandBEnrichmentDataBusiness : BusinessParent
    {
        DandBEnrichmentDataRepository rep;

        public DandBEnrichmentDataBusiness(string connectionString) : base(connectionString) { rep = new DandBEnrichmentDataRepository(Connection); }
        public DataTable GetAPILayers()
        {
            return rep.GetAPILayers();
        }
        public List<DandBEnrichmentDataEntity> FindMetaData(string APILayer, string JSONPath)
        {
            return rep.FindMetaData(APILayer, JSONPath);
        }
        public string AddNewMappingMetaData(DandBEnrichmentDataEntity model)
        {
            return rep.AddNewMappingMetaData(model);
        }
        public string EditMappingMetaData(DandBEnrichmentDataEntity model)
        {
            return rep.EditMappingMetaData(model);
        }
        public void UpdateMapping(string APILayer, int MappingId, bool Selected)
        {
            rep.UpdateMapping(APILayer, MappingId, Selected);
        }
        //public DandBEnrichmentDataEntity FindMetadataByMappingID(int MappingId)
        //{
        //    return rep.FindMetadataByMappingID(MappingId);
        //}
    }
}
