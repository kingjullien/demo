using MvcSiteMapProvider.Caching;
using ServiceStack.Caching;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{

    public class WebCacheClient : ICacheClient
    {
        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, expiresIn);
            return true;
        }

        public bool Add<T>(string key, T value, DateTimeOffset expiresAt) where T : class
        {
            var expiration = expiresAt.Subtract(DateTimeOffset.Now);
            HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.Add(expiration), System.Web.Caching.Cache.NoSlidingExpiration);
            return true;
        }

        public bool Add<T>(string key, T value)
        {
            HttpContext.Current.Cache.Insert(key, value);
            return true;
        }

        public bool AddAll<T>(IList<Tuple<string, T>> items) where T : class
        {
            for (var i = 0; i < items.Count; i++)
            {
                HttpContext.Current.Cache.Insert(items[i].Item1, items[i].Item2);
            }
            return true;
        }

        public bool Exists(string key)
        {
            return (HttpContext.Current.Cache[key] == null);
        }

        public T Get<T>(string key)
        {
            return (T)HttpContext.Current.Cache[key];
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                result.Add(key, this.Get<T>(key));
            }
            return result;
        }

        public bool Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key); //Remove the cached item
            return true;
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                HttpContext.Current.Cache.Remove(key);
            }
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            this.Remove(key);
            this.Add(key, value, expiresIn);
            return true;
        }

        //public bool Replace<T>(string key, T value, DateTimeOffset expiresAt)
        //{
        //    this.Remove(key);
        //    this.Add(key, value, expiresAt);
        //    return true;
        //}

        public bool Replace<T>(string key, T value)
        {
            this.Remove(key);
            this.Add(key, value);
            return true;
        }



        //System.Threading.Tasks.Task<bool> ICacheClient.AddAllAsync<T>(IList<Tuple<string, T>> items)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<bool> ICacheClient.AddAsync<T>(string key, T value, TimeSpan expiresIn)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<bool> ICacheClient.AddAsync<T>(string key, T value, DateTimeOffset expiresAt)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<bool> ICacheClient.AddAsync<T>(string key, T value)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<bool> ICacheClient.ExistsAsync(string key)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task ICacheClient.FlushDbAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //void ICacheClient.FlushDb()
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<IDictionary<string, T>> ICacheClient.GetAllAsync<T>(IEnumerable<string> keys)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<T> ICacheClient.GetAsync<T>(string key)
        //{
        //    throw new NotImplementedException();
        //}

        //Dictionary<string, string> ICacheClient.GetInfo()
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<Dictionary<string, string>> ICacheClient.GetInfoAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task ICacheClient.RemoveAllAsync(IEnumerable<string> keys)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<bool> ICacheClient.RemoveAsync(string key)
        //{
        //    throw new NotImplementedException();
        //}



        //System.Threading.Tasks.Task<bool> ICacheClient.ReplaceAsync<T>(string key, T value, TimeSpan expiresIn)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<bool> ICacheClient.ReplaceAsync<T>(string key, T value, DateTimeOffset expiresAt)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<bool> ICacheClient.ReplaceAsync<T>(string key, T value)
        //{
        //    throw new NotImplementedException();
        //}

        //StackExchange.Redis.IDatabase ICacheClient.Database
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //void ICacheClient.Save(StackExchange.Redis.SaveType saveType)
        //{
        //    throw new NotImplementedException();
        //}

        //void ICacheClient.SaveAsync(StackExchange.Redis.SaveType saveType)
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerable<string> ICacheClient.SearchKeys(string pattern)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<IEnumerable<string>> ICacheClient.SearchKeysAsync(string pattern)
        //{
        //    throw new NotImplementedException();
        //}

        //ISerializer ICacheClient.Serializer
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //bool ICacheClient.SetAdd(string memberName, string key)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<bool> ICacheClient.SetAddAsync(string memberName, string key)
        //{
        //    throw new NotImplementedException();
        //}

        //string[] ICacheClient.SetMember(string memberName)
        //{
        //    throw new NotImplementedException();
        //}

        //System.Threading.Tasks.Task<string[]> ICacheClient.SetMemberAsync(string memberName)
        //{
        //    throw new NotImplementedException();
        //}

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        public long Increment(string key, uint amount)
        {
            throw new NotImplementedException();
        }

        public long Decrement(string key, uint amount)
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public void FlushAll()
        {
            throw new NotImplementedException();
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
            throw new NotImplementedException();
        }
    }

}