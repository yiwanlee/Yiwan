using System;
using System.Web;
using System.Collections;
using System.Web.Caching;
using Newtonsoft.Json;

namespace Yiwan.Utilities.Cache
{
    /// <summary>
    /// 类 名 称：WebCache    
    /// 类 说 明：WebCache操作类
    /// 完成日期：2019-05-15
    /// 编码作者：赵立立 liley@foxmail.com QQ:897250000
    /// </summary>
    public static class Webcache
    {
        /// <summary>
        /// 是否存在指定的Key
        /// </summary>
        public static bool Exists(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }

        /// <summary>
        /// 是否存在指定的Key
        /// </summary>
        public static bool HashExists(string key, string dataKey)
        {
            return Exists(key + ":" + dataKey);
        }

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        public static T Get<T>(string key)
        {
            return Convert2Object<T>(HttpRuntime.Cache[key]);
        }

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        public static string Get(string key)
        {
            return Get<string>(key);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value, DateTime? expiryat = null)
        {
            if (expiryat == null) HttpRuntime.Cache.Insert(key, Convert2Json(value));
            else
            {
                DateTime attime = Convert.ToDateTime(expiryat ?? DateTime.MaxValue);
                HttpRuntime.Cache.Insert(key, Convert2Json(value), null, attime, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value, TimeSpan expiry)
        {
            HttpRuntime.Cache.Insert(key, Convert2Json(value), null, DateTime.MaxValue, expiry, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value, DateTime expiryat, TimeSpan expiry)
        {
            HttpRuntime.Cache.Insert(key, Convert2Json(value), null, expiryat, expiry, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        public static string HashGet(string key, string dataKey)
        {
            return Get<string>(key + ":" + dataKey);
        }

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        public static T HashGet<T>(string key, string dataKey)
        {
            return Get<T>(key + ":" + dataKey);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void HashSet(string key, string dataKey, object value, DateTime? expiryat = null)
        {
            Set(key + ":" + dataKey, value, expiryat);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void HashSet(string key, string dataKey, object value, TimeSpan expiry)
        {
            Set(key + ":" + dataKey, value, expiry);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void HashSet(string key, string dataKey, object value, DateTime expiryat, TimeSpan expiry)
        {
            Set(key + ":" + dataKey, value, expiryat, expiry);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void Remove(params string[] keys)
        {
            foreach (string key in keys) HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveHash(string key, string dataKey)
        {
            Remove(key + ":" + dataKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAll()
        {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                HttpRuntime.Cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// 序列化对象为Json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Convert2Json<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        /// <summary>
        /// 反序列化Json字符串为Object对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static T Convert2Object<T>(object value)
        {
            if (value == null) return default;
            if (typeof(T).Name.Equals(typeof(string).Name))
            {
                return JsonConvert.DeserializeObject<T>($"'{value.ToString()}'");
            }
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }
    }
}