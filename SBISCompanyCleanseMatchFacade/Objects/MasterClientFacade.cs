using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MasterClientFacade : BusinessParent
    {
        MasterClientBusiness rep;

        public MasterClientFacade(string connectionString) : base(connectionString)
        {
            try
            {
                rep = new MasterClientBusiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new MasterClientBusiness(Connection);
            }
        }
        #region Add Client
        public void AddClient(string ClientName, string PrimaryClientDUNSNumber, string PrimaryContactName, string PrimaryEamilAddress, string PrimaryContactPhone, string SecondaryContactName, string SecondaryEamilAddress, string SecondaryContactPhone, int UserId, string Notes, string ClientLogo)
        {
            rep.AddClient(ClientName, PrimaryClientDUNSNumber, PrimaryContactName, PrimaryEamilAddress, PrimaryContactPhone, SecondaryContactName, SecondaryEamilAddress, SecondaryContactPhone, UserId, Notes, ClientLogo);
        }
        #endregion
        public void InsertUpdateDnBMeteringClient(int ClientId, string ClientGUID, string ClientName, string PrimaryContactName, string PrimaryEamilAddress, bool Active, string Notes, string ConnectionString)
        {
            rep.InsertUpdateDnBMeteringClient(ClientId, ClientGUID, ClientName, PrimaryContactName, PrimaryEamilAddress, Active, Notes, ConnectionString);
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
        // Edits client based on Id
        public MasterClientsEntity GetClientById(int clientId)
        {
            MasterClientsEntity results = new MasterClientsEntity();
            results = rep.GetClientById(clientId);
            return results;
        }
        #endregion
        // Edits client based on GUID
        public DataTable GetClientByClientGUID(string ClientGUID)
        {
            DataTable results = new DataTable();
            results = rep.GetClientByClientGUID(ClientGUID);
            return results;
        }
    }
}
