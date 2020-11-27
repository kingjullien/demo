using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Models
{
    public class UserSessionFilterModel
    {
        public UserSessionFilterEntity objUsers = new UserSessionFilterEntity();
        public static List<string> GetCountry(string ConnectionString)
        {

            UserSessionFacade fac = new UserSessionFacade(ConnectionString);
            List<CountryEntity> lstCountry = fac.GetCountries();
            var Country = (from a in lstCountry select a.CountryWithISOCode).Distinct().ToList();
            return Country;
        }

        public static List<string> GetOrderColumn()
        {
            List<string> lst = new List<string>();
            lst.Add("SrcRecordId");
            lst.Add("CompanyName");
            lst.Add("Address");
            lst.Add("City");
            lst.Add("State");
            lst.Add("PostalCode");
            lst.Add("Country");
            lst.Add("PhoneNbr");
            return lst;
        }
    }
}