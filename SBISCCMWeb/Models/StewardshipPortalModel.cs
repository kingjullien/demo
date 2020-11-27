using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Models
{
    public class StewardshipPortalModel
    {
        public List<CompanyEntity> Companies = new List<CompanyEntity>();
        public static List<string> GetStatecountries(string ConnectionString)
        {
            int totalrecords = 10;

            BadInputDataModel model = new BadInputDataModel();
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.UserName);
            Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, 1, 10, out totalrecords);
            model.Companies = tuplecompany.Item1;
            var countries = (from a in model.Companies select a.CountryISOAlpha2Code).Distinct().ToList();
            return countries;
        }
        public static List<string> GetStates(string ConnectionString)
        {
            int totalrecords = 10;

            BadInputDataModel model = new BadInputDataModel();
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.UserName);
            Tuple<List<CompanyEntity>, string> tuplecompany = fac.GetLCMCompany(Helper.oUser.UserId, 1, 10, out totalrecords);
            model.Companies = tuplecompany.Item1;
            var states = (from a in model.Companies select a.State).Distinct().ToList();
            return states;
        }
        public static SelectList GetAllFilter()
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            lstAllFilter.Add(new SelectListItem { Value = "0", Text = "Any" });
            lstAllFilter.Add(new SelectListItem { Value = "1", Text = "A (Same)" });
            lstAllFilter.Add(new SelectListItem { Value = "2", Text = "B (Similar)" });
            return new SelectList(lstAllFilter, "Value", "Text");

            //List<string> lstAllFilter = new List<string>();
            //lstAllFilter.Add("A (Same)");
            //lstAllFilter.Add("B (Similar)");
            //return lstAllFilter;
        }

        public static List<string> LoadTopConfidenceCode(bool IsFirst)
        {
            List<string> lstCode = new List<string>();
            SettingFacade fac = new SettingFacade("");
            DataTable dtTopMatchResult = new DataTable();
            dtTopMatchResult = fac.GetTopMatchGradeSettings(IsFirst);
            if (dtTopMatchResult != null && dtTopMatchResult.Rows.Count > 0)
            {
                DataView dtView = dtTopMatchResult.DefaultView;
                if (dtView != null)
                {
                    dtView.Sort = "DnBConfidenceCode DESC";
                    DataTable dtResult = dtView.ToTable(true, "DnBConfidenceCode");
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        lstCode.Add(dtResult.Rows[i][0].ToString());
                    }
                }
            }


            return lstCode;
        }
        public static List<string> LoadTopMatchGrades(bool IsFirst)
        {
            List<string> lstMatchGrade = new List<string>();
            SettingFacade fac = new SettingFacade("");
            DataTable dtTopMatchResult = new DataTable();
            dtTopMatchResult = fac.GetTopMatchGradeSettings(IsFirst);
            if (dtTopMatchResult != null && dtTopMatchResult.Rows.Count > 0)
            {
                DataView dtView = dtTopMatchResult.DefaultView;
                if (dtView != null)
                {
                    dtView.Sort = "DnBMatchGradeText DESC";
                    DataTable dtResult = dtView.ToTable(true, "DnBMatchGradeText");
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        lstMatchGrade.Add(dtResult.Rows[i][0].ToString());
                    }
                }
            }


            return lstMatchGrade;
        }


    }
    public class InputMatches
    {
        public string InputId { get; set; }
        public string MatchSeqence { get; set; }
    }
}