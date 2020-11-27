using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    class MasterClientApplicationAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MasterClientApplicationEntity> Adapt(DataTable dt)
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MasterClientApplicationEntity matchCode = new MasterClientApplicationEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public List<MasterClientApplicationEntity> AdaptSolidQUser(DataTable dt)
        {
            List<MasterClientApplicationEntity> results = new List<MasterClientApplicationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MasterClientApplicationEntity matchCode = new MasterClientApplicationEntity();
                matchCode = AdaptItemSolidQUser(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public MasterClientApplicationEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MasterClientApplicationEntity result = new MasterClientApplicationEntity();
            result.ApplicationId = SafeHelper.GetSafeint(rw["ApplicationId"]);
            result.ClientId = SafeHelper.GetSafeint(rw["ClientId"]);
            result.AppicationSubDomain = SafeHelper.GetSafestring(rw["AppicationSubDomain"]);
            result.ApplicationDBConnectionStringHash = SafeHelper.GetSafestring(rw["ApplicationDBConnectionStringHash"]);
            result.DBServerName = SafeHelper.GetSafestring(rw["DBServerName"]);
            result.DBDatabaseName = SafeHelper.GetSafestring(rw["DBDatabaseName"]);
            result.DBUserName = SafeHelper.GetSafestring(rw["DBUserName"]);
            result.DBPasswordHash = SafeHelper.GetSafestring(rw["DBPasswordHash"]);
            result.DateAdded = SafeHelper.GetSafeDateTime(rw["DateAdded"]);
            result.Active = SafeHelper.GetSafebool(rw["Active"]);
            result.CreatedByUserId = SafeHelper.GetSafeint(rw["CreatedByUserId"]);
            result.Notes = SafeHelper.GetSafestring(rw["Notes"]);
            result.UpdatedByUserId = SafeHelper.GetSafeint(rw["UpdatedByUserId"]);
            result.DateUpdated = SafeHelper.GetSafeDateTime(rw["DateUpdated"]);

            result.LicenseSKU = SafeHelper.GetSafestring(rw["LicenseSKU"]);
            result.LicenseNumberOfUsers = SafeHelper.GetSafeint(rw["LicenseNumberOfUsers"]);
            result.LicenseNumberOfTransactions = SafeHelper.GetSafeint(rw["LicenseNumberOfTransactions"]);
            result.LicenseEnableMonitoring = SafeHelper.GetSafebool(rw["LicenseEnableMonitoring"]);
            result.LicenseEnableTags = SafeHelper.GetSafebool(rw["LicenseEnableTags"]);
            result.LicenseEnableLiveAPI = SafeHelper.GetSafebool(rw["LicenseEnableLiveAPI"]);
            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseStartDate"])))
            {
                result.LicenseStartDate = Convert.ToDateTime(rw["LicenseStartDate"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEndDate"])))
            {
                result.LicenseEndDate = Convert.ToDateTime(rw["LicenseEndDate"]);
            }

            result.LicenseEnableInvestigations = SafeHelper.GetSafebool(rw["LicenseEnableInvestigations"]);
            if (!string.IsNullOrEmpty(Convert.ToString(rw["APIKey"])))
            {
                result.APIKey = Convert.ToString(rw["APIKey"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["APISecret"])))
            {
                result.APISecret = Convert.ToString(rw["APISecret"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnableBingSearch"])))
            {
                result.LicenseEnableBingSearch = SafeHelper.GetSafebool(rw["LicenseEnableBingSearch"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseBuildAList"])))
            {
                result.LicenseBuildAList = SafeHelper.GetSafebool(rw["LicenseBuildAList"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnableGoogleMap"])))
            {
                result.LicenseEnableGoogleMap = SafeHelper.GetSafebool(rw["LicenseEnableGoogleMap"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["SAMLSSO"])))
            {
                result.SAMLSSO = SafeHelper.GetSafebool(rw["SAMLSSO"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["PartnerIdP"])))
            {
                result.PartnerIdP = SafeHelper.GetSafestring(rw["PartnerIdP"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnableAdvancedMatch"])))
            {
                result.LicenseEnableAdvancedMatch = SafeHelper.GetSafebool(rw["LicenseEnableAdvancedMatch"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnableCommandLine"])))
            {
                result.LicenseEnableCommandLine = SafeHelper.GetSafebool(rw["LicenseEnableCommandLine"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnabledOrb"])))
            {
                result.LicenseEnabledOrb = SafeHelper.GetSafebool(rw["LicenseEnabledOrb"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnableDPM"])))
            {
                result.LicenseEnableDPM = SafeHelper.GetSafebool(rw["LicenseEnableDPM"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnabledDNB"])))
            {
                result.LicenseEnabledDNB = SafeHelper.GetSafebool(rw["LicenseEnabledDNB"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnableFamilyTree"])))
            {
                result.LicenseEnableFamilyTree = SafeHelper.GetSafebool(rw["LicenseEnableFamilyTree"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnableDataStewardship"])))
            {
                result.LicenseEnableDataStewardship = SafeHelper.GetSafebool(rw["LicenseEnableDataStewardship"]);
            }

            if (dt.Columns.Contains("Branding"))
            {
                result.Branding = SafeHelper.GetSafestring(rw["Branding"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["LicenseEnableStubData"])))
            {
                result.LicenseEnableStubData = SafeHelper.GetSafebool(rw["LicenseEnableStubData"]);
            }

            if (rw.Table.Columns["LicenseEntitlements"] != null)
            {
                result.LicenseEntitlements = SafeHelper.GetSafestring(rw["LicenseEntitlements"]);
            }

            return result;
        }
        public MasterClientApplicationEntity AdaptItemSolidQUser(DataRow rw, DataTable dt)
        {
            MasterClientApplicationEntity result = new MasterClientApplicationEntity();
            result.ClientUserName = SafeHelper.GetSafestring(rw["UserName"]);
            //result.ClientLoginId = SafeHelper.GetSafestring(rw["LoginId"]);
            result.Email = SafeHelper.GetSafestring(rw["EmailAddress"]);
            return result;
        }
        public List<ClientApplicationDataEntity> ClientApplicationDataAdapt(DataTable dt)
        {
            List<ClientApplicationDataEntity> results = new List<ClientApplicationDataEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ClientApplicationDataEntity matchCode = new ClientApplicationDataEntity();
                matchCode = AdaptItemClientApplicationData(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public ClientApplicationDataEntity AdaptItemClientApplicationData(DataRow rw, DataTable dt)
        {
            ClientApplicationDataEntity result = new ClientApplicationDataEntity();
            if (!string.IsNullOrEmpty(Convert.ToString(rw["ApplicationDBConnectionStringHash"])))
            {
                result.ApplicationDBConnectionStringHash = SafeHelper.GetSafestring(rw["ApplicationDBConnectionStringHash"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["ClientName"])))
            {
                result.ClientName = SafeHelper.GetSafestring(rw["ClientName"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["ClientLogo"])))
            {
                result.ClientLogo = SafeHelper.GetSafestring(rw["ClientLogo"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["ApplicationId"])))
            {
                result.ApplicationId = SafeHelper.GetSafeint(rw["ApplicationId"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["SAMLSSO"])))
            {
                result.SAMLSSO = SafeHelper.GetSafebool(rw["SAMLSSO"]);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(rw["PartnerIdP"])))
            {
                result.PartnerIdP = SafeHelper.GetSafestring(rw["PartnerIdP"]);
            }

            return result;
        }
    }
}
