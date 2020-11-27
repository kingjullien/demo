using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class TicketBusiness : BusinessParent
    {
        TicketRepository rep;
        public TicketBusiness(string connectionString) : base(connectionString) { rep = new TicketRepository(Connection); }
        #region "Ticket Related Method"
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public int InsertUpdateTicketForClient(TicketEntity obj)
        {
            return rep.InsertUpdateTicketForClient(obj);
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public int InsertUpdateTicketForMaster(TicketEntity obj)
        {
            return rep.InsertUpdateTicketForMaster(obj);
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public int UpdateTicketFileForClients(TicketEntity obj)
        {
            return rep.UpdateTicketFileForClients(obj);
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public int UpdateTicketFileForMaster(TicketEntity obj)
        {
            return rep.UpdateTicketFileForMaster(obj);
        }
        // Get Ticket List
        public List<TicketEntity> GetTicketList(int SortOrder, int PageNumber, int PageSize, out int TotalRecords, int? Priority, string CurrentStatus, string Title, string Host, string TicketType, string AssignedTo)
        {
            List<TicketEntity> results = new List<TicketEntity>();
            results = rep.GetTicketList(SortOrder, PageNumber, PageSize, out TotalRecords, Priority, CurrentStatus, Title, Host, TicketType, AssignedTo);
            return results;
        }
        public List<TicketEntity> GetTicketListByUser(string ClientInformation, string EnteredBy, int SortOrder, int PageNumber, int PageSize, out int TotalRecords)
        {
            List<TicketEntity> results = new List<TicketEntity>();
            results = rep.GetTicketListByUser(ClientInformation, EnteredBy, SortOrder, PageNumber, PageSize, out TotalRecords);
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public TicketEntity GetTicketByIDByClients(int Id)
        {
            TicketEntity results = new TicketEntity();
            results = rep.GetTicketByIDByClients(Id);
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public TicketEntity GetTicketByIDByMaster(int Id)
        {
            TicketEntity results = new TicketEntity();
            results = rep.GetTicketByIDByMaster(Id);
            return results;
        }
        public List<TicketEntity> GetTicketListByAssignedTo(int AssignedTo)
        {
            List<TicketEntity> results = new List<TicketEntity>();
            results = rep.GetTicketListByAssignedTo(AssignedTo);
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public List<TicketStatus> GetAttributeTypeForTicketForClients(string Code)
        {
            List<TicketStatus> results = new List<TicketStatus>();
            results = rep.GetAttributeTypeForTicketForClients(Code);
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public List<TicketStatus> GetAttributeTypeForTicketForMaster(string Code)
        {
            List<TicketStatus> results = new List<TicketStatus>();
            results = rep.GetAttributeTypeForTicketForMaster(Code);
            return results;
        }
        #endregion
        #region "Ticket Audit Related Method"
        public void InsertUpdateTicketAuditForClient(TicketAuditEntity obj, string PrimaryEmailAddress, string PrimaryContactNumber, string Files, string Title, int TicketType)
        {
            TicketRepository rep = new TicketRepository(Connection);
            rep.InsertUpdateTicketAuditForClient(obj, PrimaryEmailAddress, PrimaryContactNumber, Files, Title, TicketType);
        }
        public void InsertUpdateTicketAuditForMaster(TicketAuditEntity obj, string PrimaryEmailAddress, string PrimaryContactNumber, string Files, string Title, int TicketType)
        {
            TicketRepository rep = new TicketRepository(Connection);
            rep.InsertUpdateTicketAuditForMaster(obj, PrimaryEmailAddress, PrimaryContactNumber, Files, Title, TicketType);
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public List<TicketAuditEntity> GetTicketAuditByTicketIdByClient(int Id)
        {
            TicketRepository rep = new TicketRepository(Connection);
            List<TicketAuditEntity> results = new List<TicketAuditEntity>();
            results = rep.GetTicketAuditByTicketIdByClient(Id);
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public List<TicketAuditEntity> GetTicketAuditByTicketIdByMaster(int Id)
        {
            TicketRepository rep = new TicketRepository(Connection);
            List<TicketAuditEntity> results = new List<TicketAuditEntity>();
            results = rep.GetTicketAuditByTicketIdByMaster(Id);
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public TicketEntity GetLastTicketAuditByTicketIdByClient(int Id)
        {
            TicketEntity results = new TicketEntity();
            results = rep.GetLastTicketAuditByTicketIdByClient(Id);
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public TicketEntity GetLastTicketAuditByTicketIdByMaster(int Id)
        {
            TicketEntity results = new TicketEntity();
            results = rep.GetLastTicketAuditByTicketIdByMaster(Id);
            return results;
        }
        #endregion

        #region "Get Application User"
        public DataTable GetApplicationUserList(string ConnectionString)
        {
            DataTable results = new DataTable();
            results = rep.GetApplicationUserList();
            return results;
        }
        #endregion
    }
}
