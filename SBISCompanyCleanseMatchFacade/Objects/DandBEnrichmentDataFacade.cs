using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class DandBEnrichmentDataFacade : FacadeParent
    {
        DandBEnrichmentDataBusiness rep;
        public DandBEnrichmentDataFacade(string connectionString, string UserName = "") : base(connectionString)
        {
            try
            {
                rep = new DandBEnrichmentDataBusiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new DandBEnrichmentDataBusiness(Connection);
            }
        }
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
    }
}
