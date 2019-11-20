using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Web;

namespace Yiwan.Utilities
{
    /// <summary>
    /// Cookie操作类(Cookie的增删查改) By @yiwanlee liley@foxmail.com 秋秋:897250000
    /// 1、Get(string key)获取Cookie对象
    /// 2、Set(string key, string value)设置Cookie
    /// 3、Remove(string key)删除Cookie
    /// 4、Clear()清空Cookie
    /// </summary>
    public static class Cookier
    {
        /// <summary>
        /// 删除一个指定的Cookie
        /// </summary>
        /// <param name="key">Cookie的键</param>
        public static void Remove(string key, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null) return;

            HttpCookie cookie = context.Request.Cookies[key];
            if (cookie != null)
            {
                CultureInfo cti = CultureInfo.CurrentCulture;
                cookie.Expires = DateTime.Parse("1900-01-01", cti.DateTimeFormat);
                context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 移除所有的Cookie 等价于 Clear
        /// </summary>
        public static void RemoveAll(HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null) return;

            foreach (string name in context.Request.Cookies.AllKeys)
            {
                Remove(name, context);
            }
        }

        /// <summary>
        /// 清除所有的Cookie 等价于 RemoveAll
        /// </summary>
        public static void Clear(HttpContext context = null)
        {
            RemoveAll(context);
        }

        /// <summary>
        /// 获取指定Cookie
        /// </summary>
        /// <param name="key">Cookie的键</param>
        public static HttpCookie Get(string key, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null) return null;

            HttpCookie cookie = context.Request.Cookies[key];
            return cookie;
        }

        /// <summary>
        /// 获取指定Cookie的值
        /// </summary>
        /// <param name="key">Cookie的键</param>
        public static string GetValue(string key, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null) return string.Empty;

            HttpCookie cookie = Get(key, context);
            if (cookie != null) return cookie.Value;
            else return string.Empty;
        }

        /// <summary>
        /// 获取指定Cookie的值
        /// </summary>
        /// <param name="key">Cookie的键</param>
        public static T GetValue<T>(string key, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null) return default;

            HttpCookie cookie = Get(key, context);
            if (cookie != null) return Convert2Object<T>(cookie.Value);
            else return default;
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="key">Cookie的键</param>
        /// <param name="value">Cookie的值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void Set(string key, string value, DateTime? expires, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null) return;

            HttpCookie cookie = new HttpCookie(key)
            {
                Value = value
            };
            if (expires != null) cookie.Expires = (DateTime)expires;

            context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Cookie的名称</param>
        /// <param name="value">Cookie的值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void Set<T>(string key, T value, DateTime? expires, HttpContext context = null)
        {
            Set(key, Convert2Json(value), expires, context);
        }

        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        /// <param name="name">Cookie的名称</param>
        /// <param name="cookievalue">Cookie的键</param>
        public static void Set(string key, string value, HttpContext context = null)
        {
            Set(key, value, null, context);
        }

        /// <summary>
        /// 添加一个Cookie（不设置过期时间，随浏览器生命周期）
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Cookie的名称</param>
        /// <param name="value">Cookie的键</param>
        public static void Set<T>(string key, T value, HttpContext context = null)
        {
            Set(key, Convert2Json(value), null, context);
        }

        #region 内部私有函数
        /// <summary>
        /// 序列化对象为Json字符串
        /// </summary>
        private static string Convert2Json<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        /// <summary>
        /// 反序列化Json字符串为Object对象
        /// </summary>
        private static T Convert2Object<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;
            else if (typeof(T).Name.Equals(typeof(string).Name, StringComparison.CurrentCulture))
            {
                return JsonConvert.DeserializeObject<T>($"'{value}'");
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
        #endregion 内部私有函数
    }
}
