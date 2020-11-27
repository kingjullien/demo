using Newtonsoft.Json;
using SBISCCMWeb.Models.OI;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Utility.OI
{
    public class OIBuildList
    {
        public OIBuildListSearchModelEntity OIBuildAList(OIBuildListSearchModelEntity objRequest, int? pageSize, string SubDomain)
        {
            string APIKey = Helper.OIAPIKey;
            APIKey = APIKey.Replace("Bearer ", "");
            string endpoint = ConfigurationManager.AppSettings["OIBuildAListUrl"];
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            #region "Create End Point"
            if (!string.IsNullOrEmpty(SubDomain))
            {
                endpoint += "&CustomerSubDomain=" + SubDomain;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.entity_type))
            {
                endpoint += "&entity_type=" + objRequest.request_fields.entity_type;
            }
            if (pageSize != null)
            {
                endpoint += "&limit=" + pageSize;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.parent_orb_num))
            {
                endpoint += "&parent_orb_num=" + objRequest.request_fields.parent_orb_num;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.ultimate_parent_orb_num))
            {
                endpoint += "&ultimate_parent_orb_num=" + objRequest.request_fields.ultimate_parent_orb_num;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.industry))
            {
                endpoint += "&industry=" + objRequest.request_fields.industry;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.address1))
            {
                endpoint += "&address1=" + objRequest.request_fields.address1;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.city))
            {
                endpoint += "&city=" + objRequest.request_fields.city;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.state))
            {
                endpoint += "&state	=" + objRequest.request_fields.state;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.zip))
            {
                endpoint += "&zip=" + objRequest.request_fields.zip;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.country))
            {
                endpoint += "&country=" + objRequest.request_fields.country;
            }
            if (objRequest.request_fields.employees != null)
            {
                if (objRequest.request_fields.employees.Length == 1)
                {
                    string[] employeeslst = objRequest.request_fields.employees[0].Split(',');
                    foreach (var item in employeeslst)
                    {
                        if (!string.IsNullOrEmpty(item))
                            endpoint += "&employees=" + item;
                    }
                }
                else
                {
                    foreach (var item in objRequest.request_fields.employees)
                    {
                        if (!string.IsNullOrEmpty(item))
                            endpoint += "&employees=" + item;
                    }
                }
            }
            if (objRequest.request_fields.revenue != null)
            {
                if (objRequest.request_fields.revenue.Length == 1)
                {
                    string[] revenuelst = objRequest.request_fields.revenue[0].Split(',');
                    foreach (var item in revenuelst)
                    {
                        if (!string.IsNullOrEmpty(item))
                            endpoint += "&revenue=" + item;
                    }
                }
                else
                {
                    foreach (var item in objRequest.request_fields.revenue)
                    {
                        if (!string.IsNullOrEmpty(item))
                            endpoint += "&revenue=" + item;
                    }
                }
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.techs))
            {
                endpoint += "&techs=" + objRequest.request_fields.techs;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.tech_categories))
            {
                endpoint += "&tech_categories=" + objRequest.request_fields.tech_categories;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.naics_codes))
            {
                endpoint += "&naics_codes=" + objRequest.request_fields.naics_codes;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.sic_codes))
            {
                endpoint += "&sic_codes=" + objRequest.request_fields.sic_codes;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.rankings))
            {
                endpoint += "&rankings=" + objRequest.request_fields.rankings;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.importance_score))
            {
                endpoint += "&importance_score=" + objRequest.request_fields.importance_score;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.cik))
            {
                endpoint += "&cik=" + objRequest.request_fields.cik;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.cusip))
            {
                endpoint += "&cusip=" + objRequest.request_fields.cusip;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.ticker))
            {
                endpoint += "&ticker=" + objRequest.request_fields.ticker;
            }
            if (objRequest.request_fields.offset != null)
            {
                endpoint += "&offset=" + objRequest.request_fields.offset;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.exchange))
            {
                endpoint += "&exchange=" + objRequest.request_fields.exchange;
            }
            if (objRequest.request_fields.show_full_profile != false)
            {
                endpoint += "&show_full_profile=" + objRequest.request_fields.show_full_profile;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.include))
            {
                endpoint += "&include=" + objRequest.request_fields.include;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.orb_num))
            {
                endpoint += "&orb_num=" + objRequest.request_fields.orb_num;
            }
            if (!string.IsNullOrEmpty(objRequest.request_fields.category))
            {
                endpoint += "&categories=" + objRequest.request_fields.category;
            }
            #endregion

            endpoint = endpoint.Replace("?&", "?");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", APIKey);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    OIBuildListSearchModelEntity objResponse = serializer.Deserialize<OIBuildListSearchModelEntity>(result);
                    return objResponse;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        OIBuildListSearchModelEntity objResponse = JsonConvert.DeserializeObject<OIBuildListSearchModelEntity>(result);
                        return objResponse;
                    }
                }
            }
            return null;
        }
    }
}