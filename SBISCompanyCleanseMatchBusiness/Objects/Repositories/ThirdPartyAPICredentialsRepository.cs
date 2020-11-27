using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class ThirdPartyAPICredentialsRepository : RepositoryParent
    {
        public ThirdPartyAPICredentialsRepository(string connectionString) : base(connectionString) { }
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

        internal List<ThirdPartyAPICredentialsEntity> GetThirdPartyAPICredentials(string ThirdPartyProvider)
        {
            List<ThirdPartyAPICredentialsEntity> results = new List<ThirdPartyAPICredentialsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetThirdPartyAPICredentials";
                sproc.StoredProceduresParameter.Add(GetParam("@ThirdPartyProvider", ThirdPartyProvider, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ThirdPartyAPICredentialsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<GlobalThirdPartyAPICredentialsEntity> GetThirdPartyAPICredentialsForUser(int UserId)
        {
            List<GlobalThirdPartyAPICredentialsEntity> results = new List<GlobalThirdPartyAPICredentialsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetThirdPartyAPICredentialsForUser";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new GlobalThirdPartyAPICredentialsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<ThirdPartyAPICredentialsEntity> GetThirdPartyAPICredentialsForRefresh(string ThirdPartyProvider)
        {
            List<ThirdPartyAPICredentialsEntity> results = new List<ThirdPartyAPICredentialsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetThirdPartyAPICredentialsForRefresh";
                sproc.StoredProceduresParameter.Add(GetParam("@ThirdPartyProvider", ThirdPartyProvider, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ThirdPartyAPICredentialsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal ThirdPartyAPICredentialsEntity GetThirdPartyAPICredentialsById(int CredentialId)
        {
            ThirdPartyAPICredentialsEntity results = new ThirdPartyAPICredentialsEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetThirdPartyAPICredentialsById";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ThirdPartyAPICredentialsAdapter().AdaptItem(dt.Rows[0]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal string DeleteThirdPartyAPICredentials(int CredentialId, int UserId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.DeleteThirdPartyAPICredentials";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        internal string InsertUpdateThirdPartyAPICredentials(ThirdPartyAPICredentialsEntity obj, int UserId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.InsertUpdateThirdPartyAPICredentials";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", obj.CredentialId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ThirdPartyProvider", obj.ThirdPartyProvider, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialName", obj.CredentialName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APICredential", obj.APICredential, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIPassword", obj.APIPassword, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIType", obj.APIType, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }

        internal string UpdateThirdPartyAPIAuthToken(int CredentialId, string AuthToken, int Duration, string ErrorCode, string ErrorDescription)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.UpdateThirdPartyAPIAuthToken";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AuthToken", string.IsNullOrEmpty(AuthToken) ? null : AuthToken, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Duration", string.IsNullOrEmpty(AuthToken) ? "0" : Duration.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIErrorCode", string.IsNullOrEmpty(AuthToken) ? ErrorCode : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIErrorDescription", string.IsNullOrEmpty(AuthToken) ? ErrorDescription : null, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        internal string UpdateUXDefaultCredentials(string Code, string CredentialId, int UserId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.UpdateUXDefaultCredentials";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Code", Code, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        internal DataTable GetMinConfidenceSettingsListPaging(bool IsGlobal, string LOBTag)
        {
            List<ThirdPartyAPICredentialsEntity> results = new List<ThirdPartyAPICredentialsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMinConfidenceSettingsListPaging";
                sproc.StoredProceduresParameter.Add(GetParam("@IsGlobal", IsGlobal.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", LOBTag, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    results = new ThirdPartyAPICredentialsAdapter().Adapt(dt);
                //}
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal List<ThirdPartyAPICredentialsEntity> GetMinConfidenceSettingsListPage(bool IsGlobal, string LOBTag)
        {
            List<ThirdPartyAPICredentialsEntity> results = new List<ThirdPartyAPICredentialsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMinConfidenceSettingsListPaging";
                sproc.StoredProceduresParameter.Add(GetParam("@IsGlobal", IsGlobal.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", LOBTag, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ThirdPartyAPICredentialsAdapter().Adapt(dt);
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void InsertUpdateMinConfidenceSettings(int Id, int MinConfidenceCode, int MaxCandidateQty, string Tag, int CredentialId, bool IsGlobal)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertUpdateMinConfidenceSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MinConfidenceCode", MinConfidenceCode.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MaxCandidateQty", MaxCandidateQty.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Convert.ToString(Tag)) ? Convert.ToString(Tag) : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsGlobal", IsGlobal.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal ThirdPartyAPICredentialsEntity GetMinConfidenceSettingsById(int MinCCId)
        {
            ThirdPartyAPICredentialsEntity results = new ThirdPartyAPICredentialsEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMinConfidenceSettingsById";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", MinCCId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    ThirdPartyAPICredentialsAdapter adta = new ThirdPartyAPICredentialsAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = adta.AdaptItem(rw);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void DeleteMinConfidenceSettings(int Id)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.DeleteMinConfidenceSettings";
            sproc.StoredProceduresParameter.Add(GetParam("@id", Id.ToString(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        internal List<UXDefaultCredentialsEntity> GetUXDefaultCredentials()
        {
            List<UXDefaultCredentialsEntity> results = new List<UXDefaultCredentialsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetUXDefaultCredentials";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    UXDefaultCredentialsAdapter adta = new UXDefaultCredentialsAdapter();
                    results = new UXDefaultCredentialsAdapter().Adapt(dt);
                    foreach (UXDefaultCredentialsEntity comp in results)
                    {
                        results = new UXDefaultCredentialsAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<ThirdPartyAPIForEnrichmentEntity> GetThirdPartyAPICredentialsForEhrichment(string ThirdPartyProvider)
        {
            List<ThirdPartyAPIForEnrichmentEntity> results = new List<ThirdPartyAPIForEnrichmentEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetThirdPartyAPICredentials";
                sproc.StoredProceduresParameter.Add(GetParam("@ThirdPartyProvider", ThirdPartyProvider, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ThirdPartyAPIForEnrichmentAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<ThirdPartyAPIForEnrichmentEntity> GetAPITypeForUXDefaultUXEnrichment()
        {
            List<ThirdPartyAPIForEnrichmentEntity> results = new List<ThirdPartyAPIForEnrichmentEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetAPITypeForUXDefaultUXEnrichment";
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ThirdPartyAPIForEnrichmentAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal string UpdateUXDefaultCredentialsForEnrichment(string EnrichmentType, int DnBAPIId, int CredentialId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.UpdateUXDefaultCredentialsForEnrichment";
                sproc.StoredProceduresParameter.Add(GetParam("@EnrichmentType", EnrichmentType, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DnBAPIId", DnBAPIId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        internal List<ThirdPartyAPIForEnrichmentEntity> GetUXDefaultUXEnrichments()
        {
            List<ThirdPartyAPIForEnrichmentEntity> results = new List<ThirdPartyAPIForEnrichmentEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetUXDefaultUXEnrichment";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    ThirdPartyAPIForEnrichmentAdapter adta = new ThirdPartyAPIForEnrichmentAdapter();
                    results = new ThirdPartyAPIForEnrichmentAdapter().Adapt(dt);
                    foreach (ThirdPartyAPIForEnrichmentEntity comp in results)
                    {
                        results = new ThirdPartyAPIForEnrichmentAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal void UpsertDnBAPIEntitlements(int credId, string dnBAPIId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.UpsertDnBAPIEntitlements";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", credId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DnBAPIId", dnBAPIId, SQLServerDatatype.NvarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }
    }
}
