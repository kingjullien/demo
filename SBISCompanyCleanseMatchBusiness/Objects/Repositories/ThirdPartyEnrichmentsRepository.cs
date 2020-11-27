using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class ThirdPartyEnrichmentsRepository : RepositoryParent
    {
        public ThirdPartyEnrichmentsRepository(string connectionString) : base(connectionString) { }

        internal List<ThirdPartyEnrichmentsEntity> GetThirdPartyEnrichments()
        {
            List<ThirdPartyEnrichmentsEntity> results = new List<ThirdPartyEnrichmentsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetThirdPartyEnrichments";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = CommonConvertMethods.ConvertDataTable<ThirdPartyEnrichmentsEntity>(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal ThirdPartyEnrichmentsEntity GetThirdPartyEnrichmentsByEnrichmentId(int enrichId)
        {
            ThirdPartyEnrichmentsEntity thirdPartyEnrichments = new ThirdPartyEnrichmentsEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetThirdPartyEnrichmentsByEnrichmentId";
                sproc.StoredProceduresParameter.Add(GetParam("@EnrichmentId", enrichId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        thirdPartyEnrichments = CommonConvertMethods.GetItem<ThirdPartyEnrichmentsEntity>(dr);
                    }
                }
                return thirdPartyEnrichments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void UpsertThirdPartyEnrichments(ThirdPartyEnrichmentsEntity obj, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpsertThirdPartyEnrichments";
                sproc.StoredProceduresParameter.Add(GetParam("@EnrichmentId", obj.EnrichmentId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", obj.CredentialId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnrichmentDescription", obj.EnrichmentDescription, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnrichmentURL", obj.EnrichmentURL.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(obj.Tags) ? obj.Tags : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", obj.CountryGroupId > 0 ? obj.CountryGroupId.ToString() : null, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnablePeriodicRefresh", obj.EnablePeriodicRefresh.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PeriodicRefreshIntervalDays", obj.PeriodicRefreshIntervalDays.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void DeleteThirdPartyEnrichmentsByEnrichmentId(int enrichId, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteThirdPartyEnrichmentByEnrichId";
                sproc.StoredProceduresParameter.Add(GetParam("@EnrichmentId", enrichId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "Other Methods"
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            return param;
        }
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
