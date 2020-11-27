using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class MasterClientBusiness : BusinessParent
    {
        MasterClientRepository rep;

        public MasterClientBusiness(string connectionString) : base(connectionString) { rep = new MasterClientRepository(Connection); }


        #region "MasterClient"

        #region Add Client
        public void AddClient(string ClientName, string PrimaryClientDUNSNumber, string PrimaryContactName, string PrimaryEamilAddress, string PrimaryContactPhone, string SecondaryContactName, string SecondaryEamilAddress, string SecondaryContactPhone, int UserId, string Notes, string ClientLogo)
        {
            rep.AddClient(ClientName, PrimaryClientDUNSNumber, PrimaryContactName, PrimaryEamilAddress, PrimaryContactPhone, SecondaryContactName, SecondaryEamilAddress, SecondaryContactPhone, UserId, Notes, ClientLogo);
        }
        #endregion

        public void InsertUpdateDnBMeteringClient(int ClientId, string ClientGUID, string ClientName, string PrimaryContactName, string PrimaryEamilAddress, bool Active, string Notes, string ConnectionString)
        {
            rep.InsertUpdateDnBMeteringClient(ClientId, ClientGUID, ClientName, PrimaryContactName, PrimaryEamilAddress, Active, Notes);
        }


        #region Updates Client
        public void UpdateClient(int ClientId, string ClientName, string PrimaryClientDUNSNumber, string PrimaryContactName, string PrimaryEamilAddress, string PrimaryContactPhone, string SecondaryContactName, string SecondaryEamilAddress, string SecondaryContactPhone, int UserId, string Notes, bool Active, string ClientLogo)
        {
            rep.UpdateClient(ClientId, ClientName, PrimaryClientDUNSNumber, PrimaryContactName, PrimaryEamilAddress, PrimaryContactPhone, SecondaryContactName, SecondaryEamilAddress, SecondaryContactPhone, UserId, Notes, Active, ClientLogo);
        }
        #endregion

        // Gets list of clients
        public List<MasterClientsEntity> GetClientPaging(int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            return rep.GetClientPaging(SortOrder, PageNumber, PageSize, out TotalCount);
        }

        #region Edit Client
        public MasterClientsEntity GetClientById(int clientId)
        {
            MasterClientsEntity results = new MasterClientsEntity();
            results = rep.GetClientById(clientId);
            return results;
        }
        #endregion

        public DataTable GetClientByClientGUID(string ClientGUID)
        {
            DataTable results = new DataTable();
            results = rep.GetClientByClientGUID(ClientGUID);
            return results;
        }
        #endregion
    }
}
