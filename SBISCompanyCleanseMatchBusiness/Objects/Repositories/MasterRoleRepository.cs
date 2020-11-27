using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    class MasterRoleRepository : RepositoryParent
    {
        public MasterRoleRepository(string ConnectionString) : base(ConnectionString) { }

        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }


        #region Role
        // Gets Role list
        internal List<MasterRoleEntity> GetRolePaging(int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            List<MasterRoleEntity> results = new List<MasterRoleEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetRolePaging";
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterRoleAdapter().Adapt(dt);
                }
                TotalCount = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal MasterRoleEntity GetRoleById(int RoleId)
        {
            MasterRoleEntity results = new MasterRoleEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetAllRoles";
                sproc.StoredProceduresParameter.Add(GetParam("@RoleId", RoleId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    MasterRoleAdapter ta = new MasterRoleAdapter();
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
        internal List<MasterRoleEntity> GetRole()
        {
            List<MasterRoleEntity> results = new List<MasterRoleEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetAllRoles";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    MasterRoleAdapter ta = new MasterRoleAdapter();
                    results = ta.Adapt(dt);

                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        // Deletes selected role
        internal void DeleteRole(int RoleId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.DeleteRole";
                sproc.StoredProceduresParameter.Add(GetParam("@RoleId", RoleId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }


        // Adds/Updates new role
        internal int InsertUpdateRole(MasterRoleEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.InsertUpdateRole";
                if (obj.RoleId > 0)
                {
                    sproc.StoredProceduresParameter.Add(GetParam("@RoleId", obj.RoleId.ToString(), SQLServerDatatype.IntDataType));
                }
                sproc.StoredProceduresParameter.Add(GetParam("@RoleName", obj.RoleName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Description", string.IsNullOrEmpty(obj.Description) ? "" : obj.Description, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsActive", obj.IsActive.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsAdd", obj.IsAdd.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsView", obj.IsView.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsEdit", obj.IsEdit.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDelete", obj.IsDelete.ToString(), SQLServerDatatype.BitDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        #endregion
    }
}
