using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    class DPMFTPConfigurationRepository : RepositoryParent
    {
        public DPMFTPConfigurationRepository(string connectionString) : base(connectionString) { }
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

        internal int DPMInsertUpdateFTPConfiguration(DPMFTPConfigurationEntity obj)
        {
            int result = 0;
            try
            {
                var dict = obj.ToDictionary();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DPMInsertUpdateFTPConfiguration";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id == null ? "0" : Convert.ToString(obj.Id), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Host", Convert.ToString(obj.Host), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Port", obj.Port == null ? null : Convert.ToString(obj.Port), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Password", Convert.ToString(obj.Password), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserName", Convert.ToString(obj.UserName), SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        internal List<DPMFTPConfigurationEntity> GetDPMFTPConfiguration()
        {
            List<DPMFTPConfigurationEntity> results = new List<DPMFTPConfigurationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DPMGetFTPConfiguration";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DPMFTPConfigurationAdapter ta = new DPMFTPConfigurationAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal void DeleteDPMFTPConfiguration(int id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DPMDeleteFTPConfiguration";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", id.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal DPMFTPConfigurationEntity GetDPMFTPConfigurationById(int Id)
        {
            DPMFTPConfigurationEntity results = new DPMFTPConfigurationEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DPMGetFTPConfigurationById";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DPMFTPConfigurationAdapter ta = new DPMFTPConfigurationAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.AdaptItem(rw, dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void DPMProcessNotifications()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DPMProcessNotifications";
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
