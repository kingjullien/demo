using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class OIImportDataBusiness : BusinessParent
    {
        OIImportDataRepository rep;
        public OIImportDataBusiness(string connectionString) : base(connectionString) { rep = new OIImportDataRepository(Connection); }

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
