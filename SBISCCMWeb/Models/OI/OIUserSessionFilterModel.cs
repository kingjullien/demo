using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using SBISCompanyCleanseMatchFacade.Objects.OI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.OI
{
    // DB Changes (MP-716)
    public class OIUserSessionFilterModel
    {
        public OIUserSessionFilterEntity objUsers = new OIUserSessionFilterEntity();
        public static List<string> GetCountry(string ConnectionString)
        {

            OIUserSessionFacade fac = new OIUserSessionFacade(ConnectionString);
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