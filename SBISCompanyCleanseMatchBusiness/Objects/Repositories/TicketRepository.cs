using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class TicketRepository : RepositoryParent
    {
        public TicketRepository(string connectionString) : base(connectionString) { }

        #region "Ticket Related Method"
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal int InsertUpdateTicketForClient(TicketEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.InsertUpdateTicket";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ClientInformation", obj.ClientInformation.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationUser", (obj.ApplicationUser != null) ? obj.ApplicationUser.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnteredBy", obj.EnteredBy.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryEmailAddress", (obj.PrimaryEmailAddress != null) ? obj.PrimaryEmailAddress.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactNumber", (obj.PrimaryContactNumber != null) ? obj.PrimaryContactNumber.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IssueDescription", (obj.IssueDescription != null) ? obj.IssueDescription.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Files", (obj.Files != null) ? obj.Files.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AssignedTo", obj.AssignedTo.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Priority", obj.Priority.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CurrentStatus", obj.CurrentStatus.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResolutionDescription", (obj.ResolutionDescription != null) ? obj.ResolutionDescription.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketSource", obj.TicketSource.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketType", obj.TicketType.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Title", obj.Title.ToString(), SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal int InsertUpdateTicketForMaster(TicketEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.InsertUpdateTicket";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ClientInformation", obj.ClientInformation.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationUser", (obj.ApplicationUser != null) ? obj.ApplicationUser.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnteredBy", obj.EnteredBy.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryEmailAddress", (obj.PrimaryEmailAddress != null) ? obj.PrimaryEmailAddress.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactNumber", (obj.PrimaryContactNumber != null) ? obj.PrimaryContactNumber.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IssueDescription", (obj.IssueDescription != null) ? obj.IssueDescription.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Files", (obj.Files != null) ? obj.Files.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AssignedTo", obj.AssignedTo.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Priority", obj.Priority.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CurrentStatus", obj.CurrentStatus.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResolutionDescription", (obj.ResolutionDescription != null) ? obj.ResolutionDescription.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketSource", obj.TicketSource.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketType", obj.TicketType.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Title", obj.Title.ToString(), SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal int UpdateTicketFileForClients(TicketEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.UpdateTicketFile";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Files", (obj.Files != null) ? obj.Files.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal int UpdateTicketFileForMaster(TicketEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateTicketFile";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Files", (obj.Files != null) ? obj.Files.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        // Get Ticket List
        internal List<TicketEntity> GetTicketList(int SortOrder, int PageNumber, int PageSize, out int TotalRecords, int? Priority, string CurrentStatus, string Title, string ClientInformation, string TicketType, string AssignedTo)
        {
            List<TicketEntity> results = new List<TicketEntity>();
            try
            {
                TotalRecords = 0;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetTicketList";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                sproc.StoredProceduresParameter.Add(GetParam("@Priority", Priority != null ? Priority.ToString() : null, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CurrentStatus", !string.IsNullOrEmpty(CurrentStatus) ? CurrentStatus.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Title", Title, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ClientInformation", !string.IsNullOrEmpty(ClientInformation) ? ClientInformation.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketType", string.IsNullOrEmpty(TicketType) ? null : TicketType.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AssignedTo", string.IsNullOrEmpty(AssignedTo) ? null : AssignedTo.ToString(), SQLServerDatatype.IntDataType));
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TicketAdapter().Adapt(dt);
                    TotalRecords = Convert.ToInt32(outParam);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<TicketEntity> GetTicketListByUser(string ClientInformation, string EnteredBy, int SortOrder, int PageNumber, int PageSize, out int TotalRecords)
        {
            TotalRecords = 0;
            List<TicketEntity> results = new List<TicketEntity>();
            try
            {

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetTicketListByUser";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ClientInformation", ClientInformation.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnteredBy", EnteredBy.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TicketAdapter().Adapt(dt);
                    TotalRecords = Convert.ToInt32(outParam);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal TicketEntity GetTicketByIDByClients(int Id)
        {
            TicketEntity results = new TicketEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetTicketByID";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    TicketAdapter ta = new TicketAdapter();
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
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal TicketEntity GetTicketByIDByMaster(int Id)
        {
            TicketEntity results = new TicketEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetTicketByID";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    TicketAdapter ta = new TicketAdapter();
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
        internal List<TicketEntity> GetTicketListByAssignedTo(int AssignedTo)
        {
            List<TicketEntity> results = new List<TicketEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "tkt.GetTicketListByAssignedTo";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", AssignedTo.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TicketAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal List<TicketStatus> GetAttributeTypeForTicketForClients(string Code)
        {
            List<TicketStatus> results = new List<TicketStatus>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetAttributeTypeForTicket";
                sproc.StoredProceduresParameter.Add(GetParam("@TypeCode", Code.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TicketAdapter().AdaptTicketUtil(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal List<TicketStatus> GetAttributeTypeForTicketForMaster(string Code)
        {
            List<TicketStatus> results = new List<TicketStatus>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetAttributeTypeForTicket";
                sproc.StoredProceduresParameter.Add(GetParam("@TypeCode", Code.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TicketAdapter().AdaptTicketUtil(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        #endregion
        #region "Ticket Audit Related Method"
        internal void InsertUpdateTicketAuditForClient(TicketAuditEntity obj, string PrimaryEmailAddress, string PrimaryContactNumber, string Files, string Title, int TicketType)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.InsertUpdateTicketAudit";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Status", obj.Status.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AssignedTo", obj.AssignedTo.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ChangedByUser", obj.ChangedByUser.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Priority", obj.Priority.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Notes", (obj.Notes != null) ? obj.Notes.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketId", obj.TicketId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryEmailAddress", PrimaryEmailAddress != null ? PrimaryEmailAddress.Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactNumber", PrimaryContactNumber != null ? PrimaryContactNumber.Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Files", Files != null ? Files.Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Title", Title != null ? Title.Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketType", TicketType.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void InsertUpdateTicketAuditForMaster(TicketAuditEntity obj, string PrimaryEmailAddress, string PrimaryContactNumber, string Files, string Title, int TicketType)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.InsertUpdateTicketAudit";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Status", obj.Status.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AssignedTo", obj.AssignedTo.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ChangedByUser", obj.ChangedByUser.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Priority", obj.Priority.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Notes", (obj.Notes != null) ? obj.Notes.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketId", obj.TicketId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryEmailAddress", PrimaryEmailAddress != null ? PrimaryEmailAddress.Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryContactNumber", PrimaryContactNumber != null ? PrimaryContactNumber.Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Files", Files != null ? Files.Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Title", Title != null ? Title.Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TicketType", TicketType.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal List<TicketAuditEntity> GetTicketAuditByTicketIdByClient(int TicketId)
        {
            List<TicketAuditEntity> results = new List<TicketAuditEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetTicketAuditByTicketId";
                sproc.StoredProceduresParameter.Add(GetParam("@TicketId", TicketId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TicketAuditAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal List<TicketAuditEntity> GetTicketAuditByTicketIdByMaster(int TicketId)
        {
            List<TicketAuditEntity> results = new List<TicketAuditEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetTicketAuditByTicketId";
                sproc.StoredProceduresParameter.Add(GetParam("@TicketId", TicketId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TicketAuditAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal TicketEntity GetLastTicketAuditByTicketIdByClient(int TicketId)
        {
            TicketEntity results = new TicketEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetLastTicketAuditByTicketId";
                sproc.StoredProceduresParameter.Add(GetParam("@TicketId", TicketId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    TicketAdapter ta = new TicketAdapter();
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
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal TicketEntity GetLastTicketAuditByTicketIdByMaster(int TicketId)
        {
            TicketEntity results = new TicketEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetLastTicketAuditByTicketId";
                sproc.StoredProceduresParameter.Add(GetParam("@TicketId", TicketId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    TicketAdapter ta = new TicketAdapter();
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
        #endregion
        #region "Comman Method"
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
        #region "Get Application User"
        internal DataTable GetApplicationUserList()
        {
            DataTable results = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserList";
                results = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        #endregion
    }
}
