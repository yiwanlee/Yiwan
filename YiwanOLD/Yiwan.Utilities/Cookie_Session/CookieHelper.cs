/// <summary>
/// 类说明：Cookie操作类(Cookie的增删查改)
/// 联系方式：liley@foxmail.com 秋秋:897250000
/// 额外依赖：System.Web 4.0
/// </summary>
using System;
using System.Web;

namespace Yiwan.Utilities
{
    public class CookieHelper
    {
        /// <summary>
        /// 清除所有的Cookie
        /// </summary>
        public static void Clear(HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            foreach (string name in Context.Request.Cookies.AllKeys)
            {
                Remove(name, Context);
            }
        }

        /// <summary>
        /// 清空所有的Cookie
        /// </summary>
        /// <returns></returns>
        public static void RemoveAll(HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            Clear(Context);
        }

        /// <summary>
        /// 删除一个指定的Cookie
        /// </summary>
        /// <param name="name">Cookie的名称</param>
        public static void Remove(string name, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            HttpCookie cookie = Context.Request.Cookies[name];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                Context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 获取指定Cookie
        /// </summary>
        /// <param name="name">Cookie的名称</param>
        public static HttpCookie GetCookie(string name, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            HttpCookie cookie = Context.Request.Cookies[name];
            return cookie;
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="name">Cookie的名称</param>
        public static string GetCookieValue(string name, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            HttpCookie cookie = GetCookie(name, Context);
            string str = string.Empty;
            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }

        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        /// <param name="name">Cookie的名称</param>
        /// <param name="cookievalue">Cookie的值</param>
        public static void SetCookie(string name, string cookievalue, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            SetCookie(name, cookievalue, null, Context);
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="name">Cookie的名称</param>
        /// <param name="cookievalue">Cookie的值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void SetCookie(string name, string cookievalue, DateTime? expires, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            HttpCookie cookie = new HttpCookie(name)
            {
                Value = cookievalue
            };
            if (expires != null)
            {
                cookie.Expires = (DateTime)expires;
            }
            Context.Response.Cookies.Add(cookie);
        }
    }
}
