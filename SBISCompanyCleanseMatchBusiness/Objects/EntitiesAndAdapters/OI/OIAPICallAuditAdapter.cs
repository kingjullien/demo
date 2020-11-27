using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OIAPICallAuditAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<OIAPICallAuditEntity> Adapt(DataTable dt)
        {
            List<OIAPICallAuditEntity> results = new List<OIAPICallAuditEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                OIAPICallAuditEntity matchCode = new OIAPICallAuditEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public OIAPICallAuditEntity AdaptItem(DataRow rw, DataTable dt)
        {
            OIAPICallAuditEntity result = new OIAPICallAuditEntity();
            result.Id = SafeHelper.GetSafeint(rw["Id"]);
            if (dt.Columns.Contains("TransactionTimeStamp"))
            {
                result.TransactionTimeStamp = SafeHelper.GetSafeDateTime(rw["TransactionTimeStamp"]);
            }

            if (dt.Columns.Contains("SubDomain"))
            {
                result.SubDomain = SafeHelper.GetSafestring(rw["SubDomain"]);
            }

            if (dt.Columns.Contains("CallType"))
            {
                result.CallType = SafeHelper.GetSafestring(rw["CallType"]);
            }

            if (dt.Columns.Contains("CallURL"))
            {
                result.CallURL = SafeHelper.GetSafestring(rw["CallURL"]);
            }

            if (dt.Columns.Contains("CandidateCount"))
            {
                result.CandidateCount = SafeHelper.GetSafeint(rw["CandidateCount"]);
            }

            if (dt.Columns.Contains("ResultCode"))
            {
                result.ResultCode = SafeHelper.GetSafeint(rw["ResultCode"]);
            }

            if (dt.Columns.Contains("ErrorDescription"))
            {
                result.ErrorDescription = SafeHelper.GetSafestring(rw["ErrorDescription"]);
            }

            return result;
        }
    }
}
