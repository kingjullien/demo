using PagedList;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Models
{
    public class UsersModel
    {
        public List<UsersEntity> users { get; set; }
        public UsersEntity objUsers { get; set; }

        public List<CountryGroupEntity> countryGroups { get; set; }
        public List<CountryEntity> countries { get; set; }
        public CountryGroupEntity objCountryGroup { get; set; }

        public List<CustomAttributeEntity> objCustomAttributes { get; set; }
        public CustomAttributeEntity objCustomAttribute { get; set; }

        public static List<UsersEntity> GetSecurityQuestion(string ConnectionString)
        {

            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.UserName);
            List<UsersEntity> securityQue = new List<UsersEntity>();
            securityQue = fac.GetSecurityQuestion();
            return securityQue;
        }




        public IPagedList<UsersEntity> usersPagingList { get; set; }
    }
}