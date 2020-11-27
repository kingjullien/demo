using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class MasterClientApplicationRepository : RepositoryParent
    {
        public MasterClientApplicationRepository(string connectionString) : base(connectionString) { }

        #region "MasterClientApplication"



        #region Add Client Application
        internal void AddClientApplication(MasterClientApplicationEntity model, int UserId, string enctyppasswrd, string ConntectionString)
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(ConntectionString);
                sqlcon.Open();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.AddClientApplication";
                sproc.StoredProceduresParameter.Add(GetParam("@ClientId", model.ClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AppicationSubDomain", model.AppicationSubDomain.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationDBConnectionStringHash", model.ApplicationDBConnectionStringHash.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DBServerName", model.DBServerName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DBDatabaseName", model.DBDatabaseName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DBUserName", model.DBUserName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DBPasswordHash", Convert.ToString(enctyppasswrd), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Notes", Convert.ToString(model.Notes), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseSKU", Convert.ToString(model.LicenseSKU), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseNumberOfUsers", Convert.ToString(model.LicenseNumberOfUsers), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseNumberOfTransactions", Convert.ToString(model.LicenseNumberOfTransactions), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseStartDate", !string.IsNullOrEmpty(Convert.ToString(model.LicenseStartDate)) ? Convert.ToString(model.LicenseStartDate) : null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseEndDate", !string.IsNullOrEmpty(Convert.ToString(model.LicenseEndDate)) ? Convert.ToString(model.LicenseEndDate) : null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIKey", model.APIKey == null ? "" : Convert.ToString(model.APIKey), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APISecret", model.APISecret == null ? "" : Convert.ToString(model.APISecret), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PartnerIdP", Convert.ToString(model.PartnerIdP), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Branding", Convert.ToString(model.Branding), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseEntitlements", model.LicenseEntitlements, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", model.ApplicationId == 0 ? null : model.ApplicationId.ToString(), SQLServerDatatype.IntDataType));


                int result = (int)sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                if (result > 0)
                {
                    AddClientDataInSolidqUser(model.ClientUserName, model.Email, ConntectionString);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        internal void AddClientDataInSolidqUser(string ClientUserName, string Email, string ConntectionString)
        {
            try
            {
                SqlHelper sqlclinet = new SqlHelper(ConntectionString);
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                bool IsUserLoginFirstTime = true;
                sproc.StoredProcedureName = "dnb.InsertUserByAdmin";
                sproc.StoredProceduresParameter.Add(GetParam("@UserName", ClientUserName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserStatusCode", "101001", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserTypeCode", "102001", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", Email.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsUserLoginFirstTime", IsUserLoginFirstTime.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableInvestigations", "false", SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableSearchByDUNS", "false", SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableCreateAutoAcceptRules", "false", SQLServerDatatype.BitDataType));
                sqlclinet.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void UpdateClientApplicationSubDomain(MasterClientApplicationEntity model, int UserId)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "mapp.AddClientApplication";
            sproc.StoredProceduresParameter.Add(GetParam("@ClientId", model.ClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@AppicationSubDomain", model.AppicationSubDomain.ToString(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ApplicationDBConnectionStringHash", model.ApplicationDBConnectionStringHash.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBServerName", "", SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBDatabaseName", "", SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBUserName", "", SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBPasswordHash", "", SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Notes", Convert.ToString(model.Notes), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@LicenseSKU", Convert.ToString(model.LicenseSKU), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@LicenseNumberOfUsers", Convert.ToString(model.LicenseNumberOfUsers), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@LicenseNumberOfTransactions", Convert.ToString(model.LicenseNumberOfTransactions), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@LicenseStartDate", !string.IsNullOrEmpty(Convert.ToString(model.LicenseStartDate)) ? Convert.ToString(model.LicenseStartDate) : null, SQLServerDatatype.DateTimeDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@LicenseEndDate", !string.IsNullOrEmpty(Convert.ToString(model.LicenseEndDate)) ? Convert.ToString(model.LicenseEndDate) : null, SQLServerDatatype.DateTimeDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@APIKey", model.APIKey == null ? "" : Convert.ToString(model.APIKey), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@APISecret", model.APISecret == null ? "" : Convert.ToString(model.APISecret), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PartnerIdP", Convert.ToString(model.PartnerIdP), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Branding", Convert.ToString(model.Branding), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@LicenseEntitlements", model.LicenseEntitlements, SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", model.ApplicationId == 0 ? null : model.ApplicationId.ToString(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        internal void UpdateClientApplicationDB(int ApplicationId, int ClientId, string ApplicationDBConnectionStringHash, string DBServerName, string DBDatabaseName, string DBUserName, string DBPasswordHash, int UserId, string ConnectionString)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "mapp.UpdateClientApplicationDBDetails";
            sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ClientId", ClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ApplicationDBConnectionStringHash", ApplicationDBConnectionStringHash.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBServerName", DBServerName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBDatabaseName", DBDatabaseName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBUserName", DBUserName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBPasswordHash", Convert.ToString(DBPasswordHash), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }



        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal ClientApplicationDataEntity GetClientApplicationDataForMaster(string AppicationSubDomain)
        {
            List<ClientApplicationDataEntity> results = new List<ClientApplicationDataEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientApplicationData";
                sproc.StoredProceduresParameter.Add(GetParam("@AppicationSubDomain", AppicationSubDomain, SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientApplicationAdapter().ClientApplicationDataAdapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal ClientApplicationDataEntity GetClientApplicationData(string AppicationSubDomain)
        {
            List<ClientApplicationDataEntity> results = new List<ClientApplicationDataEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientApplicationData";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@AppicationSubDomain", AppicationSubDomain, SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientApplicationAdapter().ClientApplicationDataAdapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal void CheckSubDomainExistOrNot(string dbAppicationSubDomain, string dbServerName, string dbDatabaseName, string dbUserName)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();

            sproc.StoredProcedureName = "mapp.GetClientApplicationDataAllSubDomain";

            sproc.StoredProceduresParameter.Add(GetParam("@AppicationSubDomain", dbAppicationSubDomain.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBServerName", dbServerName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBDatabaseName", dbDatabaseName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@DBUserName", dbUserName.ToString().Trim(), SQLServerDatatype.VarcharDataType));

            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        internal MasterClientApplicationEntity GetClientApplicationDataById(int ApplicationId)
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientApplicationDataById";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString().Trim(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientApplicationAdapter().Adapt(dt);
                    //foreach (MasterClientApplicationEntity comp in results)
                    //{
                    //    results = new MasterClientApplicationAdapter().Adapt(dt);
                    //}
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal MasterClientApplicationEntity GetSolidQUser(string ConnectionString)
        {
            SqlHelper sqlClient = new SqlHelper(ConnectionString);
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserList";

                DataTable dt;
                dt = sqlClient.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientApplicationAdapter().AdaptSolidQUser(dt);
                    foreach (MasterClientApplicationEntity comp in results)
                    {
                        results = new MasterClientApplicationAdapter().AdaptSolidQUser(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here

            }
            return results.FirstOrDefault();
        }
        #region Get Client Application
        internal List<MasterClientApplicationEntity> GetClientApplicationDataByClientId(int clientId)
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientApplicationDataByClientId";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ClientId", clientId.ToString().Trim(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientApplicationAdapter().Adapt(dt);
                    foreach (MasterClientApplicationEntity comp in results)
                    {
                        results = new MasterClientApplicationAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        #endregion
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal List<MasterClientApplicationEntity> GetAll()
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientApplicationDataAll";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientApplicationAdapter().Adapt(dt);
                    foreach (MasterClientApplicationEntity comp in results)
                    {
                        results = new MasterClientApplicationAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal List<MasterClientApplicationEntity> GetAllForClients()
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetClientApplicationDataAll";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientApplicationAdapter().Adapt(dt);
                    foreach (MasterClientApplicationEntity comp in results)
                    {
                        results = new MasterClientApplicationAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        #endregion
        internal void UpdateClientApplicationAPICredential(string APIKey, string APISecret, string ApplicationId)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "mapp.UpdateClientApplicationAPICredential";
            sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@APIKey", Convert.ToString(APIKey), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@APISecret", Convert.ToString(APISecret), SQLServerDatatype.VarcharDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        internal void UpdateEnableDisableLicance(string url, string EnableMonitoring, string EnableInvestigation)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "ApplicationId.UpdateEnableDisableLicense";
            sproc.StoredProceduresParameter.Add(GetParam("@AppicationSubDomain", url.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@LicenseEnableMonitoring", Convert.ToString(EnableMonitoring), SQLServerDatatype.BitDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@LicenseEnableInvestigations", Convert.ToString(EnableInvestigation), SQLServerDatatype.BitDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        internal void DeleteClientApplication(string AppicationSubDomain, int ApplicationId)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();

            sproc.StoredProcedureName = "mapp.DeleteClientApplication";
            sproc.StoredProceduresParameter.Add(GetParam("@AppicationSubDomain", AppicationSubDomain.ToString(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }


        internal List<LicenseEntity> GetLicense()
        {
            List<LicenseEntity> results = new List<LicenseEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetLicense";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new LicenseAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

        #region "Other Methods"

        public DataTable GetMenu(int UserId)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetMenu";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", (UserId == 0 ? null : Convert.ToString(UserId)), SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal string InsertOrMergeSupportSubDomain(int ClientId, string SupportSubDomain, string ApplicationIds)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.InsertOrMergeSupportSubDomain";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ClientId", ClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationIds", ApplicationIds.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SupportSubDomain", SupportSubDomain.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                string Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return Message;
            }
        }

        #region Assign To Support Portal
        internal DataTable GetClientApplicationAccessByClientId(int ClientId)
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientApplicationAccessByClientId";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ClientId", ClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        #endregion

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
