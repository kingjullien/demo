using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class OISettingFacade : FacadeParent
    {
        OISettingBusiness rep;
        public OISettingFacade(string connectionString) : base(connectionString) { rep = new OISettingBusiness(Connection); }

        public void UpdateOrbCredentials(OISettingEntity orbEntity, string Section)
        {
            rep.UpdateOrbCredentials(orbEntity, Section);
        }
        public void UpdateOrbBackgroundSetting(OISettingEntity orbEntity, string Section)
        {
            rep.UpdateOrbBackgroundSetting(orbEntity, Section);
        }
        public void UpdateOrbDataImportHandling(OISettingEntity orbEntity, string Section)
        {
            rep.UpdateOrbDataImportHandling(orbEntity, Section);
        }
        #region Reset Data
        public void ResetOISystemData()
        {
            rep.ResetOISystemData();
        }
        #endregion
        #region OI License
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public void UpdateOIAPILicenseForMaster(string CustomerSubDomain, string APIKey)
        {
            rep.UpdateOIAPILicenseForMaster(CustomerSubDomain, APIKey);
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public void UpdateOIAPILicenseForClients(string CustomerSubDomain, string APIKey)
        {
            rep.UpdateOIAPILicenseForClients(CustomerSubDomain, APIKey);
        }

        public DataTable GetOIAPILicense(string Domain = null)
        {
            return rep.GetOIAPILicense(Domain);
        }
        #endregion

        #region "Accept From File"
        public DataTable GetOIImportDataForAcceptColumnsName()
        {
            return rep.GetOIImportDataForAcceptColumnsName();
        }
        public string AcceptLCMDataFromImport(int UserId)
        {
            return rep.AcceptLCMDataFromImport(UserId);
        }
        public string AcceptOIMatchDataFromImport(int UserId)
        {
            return rep.AcceptOIMatchDataFromImport(UserId);
        }
        #endregion

        #region Import Data
        public DataTable GetOIImportDataColumnsName()
        {
            return rep.GetOIImportDataColumnsName();
        }
        #endregion

        #region OI Delete From Files
        public DataTable GetStgInputDataForPurgeColumnName()
        {
            return rep.GetStgInputDataForPurgeColumnName();
        }
        public string DeleteCompanyDataFromImport(int UserId)
        {
            return rep.DeleteCompanyDataFromImport(UserId);
        }
        #endregion

        public DataTable GetAllOrbTags(string LOBTag, string SecurityTags, string UserId)
        {
            return rep.GetAllOrbTags(LOBTag, SecurityTags, UserId);
        }

    }
}
