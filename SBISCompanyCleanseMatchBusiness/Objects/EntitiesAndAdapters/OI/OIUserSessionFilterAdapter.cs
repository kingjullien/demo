using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI
{
    // DB Changes (MP-716)
    public class OIUserSessionFilterAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<OIUserSessionFilterEntity> Adapt(DataTable dt)
        {
            List<OIUserSessionFilterEntity> results = new List<OIUserSessionFilterEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                OIUserSessionFilterEntity session = new OIUserSessionFilterEntity();
                session = AdaptItem(rw, dt);
                results.Add(session);
            }
            return results;
        }

        public OIUserSessionFilterEntity AdaptItem(DataRow rw, DataTable dt)
        {
            OIUserSessionFilterEntity result = new OIUserSessionFilterEntity();
            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (dt.Columns.Contains("SrcRecordId"))
            {
                result.SrcRecordId = SafeHelper.GetSafestring(rw["SrcRecordId"]);
            }

            if (dt.Columns.Contains("CompanyName"))
            {
                result.CompanyName = SafeHelper.GetSafestring(rw["CompanyName"]);
            }

            if (dt.Columns.Contains("City"))
            {
                result.City = SafeHelper.GetSafestring(rw["City"]);
            }

            if (dt.Columns.Contains("State"))
            {
                result.State = SafeHelper.GetSafestring(rw["State"]);
            }

            if (dt.Columns.Contains("CountryISOAlpha2Code"))
            {
                result.CountryISOAlpha2Code = SafeHelper.GetSafestring(rw["CountryISOAlpha2Code"]);
            }

            if (dt.Columns.Contains("CountryISOAlpha2Code"))
            {
                result.CountryISOAlpha2Code = SafeHelper.GetSafestring(rw["CountryISOAlpha2Code"]);
            }

            if (dt.Columns.Contains("OrderByColumn"))
            {
                result.OrderByColumn = SafeHelper.GetSafestring(rw["OrderByColumn"]);
            }

            if (dt.Columns.Contains("FilterText"))
            {
                result.FilterText = SafeHelper.GetSafestring(rw["FilterText"]);
            }

            if (dt.Columns.Contains("FilterExists"))
            {
                result.FilterExists = SafeHelper.GetSafebool(rw["FilterExists"]);
            }

            if (dt.Columns.Contains("Tag"))
            {
                result.Tag = SafeHelper.GetSafestring(rw["Tag"]);
            }

            if (dt.Columns.Contains("CountryGroupId"))
            {
                result.CountryGroupId = SafeHelper.GetSafeint(rw["CountryGroupId"]);
            }

            if (dt.Columns.Contains("ImportProcess"))
            {
                result.ImportProcess = SafeHelper.GetSafestring(rw["ImportProcess"]);
            }

            return result;
        }
    }
}
