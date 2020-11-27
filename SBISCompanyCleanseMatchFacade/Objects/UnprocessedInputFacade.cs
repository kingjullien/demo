using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class UnprocessedInputFacade : FacadeParent
    {
        UnprocessedInputBusiness rep;
        public UnprocessedInputFacade(string connectionString) : base(connectionString) { rep = new UnprocessedInputBusiness(Connection); }

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
