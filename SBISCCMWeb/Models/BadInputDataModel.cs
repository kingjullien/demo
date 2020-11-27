using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class BadInputDataModel
    {
        public List<CompanyEntity> Companies = new List<CompanyEntity>();
        // Get the Country according to the State
        public static List<string> GetStatecountries(string ConnectionString)
        {
            int totalrecords = 10;

            BadInputDataModel model = new BadInputDataModel();

            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.UserName);
            //model.Companies = fac.GetBIDCompany(Helper.oUser.UserId, 1, 10, out totalrecords);
            Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetBIDCompany(Helper.oUser.UserId, 1, 10, out totalrecords);
            model.Companies = tuplecompany.Item1;
            var countries = (from a in model.Companies select a.CountryISOAlpha2Code).Distinct().ToList();
            return countries;
        }
        // Get State or region 
        public static List<string> GetStates(string ConnectionString)
        {
            int totalrecords = 10;

            BadInputDataModel model = new BadInputDataModel();
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.UserName);
            //model.Companies = fac.GetBIDCompany(Helper.oUser.UserId, 1, 10, out totalrecords);
            Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetBIDCompany(Helper.oUser.UserId, 1, 10, out totalrecords);
            model.Companies = tuplecompany.Item1;
            var states = (from a in model.Companies select a.State).Distinct().ToList();
            return states;
        }

    }
}