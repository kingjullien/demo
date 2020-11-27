using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class MasterClientRepository : RepositoryParent
    {

        public MasterClientRepository(string connectionString) : base(connectionString) { }

        #region "MasterClients"

        #region Add Client
        internal void AddClient(string ClientName, string PrimaryClientDUNSNumber, string PrimaryContactName, string PrimaryEamilAddress, string PrimaryContactPhone, string SecondaryContactName, string SecondaryEamilAddress, string SecondaryContactPhone, int UserId, string Notes, string ClientLogo)
        {

            StoredProcedureEntity sproc = new StoredProcedureEntity();

            sproc.StoredProcedureName = "mapp.AddClient";

            sproc.StoredProceduresParameter.Add(GetParam("@ClientName", ClientName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryClientDUNSNumber", Convert.ToString(PrimaryClientDUNSNumber), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactName", PrimaryContactName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryEamilAddress", PrimaryEamilAddress.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactPhone", PrimaryContactPhone.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecondaryContactName", Convert.ToString(SecondaryContactName), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecondaryEamilAddress", Convert.ToString(SecondaryEamilAddress), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecondaryContactPhone", Convert.ToString(SecondaryContactPhone), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Notes", Convert.ToString(Notes), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ClientLogo", Convert.ToString(ClientLogo), SQLServerDatatype.VarcharDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        #endregion

        internal void InsertUpdateDnBMeteringClient(int ClientId, string ClientGUID, string ClientName, string PrimaryContactName, string PrimaryEamilAddress, bool Active, string Notes)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "auth.InsertUpdateClient";
            sproc.StoredProceduresParameter.Add(GetParam("@ClientId", Convert.ToString(ClientId), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ClientGUID", ClientGUID.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ClientName", ClientName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactName", PrimaryContactName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryEmailAddress", PrimaryEamilAddress.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Active", Active.ToString().Trim(), SQLServerDatatype.BitDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Notes", Convert.ToString(Notes), SQLServerDatatype.VarcharDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        #region Updates Client
        internal void UpdateClient(int ClientId, string ClientName, string PrimaryClientDUNSNumber, string PrimaryContactName, string PrimaryEamilAddress, string PrimaryContactPhone, string SecondaryContactName, string SecondaryEamilAddress, string SecondaryContactPhone, int UserId, string Notes, bool Active, string ClientLogo)
        {

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "mapp.UpdateClient";

            sproc.StoredProceduresParameter.Add(GetParam("@ClientId", ClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ClientName", ClientName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryClientDUNSNumber", Convert.ToString(PrimaryClientDUNSNumber), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactName", PrimaryContactName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryEamilAddress", PrimaryEamilAddress.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactPhone", PrimaryContactPhone.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecondaryContactName", Convert.ToString(SecondaryContactName), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecondaryEamilAddress", Convert.ToString(SecondaryEamilAddress), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecondaryContactPhone", Convert.ToString(SecondaryContactPhone), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Notes", Convert.ToString(Notes), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@Active", Active.ToString(), SQLServerDatatype.BitDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@ClientLogo", Convert.ToString(ClientLogo), SQLServerDatatype.VarcharDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);


        }
        #endregion

        // Gets list of clients
        internal List<MasterClientsEntity> GetClientPaging(int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            List<MasterClientsEntity> results = new List<MasterClientsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientPaging";
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        #region Edit Client
        internal MasterClientsEntity GetClientById(int ClientId)
        {
            List<MasterClientsEntity> results = new List<MasterClientsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientById";
                sproc.StoredProceduresParameter.Add(GetParam("@ClientId", ClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterClientsAdapter().Adapt(dt);
                    foreach (MasterClientsEntity comp in results)
                    {
                        results = new MasterClientsAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        #endregion

        internal DataTable GetClientByClientGUID(string ClientGUID)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "auth.GetClientByClientGUID";
                sproc.StoredProceduresParameter.Add(GetParam("@ClientGUID", ClientGUID.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        #endregion

        #region "Other Methods"

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
