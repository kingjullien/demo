using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MasterClientApplicationFacade : FacadeParent
    {
        MasterClientApplicationBusiness rep;
        public MasterClientApplicationFacade(string connectionString) : base(connectionString)
        {
            try
            {
                rep = new MasterClientApplicationBusiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new MasterClientApplicationBusiness(Connection);
            }
        }
        string enctyppasswrd;



        #region Add Client Application
        public void AddClientApplication(MasterClientApplicationEntity model, int UserId)
        {
            string fullConnectionString = "Data Source=" + model.DBServerName + ";Initial Catalog=" + model.DBDatabaseName + ";Persist Security Info=True;User ID=" + model.DBUserName + ";Password=" + model.DBPasswordHash + ";";
            model.ApplicationDBConnectionStringHash = StringCipher.Encrypt(fullConnectionString, General.passPhrase);
            if (model.DBPasswordHash == string.Empty || model.DBPasswordHash == null)
            {
                enctyppasswrd = model.DBPasswordHash;
            }
            else
            {
                enctyppasswrd = StringCipher.Encrypt(model.DBPasswordHash, General.passPhrase);
            }
            rep.AddClientApplication(model, UserId, enctyppasswrd, fullConnectionString);

        }
        #endregion

        public void UpdateClientApplicationSubDomain(MasterClientApplicationEntity model, int UserId)
        {
            rep.UpdateClientApplicationSubDomain(model, UserId);
        }

        public void UpdateClientApplicationDB(int ApplicationId, int ClientId, string ApplicationDBConnectionStringHash, string DBServerName, string DBDatabaseName, string DBUserName, string DBPasswordHash, int UserId)
        {
            string fullConnectionString = "Data Source=" + DBServerName + ";Initial Catalog=" + DBDatabaseName + ";Persist Security Info=True;User ID=" + DBUserName + ";Password=" + DBPasswordHash + ";";
            string fullConnectionStringEncrypt = StringCipher.Encrypt(fullConnectionString, General.passPhrase);
            if (DBPasswordHash == string.Empty || DBPasswordHash == null)
            {
                enctyppasswrd = DBPasswordHash;
            }
            else
            {
                enctyppasswrd = StringCipher.Encrypt(DBPasswordHash, General.passPhrase);
            }
            rep.UpdateClientApplicationDB(ApplicationId, ClientId, fullConnectionStringEncrypt, DBServerName, DBDatabaseName, DBUserName, enctyppasswrd, UserId, fullConnectionString);
        }

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
        public MasterClientApplicationEntity GetSolidQUser(string connctionstring)
        {
            MasterClientApplicationEntity results = new MasterClientApplicationEntity();
            //General.databaseConnectionString = null;
            //General.setConnectionString(connctionstring);
            results = rep.GetSolidQUser(StringCipher.Decrypt(connctionstring, General.passPhrase));
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
