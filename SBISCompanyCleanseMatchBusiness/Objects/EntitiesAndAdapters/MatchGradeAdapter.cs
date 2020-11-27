using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class MatchGradeAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MatchGradeEntity> Adapt(DataTable dt)
        {
            List<MatchGradeEntity> results = new List<MatchGradeEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MatchGradeEntity matchGrade = new MatchGradeEntity();
                matchGrade = AdaptItem(rw);
                results.Add(matchGrade);
            }
            return results;
        }

        public MatchGradeEntity AdaptItem(DataRow rw)
        {
            MatchGradeEntity result = new MatchGradeEntity();
            result.MatchGradeCode = SafeHelper.GetSafestring(rw["MatchGradeCode"]);
            result.MatchGradeValue = SafeHelper.GetSafestring(rw["MatchGradeValue"]);
            return result;
        }
    }
}
