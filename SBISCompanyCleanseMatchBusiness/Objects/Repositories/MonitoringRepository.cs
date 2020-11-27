using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class MonitoringRepository : RepositoryParent
    {
        public MonitoringRepository(string connectionString) : base(connectionString) { }

        internal List<MonitoringProductEntity> GetProductData()
        {
            List<MonitoringProductEntity> results = new List<MonitoringProductEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetProductData";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }



        internal List<MonitoringProductElementEntity> GetProductElementData(int productID)
        {
            List<MonitoringProductElementEntity> results = new List<MonitoringProductElementEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetProductElementData";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@ProductID";
                param.ParameterValue = productID.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptElement(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal int InsertUpdateMonitoringProfile(MonitoringProfileEntity obj)
        {

            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertUpdateMonitoringProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@ProfileName", obj.ProfileName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProfileDescription", obj.ProfileDescription.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProductCode", obj.ProductCode.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProductID", obj.ProductID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringLevel", obj.MonitoringLevel.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationTransactionID", obj.ApplicationTransactionID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TransactionTimestamp", obj.TransactionTimestamp.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CustomerReferenceText", obj.CustomerReferenceText.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultID", obj.ResultID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SeverityText", obj.SeverityText.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultText", obj.ResultText.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringProfileID", obj.MonitoringProfileID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestDateTime", obj.RequestDateTime.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseDateTime", obj.ResponseDateTime.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CreatedBy", obj.CreatedBy.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ModifiedBy", obj.ModifiedBy.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDeleted", obj.IsDeleted.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", obj.CredentialId.ToString(), SQLServerDatatype.IntDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        internal int UpdateMonitorProfile(MonitoringProfileEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateMonitoringProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationTransactionID", obj.ApplicationTransactionID.ToString(), SQLServerDatatype.BigintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TransactionTimestamp", obj.TransactionTimestamp.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CustomerReferenceText", obj.CustomerReferenceText.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultID", obj.ResultID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SeverityText", obj.SeverityText.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultText", obj.ResultText.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringProfileID", obj.MonitoringProfileID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestDateTime", obj.RequestDateTime.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseDateTime", obj.ResponseDateTime.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ModifiedBy", obj.ModifiedBy.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDeleted", obj.IsDeleted.ToString(), SQLServerDatatype.BitDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        internal int InsertMonitorProfileElementCondition(MonitoringElementConditionsEntity obj)
        {

            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertOrUpdateMonitoringElementConditions";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringConditionID", obj.MonitoringConditionID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProfileID", obj.ProfileID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProductElementID", obj.ProductElementID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ChangeCondition", obj.ChangeCondition.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Condition", Convert.ToString(obj.Condition), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@JsonCondition", !string.IsNullOrEmpty(obj.JsonCondition) ? Convert.ToString(obj.JsonCondition) : null, SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        internal List<MonitoringElementConditionsEntity> GetMonitoringElementConditionsByProfileId(int Id)
        {
            List<MonitoringElementConditionsEntity> results = new List<MonitoringElementConditionsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMonitoringElementConditionsByProfileId";
                sproc.StoredProceduresParameter.Add(GetParam("@ProfileID", Id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptElementConditions(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<MonitoringElementConditionsEntity> GetMonitoringElementConditionsByID(int Id)
        {
            List<MonitoringElementConditionsEntity> results = new List<MonitoringElementConditionsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMonitoringElementConditionsByID";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringConditionID", Id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptElementConditions(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }



        internal void DeleteMonitoringElementConditions(int MonitoringConditionID)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteMonitoringElementConditions";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringConditionID", MonitoringConditionID.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }


        internal List<MonitoringProfileEntity> GetAllMonitoringProfile(int CredentialId)
        {
            List<MonitoringProfileEntity> results = new List<MonitoringProfileEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllMonitoringProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptProfile(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<MonitoringProfileEntity> ValidateMonitoringProfile(int ElementId, string ProductCode, string MonitoringLevel, int Id = 0)
        {
            List<MonitoringProfileEntity> results = new List<MonitoringProfileEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ValidateMonitoringProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@ProfileId", Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ElementId ", ElementId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProductCode", ProductCode.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringLevel", MonitoringLevel.ToString(), SQLServerDatatype.VarcharDataType));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptProfile(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<MonitoringProfileEntity> GetMonitoringProfile(int CredentialId)
        {
            List<MonitoringProfileEntity> results = new List<MonitoringProfileEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMonitoringProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptProfile(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }


        internal void DeleteMonitoringProfile(int Id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteMonitoringProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringProfileID", Id.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }



        internal MonitoringProfileEntity GetMonitorProfileByID(int Id)
        {
            MonitoringProfileEntity results = new MonitoringProfileEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMonitorProfileByID";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@MonitoringProfileID";
                param.ParameterValue = Id.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptProfile(dt).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal MonitoringProductElementEntity GetProductElementByID(int productElementID)
        {
            MonitoringProductElementEntity results = new MonitoringProductElementEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetProductElementByID";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@productElementID";
                param.ParameterValue = productElementID.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptElement(dt).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal SqlDataReader ExportMonitoringNotification(string APIName, bool FlagExport, string ExportCategory)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ExportMonitoringNotifications";
                sproc.StoredProceduresParameter.Add(GetParam("@APIName", APIName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportCategory", ExportCategory.ToString(), SQLServerDatatype.VarcharDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;

            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void FinalizeMonitoringNotificationDataExport(bool FlagExport, string APIName)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.FinalizeMonitoringNotificationDataExport";
            sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@APIName", APIName.ToString(), SQLServerDatatype.VarcharDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        #region "Common Method"
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
        #endregion
    }
}