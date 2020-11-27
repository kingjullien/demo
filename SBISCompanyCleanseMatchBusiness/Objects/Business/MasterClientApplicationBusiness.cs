using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class MasterClientApplicationBusiness : BusinessParent
    {
        MasterClientApplicationRepository rep;
        public MasterClientApplicationBusiness(string connectionString) : base(connectionString) { rep = new MasterClientApplicationRepository(Connection); }
        #region "MasterClientApplication"
        #region Add Client Application
        public void AddClientApplication(MasterClientApplicationEntity model, int UserId, string enctyppasswrd, string fullConnectionString)
        {
            rep.AddClientApplication(model, UserId, enctyppasswrd, fullConnectionString);
        }
        #endregion
        public void UpdateClientApplicationSubDomain(MasterClientApplicationEntity model, int UserId)
        {
            rep.UpdateClientApplicationSubDomain(model, UserId);
        }
        public void UpdateClientApplicationDB(int ApplicationId, int ClientId, string ApplicationDBConnectionStringHash, string DBServerName, string DBDatabaseName, string DBUserName, string DBPasswordHash, int UserId, string ConnectionString)
        {
            rep.UpdateClientApplicationDB(ApplicationId, ClientId, ApplicationDBConnectionStringHash, DBServerName, DBDatabaseName, DBUserName, DBPasswordHash, UserId, ConnectionString);
        }
        #endregion

        #region "Client And ClientApplication"
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public ClientApplicationDataEntity GetClientApplicationDataForMaster(string AppicationSubDomain)
        {
            return rep.GetClientApplicationDataForMaster(AppicationSubDomain);
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public ClientApplicationDataEntity GetClientApplicationData(string AppicationSubDomain)
        {
            return rep.GetClientApplicationData(AppicationSubDomain);
        }
        public MasterClientApplicationEntity GetClientApplicationDataById(int ApplicationId)
        {
            MasterClientApplicationEntity results = new MasterClientApplicationEntity();
            results = rep.GetClientApplicationDataById(ApplicationId);
            return results;
        }
        public MasterClientApplicationEntity GetSolidQUser(string ConnectionString)
        {
            MasterClientApplicationEntity results = new MasterClientApplicationEntity();
            results = rep.GetSolidQUser(ConnectionString);
            return results;
        }
        #region Get Client Application
        public List<MasterClientApplicationEntity> GetClientApplicationDataByClientId(int clientId)
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            results = rep.GetClientApplicationDataByClientId(clientId);
            return results;
        }
        #endregion
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public List<MasterClientApplicationEntity> GetAll()
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            results = rep.GetAll();
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public List<MasterClientApplicationEntity> GetAllForClients()
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            results = rep.GetAllForClients();
            return results;
        }
        public void CheckSubDomainExistOrNot(string dbAppicationSubDomain, string dbServerName, string dbDatabaseName, string dbUserName)
        {
            rep.CheckSubDomainExistOrNot(dbAppicationSubDomain, dbServerName, dbDatabaseName, dbUserName);

        }


        #endregion

        public void UpdateClientApplicationAPICredential(string APIKey, string APISecret, string ApplicationId)
        {
            rep.UpdateClientApplicationAPICredential(APIKey, APISecret, ApplicationId);
        }
        public void UpdateEnableDisableLicance(string url, string EnableMonitoring, string EnableInvestigation)
        {
            rep.UpdateEnableDisableLicance(url, EnableMonitoring, EnableInvestigation);
        }
        public DataTable GetMenu(int UserId)
        {
            return rep.GetMenu(UserId);
        }
        public void DeleteClientApplication(string AppicationSubDomain, int ApplicationId)
        {
            rep.DeleteClientApplication(AppicationSubDomain, ApplicationId);
        }

        #region Assign To Support Portal
        public string InsertOrMergeSupportSubDomain(int ClientId, string SupportSubDomain, string ApplicationIds)
        {
            string results = rep.InsertOrMergeSupportSubDomain(ClientId, SupportSubDomain, ApplicationIds);
            return results;
        }
        public DataTable GetClientApplicationAccessByClientId(int ClientId)
        {
            return rep.GetClientApplicationAccessByClientId(ClientId);
        }
        #endregion


        public List<LicenseEntity> GetLicense()
        {
            return rep.GetLicense();
        }
    }
}
