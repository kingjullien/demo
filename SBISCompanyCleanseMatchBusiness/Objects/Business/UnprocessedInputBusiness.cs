using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class UnprocessedInputBusiness : BusinessParent
    {
        UnprocessedInputRepository rep;
        public UnprocessedInputBusiness(string connectionString) : base(connectionString) { rep = new UnprocessedInputRepository(Connection); }

        public List<UnprocessedInputEntity> GetUnprocessedInputRecords(string importProcess, int PageSize, int PageNumber, out int TotalRecords)
        {
            return rep.GetUnprocessedInputRecords(importProcess, PageSize, PageNumber, out TotalRecords);
        }
        public bool DeleteUnprocessedInputRecords(string importProcess)
        {
            return rep.DeleteUnprocessedInputRecords(importProcess);
        }
        public bool MoveUnprocessedInputRecordsToBID(string importProcess)
        {
            return rep.MoveUnprocessedInputRecordsToBID(importProcess);
        }
    }
}
