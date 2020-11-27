using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class APICallLogAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<APICallLogEntity> Adapt(DataTable dt)
        {
            List<APICallLogEntity> results = new List<APICallLogEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                APICallLogEntity matchCode = new APICallLogEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public APICallLogEntity AdaptItem(DataRow rw, DataTable dt)
        {
            APICallLogEntity result = new APICallLogEntity();
            result.id = SafeHelper.GetSafeint(rw["id"]).ToString();


            if (dt.Columns.Contains("ApiMethod"))
            {
                result.ApiMethod = SafeHelper.GetSafestring(rw["ApiMethod"]);
            }

            if (dt.Columns.Contains("subDomain"))
            {
                result.subDomain = SafeHelper.GetSafestring(rw["subDomain"]);
            }

            if (dt.Columns.Contains("InputData"))
            {
                result.InputData = SafeHelper.GetSafestring(rw["InputData"]);
            }

            if (dt.Columns.Contains("OutputData"))
            {
                result.OutputData = SafeHelper.GetSafestring(rw["OutputData"]);
            }

            if (dt.Columns.Contains("Message"))
            {
                result.Message = SafeHelper.GetSafestring(rw["Message"]);
            }

            if (dt.Columns.Contains("HttpStatusCode"))
            {
                result.HttpStatusCode = SafeHelper.GetSafeint(rw["HttpStatusCode"]);
            }

            if (dt.Columns.Contains("InputPostBody"))
            {
                result.InputPostBody = SafeHelper.GetSafestring(rw["InputPostBody"]);
            }

            if (dt.Columns.Contains("CreatedDate"))
            {
                result.CreatedDate = SafeHelper.GetSafeDateTime(rw["CreatedDate"]);
            }

            if (dt.Columns.Contains("Environment"))
            {
                result.Environment = SafeHelper.GetSafestring(rw["Environment"]);
            }

            return result;
        }

    }
}
