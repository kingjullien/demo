using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    class MasterClientsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MasterClientsEntity> Adapt(DataTable dt)
        {
            List<MasterClientsEntity> results = new List<MasterClientsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MasterClientsEntity matchCode = new MasterClientsEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public MasterClientsEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MasterClientsEntity result = new MasterClientsEntity();
            result.ClientId = SafeHelper.GetSafeint(rw["ClientId"]);
            result.ClientGUID = SafeHelper.GetSafestring(rw["ClientGUID"]);
            result.ClientName = SafeHelper.GetSafestring(rw["ClientName"]);
            result.PrimaryClientDUNSNumber = SafeHelper.GetSafestring(rw["PrimaryClientDUNSNumber"]);
            result.PrimaryContactName = SafeHelper.GetSafestring(rw["PrimaryContactName"]);
            result.PrimaryEamilAddress = SafeHelper.GetSafestring(rw["PrimaryEamilAddress"]);
            result.PrimaryContactPhone = SafeHelper.GetSafestring(rw["PrimaryContactPhone"]);
            result.SecondaryContactName = SafeHelper.GetSafestring(rw["SecondaryContactName"]);
            result.SecondaryEamilAddress = SafeHelper.GetSafestring(rw["SecondaryEamilAddress"]);
            result.SecondaryContactPhone = SafeHelper.GetSafestring(rw["SecondaryContactPhone"]);
            result.DateAdded = SafeHelper.GetSafeDateTime(rw["DateAdded"]);
            result.Active = SafeHelper.GetSafebool(rw["Active"]);
            result.CreatedByUserId = SafeHelper.GetSafeint(rw["CreatedByUserId"]);
            result.Notes = SafeHelper.GetSafestring(rw["Notes"]);
            result.UpdatedByUserId = SafeHelper.GetSafeint(rw["UpdatedByUserId"]);
            result.DateUpdated = SafeHelper.GetSafeDateTime(rw["DateUpdated"]);
            result.ClientLogo = SafeHelper.GetSafestring(rw["ClientLogo"]);
            //result.ApplicationCount = SafeHelper.GetSafeint(rw["ApplicationCount"]);

            return result;
        }
    }
}
