using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ElmahAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ElmahEntity> Adapt(DataTable dt)
        {
            List<ElmahEntity> results = new List<ElmahEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ElmahEntity matchCode = new ElmahEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public ElmahEntity AdaptItem(DataRow rw, DataTable dt)
        {
            ElmahEntity result = new ElmahEntity();
            result.ErrorId = SafeHelper.GetSafestring(rw["ErrorId"]);

            if (dt.Columns.Contains("Host"))
                result.Host = SafeHelper.GetSafestring(rw["Host"]);
            if (dt.Columns.Contains("StatusCode"))
                result.StatusCode = SafeHelper.GetSafestring(rw["StatusCode"]);
            if (dt.Columns.Contains("Type"))
                result.Type = SafeHelper.GetSafestring(rw["Type"]);
            if (dt.Columns.Contains("Error"))
                result.Error = SafeHelper.GetSafestring(rw["Error"]);
            if (dt.Columns.Contains("User"))
                result.User = SafeHelper.GetSafestring(rw["User"]);

            if (dt.Columns.Contains("TimeUtc"))
                result.TimeUtc = SafeHelper.GetSafeDateTime(rw["TimeUtc"]);


            if (dt.Columns.Contains("Application"))
                result.Application = SafeHelper.GetSafestring(rw["Application"]);
            if (dt.Columns.Contains("Source"))
                result.Source = SafeHelper.GetSafestring(rw["Source"]);
            if (dt.Columns.Contains("AllXml"))
                result.AllXml = SafeHelper.GetSafestring(rw["AllXml"]);


            return result;
        }

    }
}
