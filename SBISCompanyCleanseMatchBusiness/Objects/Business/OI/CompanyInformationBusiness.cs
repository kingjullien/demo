using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{

    public class CompanyInformationBusiness : BusinessParent
    {
        CompanyInformationRepository rep;
        public CompanyInformationBusiness(string connectionString) : base(connectionString) { rep = new CompanyInformationRepository(Connection); }
        public int InsertCompanyInformation(OICompanyInformationEntity obj)
        {
            return rep.InsertCompanyInformation(obj);
        }
    }
}
