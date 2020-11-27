using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class FeedbackAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<FeedbackEntity> Adapt(DataTable dt)
        {
            List<FeedbackEntity> results = new List<FeedbackEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                FeedbackEntity matchCode = new FeedbackEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public FeedbackEntity AdaptItem(DataRow rw, DataTable dt)
        {
            FeedbackEntity result = new FeedbackEntity();
            result.Id = SafeHelper.GetSafeint(rw["Id"]);

            if (dt.Columns.Contains("ClientInformation"))
            {
                result.ClientInformation = SafeHelper.GetSafestring(rw["ClientInformation"]);
            }

            if (dt.Columns.Contains("EnteredBy"))
            {
                result.EnteredBy = SafeHelper.GetSafestring(rw["EnteredBy"]);
            }

            if (dt.Columns.Contains("EmailAddress"))
            {
                result.EmailAddress = SafeHelper.GetSafestring(rw["EmailAddress"]);
            }

            if (dt.Columns.Contains("FeedbackType"))
            {
                result.FeedbackType = SafeHelper.GetSafestring(rw["FeedbackType"]);
            }

            if (dt.Columns.Contains("Feedback"))
            {
                result.Feedback = SafeHelper.GetSafestring(rw["Feedback"]);
            }

            if (dt.Columns.Contains("AddDateTime"))
            {
                result.AddDateTime = SafeHelper.GetSafeDateTime(rw["AddDateTime"]);
            }

            if (dt.Columns.Contains("FeedbackPath"))
            {
                result.FeedbackPath = SafeHelper.GetSafestring(rw["FeedbackPath"]);
            }

            return result;
        }

    }
}
