using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MonitoringProductAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        /// <summary>
        /// Product data
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<MonitoringProductEntity> Adapt(DataTable dt)
        {
            List<MonitoringProductEntity> results = new List<MonitoringProductEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MonitoringProductEntity productDataEntity = new MonitoringProductEntity();
                productDataEntity = AdaptItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }

        public MonitoringProductEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MonitoringProductEntity result = new MonitoringProductEntity();
            result.ProductID = SafeHelper.GetSafeint(rw["ProductID"]);
            if (dt.Columns.Contains("ProductCode"))
            {
                result.ProductCode = SafeHelper.GetSafestring(rw["ProductCode"]);
            }

            if (dt.Columns.Contains("ProductDescription"))
            {
                result.ProductDescription = SafeHelper.GetSafestring(rw["ProductDescription"]);
            }

            if (dt.Columns.Contains("IsActive"))
            {
                result.IsActive = SafeHelper.GetSafebool(rw["IsActive"]);
            }

            return result;
        }

        /// <summary>
        /// Business Element
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<MonitoringProductElementEntity> AdaptElement(DataTable dt)
        {
            List<MonitoringProductElementEntity> results = new List<MonitoringProductElementEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MonitoringProductElementEntity productDataEntity = new MonitoringProductElementEntity();
                productDataEntity = AdaptElementItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }
        public MonitoringProductElementEntity AdaptElementItem(DataRow rw, DataTable dt)
        {
            MonitoringProductElementEntity result = new MonitoringProductElementEntity();
            result.ProductElementID = SafeHelper.GetSafeint(rw["ProductElementID"]);
            if (dt.Columns.Contains("ElementName"))
            {
                result.ElementName = SafeHelper.GetSafestring(rw["ElementName"]);
            }

            if (dt.Columns.Contains("ElementName"))
            {
                result.ElementPCMXPath = SafeHelper.GetSafestring(rw["ElementPCMXPath"]);
            }

            if (dt.Columns.Contains("ElementType"))
            {
                result.ElementType = SafeHelper.GetSafestring(rw["ElementType"]);
            }

            if (dt.Columns.Contains("ProductElementID"))
            {
                result.ProductElementID = SafeHelper.GetSafeint(rw["ProductElementID"]);
            }

            if (dt.Columns.Contains("IsActive"))
            {
                result.IsActive = SafeHelper.GetSafebool(rw["IsActive"]);
            }

            if (dt.Columns.Contains("IsAggregate"))
            {
                result.IsAggregate = SafeHelper.GetSafebool(rw["IsAggregate"]);
            }

            if (dt.Columns.Contains("MontoringType"))
            {
                result.MontoringType = SafeHelper.GetSafestring(rw["MontoringType"]);
            }

            result.ElementTypeWithId = SafeHelper.GetSafeint(rw["ProductElementID"]) + "@#$" + SafeHelper.GetSafestring(rw["ElementType"]);
            return result;
        }


        /// <summary>
        /// Monitoring Profile
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<MonitoringProfileEntity> AdaptProfile(DataTable dt)
        {
            List<MonitoringProfileEntity> results = new List<MonitoringProfileEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MonitoringProfileEntity profileDataEntity = new MonitoringProfileEntity();
                profileDataEntity = AdaptprofileItem(rw, dt);
                results.Add(profileDataEntity);
            }
            return results;
        }

        public MonitoringProfileEntity AdaptprofileItem(DataRow rw, DataTable dt)
        {
            MonitoringProfileEntity result = new MonitoringProfileEntity();

            if (dt.Columns.Contains("ProfileName"))
            {
                result.ProfileName = SafeHelper.GetSafestring(rw["ProfileName"]);
            }

            if (dt.Columns.Contains("ProfileDescription"))
            {
                result.ProfileDescription = SafeHelper.GetSafestring(rw["ProfileDescription"]);
            }

            if (dt.Columns.Contains("ProductCode"))
            {
                result.ProductCode = SafeHelper.GetSafestring(rw["ProductCode"]);
            }

            if (dt.Columns.Contains("ProductID"))
            {
                result.ProductID = SafeHelper.GetSafeint(rw["ProductID"]);
            }

            if (dt.Columns.Contains("MonitoringLevel"))
            {
                result.MonitoringLevel = SafeHelper.GetSafestring(rw["MonitoringLevel"]);
            }

            if (dt.Columns.Contains("ApplicationTransactionID"))
            {
                result.ApplicationTransactionID = SafeHelper.GetSafeint(rw["ApplicationTransactionID"]);
            }

            if (dt.Columns.Contains("TransactionTimestamp"))
            {
                result.TransactionTimestamp = SafeHelper.GetSafeDateTime(rw["TransactionTimestamp"]);
            }

            if (dt.Columns.Contains("CustomerReferenceText"))
            {
                result.CustomerReferenceText = SafeHelper.GetSafestring(rw["CustomerReferenceText"]);
            }

            if (dt.Columns.Contains("ResultID"))
            {
                result.ResultID = SafeHelper.GetSafestring(rw["ResultID"]);
            }

            if (dt.Columns.Contains("SeverityText"))
            {
                result.SeverityText = SafeHelper.GetSafestring(rw["SeverityText"]);
            }

            if (dt.Columns.Contains("ResultText"))
            {
                result.ResultText = SafeHelper.GetSafestring(rw["ResultText"]);
            }

            if (dt.Columns.Contains("MonitoringProfileID"))
            {
                result.MonitoringProfileID = SafeHelper.GetSafeint(rw["MonitoringProfileID"]);
            }

            if (dt.Columns.Contains("RequestDateTime"))
            {
                result.RequestDateTime = SafeHelper.GetSafeDateTime(rw["RequestDateTime"]);
            }

            if (dt.Columns.Contains("ResponseDateTime"))
            {
                result.ResponseDateTime = SafeHelper.GetSafeDateTime(rw["ResponseDateTime"]);
            }

            if (dt.Columns.Contains("CreatedBy"))
            {
                result.CreatedBy = SafeHelper.GetSafeint(rw["CreatedBy"]);
            }

            if (dt.Columns.Contains("ModifiedBy"))
            {
                result.CreatedBy = SafeHelper.GetSafeint(rw["ModifiedBy"]);
            }

            if (dt.Columns.Contains("CreatedDateTime"))
            {
                result.CreatedDateTime = SafeHelper.GetSafeDateTime(rw["CreatedDateTime"]);
            }

            if (dt.Columns.Contains("ModifiedDateTime"))
            {
                result.ModifiedDateTime = SafeHelper.GetSafeDateTime(rw["ModifiedDateTime"]);
            }

            if (dt.Columns.Contains("ElementName"))
            {
                result.ElementName = SafeHelper.GetSafestring(rw["ElementName"]);
            }

            if (dt.Columns.Contains("Condition"))
            {
                result.Condition = SafeHelper.GetSafestring(rw["Condition"]);
            }

            if (dt.Columns.Contains("SingleCondition"))
            {
                result.SingleCondition = SafeHelper.GetSafebool(rw["SingleCondition"]);
            }

            if (dt.Columns.Contains("CredentialId"))
            {
                result.CredentialId = SafeHelper.GetSafeint(rw["CredentialId"]);
            }

            return result;
        }



        /// <summary>
        ///Monitoring Element Conditions
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<MonitoringElementConditionsEntity> AdaptElementConditions(DataTable dt)
        {
            List<MonitoringElementConditionsEntity> results = new List<MonitoringElementConditionsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MonitoringElementConditionsEntity profileDataEntity = new MonitoringElementConditionsEntity();
                profileDataEntity = AdaptElementConditionsItem(rw, dt);
                results.Add(profileDataEntity);
            }
            return results;
        }
        public MonitoringElementConditionsEntity AdaptElementConditionsItem(DataRow rw, DataTable dt)
        {
            MonitoringElementConditionsEntity result = new MonitoringElementConditionsEntity();
            result.MonitoringConditionID = SafeHelper.GetSafeint(rw["MonitoringConditionID"]);
            if (dt.Columns.Contains("ProfileID"))
            {
                result.ProfileID = SafeHelper.GetSafeint(rw["ProfileID"]);
            }

            if (dt.Columns.Contains("ProductElementID"))
            {
                result.ProductElementID = SafeHelper.GetSafeint(rw["ProductElementID"]);
            }

            if (dt.Columns.Contains("ElementName"))
            {
                result.ElementName = SafeHelper.GetSafestring(rw["ElementName"]);
            }

            if (dt.Columns.Contains("ChangeCondition"))
            {
                result.ChangeCondition = SafeHelper.GetSafestring(rw["ChangeCondition"]);
            }

            if (dt.Columns.Contains("Condition"))
            {
                result.Condition = SafeHelper.GetSafestring(rw["Condition"]);
            }

            if (dt.Columns.Contains("JsonCondition"))
            {
                result.JsonCondition = SafeHelper.GetSafestring(rw["JsonCondition"]);
            }

            if (dt.Columns.Contains("ElementPCMXPath"))
            {
                result.ElementPCMXPath = SafeHelper.GetSafestring(rw["ElementPCMXPath"]);
            }

            result.TempConditionId = SafeHelper.GetSafeint(rw["MonitoringConditionID"]);
            return result;
        }
    }
}
