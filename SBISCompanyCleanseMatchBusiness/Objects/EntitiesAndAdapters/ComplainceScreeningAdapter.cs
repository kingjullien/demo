using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class ComplainceScreeningAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ComplainceScreeningEntity> Adapt(DataTable dt)
        {
            List<ComplainceScreeningEntity> results = new List<ComplainceScreeningEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ComplainceScreeningEntity productDataEntity = new ComplainceScreeningEntity();
                productDataEntity = AdaptItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }

        public ComplainceScreeningEntity AdaptItem(DataRow rw, DataTable dt)
        {
            ComplainceScreeningEntity result = new ComplainceScreeningEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("DUNSNo"))
            {
                result.DUNSNo = SafeHelper.GetSafestring(rw["DUNSNo"]);
            }

            if (dt.Columns.Contains("Clientsubdomain"))
            {
                result.Clientsubdomain = SafeHelper.GetSafestring(rw["Clientsubdomain"]);
            }

            if (dt.Columns.Contains("FileName"))
            {
                result.FileName = SafeHelper.GetSafestring(rw["FileName"]);
            }

            if (dt.Columns.Contains("RequestedDate"))
            {
                result.RequestedDate = SafeHelper.GetSafeDateTime(rw["RequestedDate"]);
            }

            if (dt.Columns.Contains("ProcessStartDate"))
            {
                result.ProcessStartDate = SafeHelper.GetSafeDateTime(rw["ProcessStartDate"]);
            }

            if (dt.Columns.Contains("ProcessEndDate"))
            {
                result.ProcessEndDate = SafeHelper.GetSafeDateTime(rw["ProcessEndDate"]);
            }

            if (dt.Columns.Contains("IsProcessComplete"))
            {
                result.IsProcessComplete = SafeHelper.GetSafebool(rw["IsProcessComplete"]);
            }

            return result;
        }
    }
}
