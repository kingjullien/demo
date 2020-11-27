using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class MonitoringBusiness : BusinessParent
    {
        MonitoringRepository rep;
        public MonitoringBusiness(string connectionString) : base(connectionString) { rep = new MonitoringRepository(Connection); }

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

        public List<MonitoringProductEntity> GetProductData()
        {
            return rep.GetProductData();
        }
        public List<MonitoringProductElementEntity> GetProductElementData(int productID)
        {
            return rep.GetProductElementData(productID);
        }

        public List<MonitoringElementConditionsEntity> GetMonitoringElementConditionsByProfileId(int Id)
        {
            return rep.GetMonitoringElementConditionsByProfileId(Id);
        }

        public List<MonitoringElementConditionsEntity> GetMonitoringElementConditionsByID(int Id)
        {
            return rep.GetMonitoringElementConditionsByID(Id);
        }

        public List<MonitoringProfileEntity> GetMonitoringProfile(int CredentialId)
        {
            return rep.GetMonitoringProfile(CredentialId);
        }
        public List<MonitoringProfileEntity> GetAllMonitoringProfile(int CredentialId)
        {
            return rep.GetAllMonitoringProfile(CredentialId);
        }
        public List<MonitoringProfileEntity> ValidateMonitoringProfile(int ElementId, string ProductCode, string MonitoringLevel, int Id = 0)
        {
            return rep.ValidateMonitoringProfile(ElementId, ProductCode, MonitoringLevel, Id);
        }
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
