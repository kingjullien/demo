using Newtonsoft.Json;
using SBISCCMWeb.Models;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchFacade.Objects;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;

namespace SBISCCMWeb.Utility
{
    //updated caching to use redis cache
    public class CacheHelper
    {
        static CacheHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(Convert.ToString(ConfigurationManager.AppSettings["RedisKey"]));
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        // Create Cache Conenction using redis connection string
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        // Set value to cache using Key-value
        public static void Set<T>(string key, T value) where T : class
        {
            Set(key, value, new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["RedisCacheExpire"]), 0, 0));
        }
        // Set value to cache using Key-value-Time to live
        public static void Set<T>(string key, T value, TimeSpan tSpan) where T : class
        {
            try
            {
                var dbCon = Connection.GetDatabase();
                var DataString = JsonConvert.SerializeObject(value);
                dbCon.StringSet(key, DataString, tSpan);
            }
            catch (Exception ex)
            {
                // will implement logging when doing modification about Cosmos with mongo
            }

        }
        // Get value from cache using Key
        public static T Get<T>(string key) where T : class
        {
            try
            {
                var dbCon = Connection.GetDatabase();
                string DataString = dbCon.StringGet(key);
                if (string.IsNullOrEmpty(DataString))
                    return null;
                return JsonConvert.DeserializeObject<T>(DataString);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // Remove cache data
        public static bool Remove(string key)
        {
            try
            {
                var dbCon = Connection.GetDatabase();
                dbCon.KeyDelete(key);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static BaseModel GetBaseModel(string hostName, HttpContextBase currentContext)
        {
            hostName = hostName.ToLowerInvariant();

            BaseModel _oBaseModel = new BaseModel();
            if (currentContext != null && currentContext.Items["oBaseModel"] != null)
            {
                _oBaseModel = (BaseModel)currentContext.Items["oBaseModel"];
            }
            else
            {
                _oBaseModel = CacheHelper.Get<BaseModel>(hostName);
                if (_oBaseModel == null)
                {
                    _oBaseModel = new BaseModel();
                    MasterClientApplicationFacade fac = new MasterClientApplicationFacade(Helper.GetMasterConnctionstring());
                    General.setMasterConnectionString(Helper.GetMasterConnctionstring());
                    if (Helper.ApplicationData == null)
                    {
                        Helper.ApplicationData = fac.GetClientApplicationData(HttpContext.Current.Request.Url.Authority);
                    }
                    ClientApplicationData oClientApplicationData = new ClientApplicationData();
                    if (Helper.ApplicationData != null)
                    {
                        oClientApplicationData.ApplicationDBConnectionStringHash = Helper.ApplicationData.ApplicationDBConnectionStringHash;
                        oClientApplicationData.ClientName = Helper.ApplicationData.ClientName;
                        oClientApplicationData.ClientLogo = Helper.ApplicationData.ClientLogo;
                        oClientApplicationData.ApplicationId = Helper.ApplicationData.ApplicationId;

                    }
                    SessionHelper.GetConnctionstring = oClientApplicationData.ApplicationDBConnectionStringHash;
                    oClientApplicationData.ApplicationDBConnectionString = StringCipher.Decrypt(oClientApplicationData.ApplicationDBConnectionStringHash, General.passPhrase);
                    if (oClientApplicationData.ApplicationDBConnectionStringHash == null)
                        return null;

                    _oBaseModel.CurrentClient = oClientApplicationData;

                    CacheHelper.Set(hostName, _oBaseModel);
                }
                currentContext.Items["oBaseModel"] = _oBaseModel;
            }
            return _oBaseModel;
        }

        // Clears Cache data
        public static bool ClearCache(HttpContextBase currentContext)
        {
            string hostName = (new WebHelper(currentContext)).HostName();
            return CacheHelper.Remove(hostName);
        }
    }
}