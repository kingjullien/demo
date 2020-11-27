using Newtonsoft.Json.Linq;
using SBISCCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility
{
    public class SalesForceMethod
    {
        #region "Salesforce Tables"
        //set Dropdown
        public static SelectList FillTableName()
        {
            // Return list of the table name from the Salesforce and fill into dropdown
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            if (Helper.SalesForcedtTable != null)
            {
                DataTable dt = Helper.SalesForcedtTable;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstAllFilter.Add(new SelectListItem { Value = dt.Rows[i]["name"].ToString(), Text = dt.Rows[i]["name"].ToString() });
                }
            }
            return new SelectList(lstAllFilter, "Value", "Text");
        }

        //check Validate Salesforce Token
        public bool ValidateSalesforceToken()
        {
            bool IsValid = false;
            if (!string.IsNullOrEmpty(Helper.SalesForceAccessToken))
            {
                IsValid = true;
            }
            return IsValid;
        }

        public List<SelectListItem> GetTableViewName(string access_token, string instance_url, string TableName)
        {
            List<SelectListItem> lstResult = new List<SelectListItem>();
            JObject jsonnew = new JObject();
            using (var sobjectsData = new WebClient())
            {
                try
                {
                    sobjectsData.Headers.Add("Authorization", "Bearer " + access_token);
                    sobjectsData.Headers.Add("content-type", "application/json");
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var responses = sobjectsData.DownloadString(instance_url + "/services/data/" + Helper.SalesForceVersion + "/sobjects/" + TableName + "/listviews");
                    jsonnew = JObject.Parse(responses);
                    DataTable dtnew;
                    dtnew = CommonMethod.ConvertJSONToDataTable(jsonnew.ToString());

                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        lstResult.Add(new SelectListItem { Value = dtnew.Rows[i]["resultsUrl"].ToString(), Text = dtnew.Rows[i]["label"].ToString() });
                    }
                }
                catch
                {
                    //Empty catch block to stop from breaking
                }
            }
            return lstResult;
        }


        public DataTable setSalesForceDataTable(string access_token, string instance_url, string ListViewName)
        {
            string Message = string.Empty;
            using (var sobjectsData = new WebClient())
            {

                sobjectsData.Headers.Add("Authorization", "Bearer " + access_token);
                sobjectsData.Headers.Add("content-type", "application/json");
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var responses = sobjectsData.DownloadString(instance_url + ListViewName);

                    var serializer = new JavaScriptSerializer();
                    SalesForcesCustomView objMatch = serializer.Deserialize<SalesForcesCustomView>(responses);
                    DataTable dt = new DataTable();

                    if (objMatch.records != null && objMatch.records.Any())
                    {
                        foreach (var cols in objMatch.records.First().columns)
                        {
                            dt.Columns.Add(cols.fieldNameOrPath);
                        }
                        foreach (var item in objMatch.records)
                        {
                            DataRow dr = dt.NewRow();
                            foreach (var inneritem in item.columns)
                            {
                                dr[inneritem.fieldNameOrPath] = inneritem.value;
                            }
                            dt.Rows.Add(dr);
                        }
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        #endregion
    }
}