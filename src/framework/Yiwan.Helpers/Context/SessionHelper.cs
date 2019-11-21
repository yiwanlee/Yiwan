using System.Web;

namespace Yiwan.Helpers.Context
{
    /// <summary>
    /// Session操作类(Session的增删查改) By @yiwanlee liley@foxmail.com 秋秋:897250000
    /// 1、Get(string key)获取Session对象
    /// 2、Set(string key, string value)设置Session
    /// 3、Remove(string key)删除Session
    /// 4、Clear()清空Session
    /// </summary>
    public static class SessionHelper
    {
        /// <summary>
        /// 删除一个指定的Session
        /// </summary>
        /// <param name="key">Session的名称</param>
        public static void Remove(string name, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return;
            }

            context.Session.Remove(name);
        }

        /// <summary>
        /// 移除所有的Session 等价于 Clear
        /// </summary>
        public static void RemoveAll(HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return;
            }

            context.Session.Clear();
        }

        /// <summary>
        /// 清空所有的Session
        /// </summary>
        public static void Clear(HttpContext context = null)
        {
            RemoveAll(context);
        }

        /// <summary>
        /// 根据Session的键获取Session对象
        /// </summary>
        /// <param name="key">Session的键</param>
        public static string Get(string key, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return string.Empty;
            }

            if (context.Session[key] != null)
            {
                return context.Session[key].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据Session的键获取Session对象
        /// </summary>
        /// <param name="key">Session的键</param>
        public static T Get<T>(string key, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return default;
            }

            if (context.Session[key] != null)
            {
                return Utilities.JsonUtility.ConvertToObject<T>(context.Session[key].ToString());
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 设置一个Session
        /// </summary>
        /// <param name="key">Session的键</param>
        /// <param name="value">Session的值</param>
        public static void Set(string key, string value, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return;
            }

            context.Session.Remove(key);
            context.Session.Add(key, value);
        }

        /// <summary>
        /// 设置一个Session
        /// </summary>
        /// <param name="key">Session的键</param>
        /// <param name="value">Session的值</param>
        public static void Set<T>(string key, T value, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return;
            }

            context.Session.Remove(key);
            context.Session.Add(key, Utilities.JsonUtility.ConvertToJson(value));
        }

        /// <summary>
        /// 设置一个Session
        /// </summary>
        /// <param name="key">Session的键</param>
        /// <param name="value">Session的值</param>
        /// <param name="timeout">在??分钟后失效</param>
        public static void Set(string key, string value, int timeout, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return;
            }

            Set(key, value, context);
            context.Session.Timeout = timeout;
        }

        /// <summary>
        /// 设置一个Session
        /// </summary>
        /// <param name="key">Session的键</param>
        /// <param name="value">Session的值</param>
        /// <param name="timeout">在??分钟后失效</param>
        public static void Set<T>(string key, T value, int timeout, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return;
            }

            Set(key, value, context);
            context.Session.Timeout = timeout;
        }

        /// <summary>
        /// 设置Session过期时间
        /// </summary>
        /// <param name="timeout">在??分钟后失效</param>
        public static void SetTimeout(int timeout, HttpContext context = null)
        {
            context = context ?? HttpContext.Current;
            if (context == null)
            {
                return;
            }

            context.Session.Timeout = timeout;
        }
    }
}
