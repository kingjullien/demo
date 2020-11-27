using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class CompanyInformationFacade : FacadeParent
    {
        CompanyInformationBusiness rep;
        public CompanyInformationFacade(string connectionString) : base(connectionString) { rep = new CompanyInformationBusiness(Connection); }

        public int InsertCompanyInformation(OICompanyInformationEntity obj)
        {
            return rep.InsertCompanyInformation(obj);
        }
    }
}
