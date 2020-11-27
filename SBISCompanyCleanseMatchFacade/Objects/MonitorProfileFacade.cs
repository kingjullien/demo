using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MonitorProfileFacade : FacadeParent
    {
        MonitoringBusiness rep;
        public MonitorProfileFacade(string connectionString) : base(connectionString) { rep = new MonitoringBusiness(Connection); }

        public int InsertUpdateMonitoringProfile(MonitoringProfileEntity obj)
        {
            return rep.InsertUpdateMonitoringProfile(obj);
        }

        public int UpdateMonitorProfile(MonitoringProfileEntity obj)
        {
            return rep.UpdateMonitorProfile(obj);
        }

        public int InsertMonitorProfileElementCondition(MonitoringElementConditionsEntity obj)
        {
            return rep.InsertMonitorProfileElementCondition(obj);
        }

        public void DeleteMonitoringElementConditions(int MonitoringConditionID)
        {
            rep.DeleteMonitoringElementConditions(MonitoringConditionID);
        }


        public List<MonitoringProductEntity> getProductList()
        {
            List<MonitoringProductEntity> results = new List<MonitoringProductEntity>();

            results = rep.GetProductData();
            return results;
        }


        public List<MonitoringProductElementEntity> GetProductElementData(int productID)
        {
            List<MonitoringProductElementEntity> results = new List<MonitoringProductElementEntity>();

            results = rep.GetProductElementData(productID);
            return results;
        }

        public List<MonitoringElementConditionsEntity> GetMonitoringElementConditionsByProfileId(int Id)
        {
            return rep.GetMonitoringElementConditionsByProfileId(Id);
        }


        public List<MonitoringElementConditionsEntity> GetMonitoringElementConditionsByID(int Id)
        {
            return rep.GetMonitoringElementConditionsByID(Id);
        }
        // Gets Monitoring Profile
        public List<MonitoringProfileEntity> GetMonitoringProfile(int CredentialId)
        {
            return rep.GetMonitoringProfile(CredentialId);
        }
        public List<MonitoringProfileEntity> GetAllMonitoringProfile(int CredentialId)
        {
            return rep.GetAllMonitoringProfile(CredentialId);
        }
        // Validates Monitoring Profile
        public List<MonitoringProfileEntity> ValidateMonitoringProfile(int ElementId, string ProductCode, string MonitoringLevel, int Id = 0)
        {
            return rep.ValidateMonitoringProfile(ElementId, ProductCode, MonitoringLevel, Id);
        }
        // Deletes Monitoring Profile
        public void DeleteMonitoringProfile(int ID)
        {
            rep.DeleteMonitoringProfile(ID);
        }

        public MonitoringProfileEntity GetMonitorProfileByID(int Id)
        {
            return rep.GetMonitorProfileByID(Id);
        }
        public MonitoringProductElementEntity GetProductElementByID(int productElementID)
        {
            return rep.GetProductElementByID(productElementID);
        }
        public SqlDataReader ExportMonitoringNotification(string APIName, bool FlagExport, string ExportCategory)
        {
            return rep.ExportMonitoringNotification(APIName, FlagExport, ExportCategory);
        }
        public void FinalizeMonitoringNotificationDataExport(bool FlagExport, string APIName)
        {
            rep.FinalizeMonitoringNotificationDataExport(FlagExport, APIName);
        }
    }
}
