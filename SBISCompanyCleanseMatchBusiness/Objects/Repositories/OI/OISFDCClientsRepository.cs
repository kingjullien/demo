using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories.OI
{

    internal class OISFDCClientsRepository : RepositoryParent
    {
        public OISFDCClientsRepository(string connectionString) : base(connectionString) { }
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


        // Adds client
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal string InsertOISFDCClientsForMaster(OISFDCClientsEntity model)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.InsertOISFDCClients";
                sproc.StoredProceduresParameter.Add(GetParam("@OrgId", model.OrgId, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyName", model.CompanyName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AddressLine1", string.IsNullOrEmpty(model.AddressLine1) ? "" : model.AddressLine1, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AddressLine2", string.IsNullOrEmpty(model.AddressLine2) ? "" : model.AddressLine2, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", string.IsNullOrEmpty(model.City) ? "" : model.City, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", string.IsNullOrEmpty(model.State) ? "" : model.State, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PostalCode", string.IsNullOrEmpty(model.PostalCode) ? "" : model.PostalCode, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Country", string.IsNullOrEmpty(model.Country) ? "" : model.Country, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseKey", string.IsNullOrEmpty(model.LicenseKey) ? "" : model.LicenseKey, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ContactName", string.IsNullOrEmpty(model.ContactName) ? "" : model.ContactName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ContactEmail", string.IsNullOrEmpty(model.ContactEmail) ? "" : model.ContactEmail, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ContactPhone", string.IsNullOrEmpty(model.ContactPhone) ? "" : model.ContactPhone, SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        // Updates client
        internal string UpdateOISFDCClients(OISFDCClientsEntity model)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "mapp.UpdateOISFDCClients";    // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@OIClientId", model.OIClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrgId", model.OrgId, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StartDate", model.StartDate.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EndDate", model.EndDate.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyName", model.CompanyName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AddressLine1", string.IsNullOrEmpty(model.AddressLine1) ? "" : model.AddressLine1, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AddressLine2", string.IsNullOrEmpty(model.AddressLine2) ? "" : model.AddressLine2, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", string.IsNullOrEmpty(model.City) ? "" : model.City, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", string.IsNullOrEmpty(model.State) ? "" : model.State, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PostalCode", string.IsNullOrEmpty(model.PostalCode) ? "" : model.PostalCode, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Country", string.IsNullOrEmpty(model.Country) ? "" : model.Country, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseType", string.IsNullOrEmpty(model.LicenseType) ? "" : model.LicenseType, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseKey", string.IsNullOrEmpty(model.LicenseKey) ? "" : model.LicenseKey, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ContactName", string.IsNullOrEmpty(model.ContactName) ? "" : model.ContactName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ContactEmail", string.IsNullOrEmpty(model.ContactEmail) ? "" : model.ContactEmail, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ContactPhone", string.IsNullOrEmpty(model.ContactPhone) ? "" : model.ContactPhone, SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        // Gets list of all OISFDCClients
        public List<OISFDCClientsEntity> GetOISFDCClientsByOIClientId()
        {
            List<OISFDCClientsEntity> results = new List<OISFDCClientsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetOISFDCClientsByOIClientId";   // MP-846 Admin database cleanup and code cleanup.
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = (from DataRow dr in dt.Rows
                               select new OISFDCClientsEntity()
                               {
                                   OIClientId = Convert.ToInt32(dr["OIClientId"]),
                                   OrgId = dr["OrgId"].ToString(),
                                   LicenseType = dr["LicenseType"].ToString(),
                                   StartDate = Convert.ToDateTime(dr["StartDate"]),
                                   EndDate = Convert.ToDateTime(dr["EndDate"]),
                                   CompanyName = dr["CompanyName"].ToString(),
                                   AddressLine1 = dr["AddressLine1"].ToString(),
                                   AddressLine2 = dr["AddressLine2"].ToString(),
                                   City = dr["City"].ToString(),
                                   State = dr["State"].ToString(),
                                   PostalCode = dr["PostalCode"].ToString(),
                                   Country = dr["Country"].ToString(),
                                   LicenseKey = dr["LicenseKey"].ToString(),
                                   ContactName = dr["ContactName"].ToString(),
                                   ContactEmail = dr["ContactEmail"].ToString(),
                                   ContactPhone = dr["ContactPhone"].ToString(),
                                   VerifiedDate = Convert.ToDateTime(dr["VerifiedDate"])
                               }).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        // Get all client details to Updates client
        internal OISFDCClientsEntity GetOISFDCClientsByOIClientIdforUpdate(int OIClientId)
        {
            List<OISFDCClientsEntity> results = new List<OISFDCClientsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetOISFDCClientsByOIClientId";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@OIClientId", OIClientId.ToString().Trim(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new OISFDCClientsAdapter().Adapt(dt);
                    foreach (OISFDCClientsEntity comp in results)
                    {
                        results = new OISFDCClientsAdapter().Adapt(dt);
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
        // Deletes client
        internal string DeleteOISFDCClients(string OrgId)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "mapp.DeleteOISFDCClients";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@OrgId", OrgId, SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
    }
}
