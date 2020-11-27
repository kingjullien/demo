using SBISCompanyCleanseMatchBusiness.Objects.Business.OI;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects.OI
{
    public class OISFDCClientsFacade : FacadeParent
    {
        OISFDCClientsBusiness rep;
        public OISFDCClientsFacade(string connectionString) : base(connectionString) { rep = new OISFDCClientsBusiness(Connection); }

        // Adds client
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public string InsertOISFDCClientsForMaster(OISFDCClientsEntity model)
        {
            return rep.InsertOISFDCClientsForMaster(model);
        }


        // Updates client
        public string UpdateOISFDCClients(OISFDCClientsEntity model)
        {
            return rep.UpdateOISFDCClients(model);
        }
        // Gets list of all OISFDCClients
        public List<OISFDCClientsEntity> GetOISFDCClientsByOIClientId()
        {
            return rep.GetOISFDCClientsByOIClientId();
        }
        // Get all client details to Updates client
        public OISFDCClientsEntity GetOISFDCClientsByOIClientIdforUpdate(int OIClientId)
        {
            OISFDCClientsEntity results = new OISFDCClientsEntity();
            results = rep.GetOISFDCClientsByOIClientIdforUpdate(OIClientId);
            return results;
        }
        // Deletes client
        public string DeleteOISFDCClients(string OrgId)
        {
            return rep.DeleteOISFDCClients(OrgId);
        }
    }
}
