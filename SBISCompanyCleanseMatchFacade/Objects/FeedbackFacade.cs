using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class FeedbackFacade
    {
        public FeedbackFacade(string connectionString = null)
        {
            if (connectionString != null)
            {
                General.setMasterConnectionString(connectionString);
            }
        }
        #region "Ticket Related Method"

        public int InsertUpdateFeedback(FeedbackEntity obj)
        {
            FeedbackBusiness rep = new FeedbackBusiness();
            return rep.InsertUpdateFeedback(obj);
        }
        #region Feedback
        // Gets All Feedback
        public List<FeedbackEntity> GetAllFeedback(int SortOrder, int PageNumber, int PageSize, out int TotalRecords, string HostName, string FeedbackType)
        {
            FeedbackBusiness rep = new FeedbackBusiness();
            List<FeedbackEntity> results = new List<FeedbackEntity>();
            results = rep.GetAllFeedback(SortOrder, PageNumber, PageSize, out TotalRecords, HostName, FeedbackType);
            return results;
        }
        #endregion
        #endregion
    }
}
