using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class FeedbackBusiness
    {
        public int InsertUpdateFeedback(FeedbackEntity obj)
        {
            FeedbackRepository rep = new FeedbackRepository();
            return rep.InsertUpdateFeedback(obj);
        }
        #region Feedback
        // Gets All Feedback
        public List<FeedbackEntity> GetAllFeedback(int SortOrder, int PageNumber, int PageSize, out int TotalRecords, string HostName, string FeedbackType)
        {
            FeedbackRepository rep = new FeedbackRepository();
            List<FeedbackEntity> results = new List<FeedbackEntity>();
            results = rep.GetAllFeedback(SortOrder, PageNumber, PageSize, out TotalRecords, HostName, FeedbackType);
            return results;
        }
        #endregion
    }
}
