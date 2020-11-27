using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class OIImportDataFacade : FacadeParent
    {
        OIImportDataBusiness rep;
        public OIImportDataFacade(string connectionString) : base(connectionString) { rep = new OIImportDataBusiness(Connection); }
        public int InsertOIStgInputCompany(OIInpCompanyEntity objCompany)
        {
            return rep.InsertOIStgInputCompany(objCompany);
        }
        public int InsertOIStgInputCompanyMatchRefresh(OIInpCompanyEntityMatchRefresh objCompany)
        {
            return rep.InsertOIStgInputCompanyMatchRefresh(objCompany);
        }

        public string OIProcessDataImport(int ImportProcessId)
        {
            return rep.OIProcessDataImport(ImportProcessId);
        }
        public string OIProcessDataForEnrichment(int ImportProcessId)
        {
            return rep.OIProcessDataForEnrichment(ImportProcessId);
        }
        public DataTable GetOIStgInputCompanyColumnsName()
        {
            return rep.GetOIStgInputCompanyColumnsName();
        }
        public DataTable GetDataImportProcess()
        {
            return rep.GetDataImportProcess();
        }
    }
}
