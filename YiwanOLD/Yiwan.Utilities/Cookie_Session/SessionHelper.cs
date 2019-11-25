/// <summary>
/// 类说明：Session操作类(Session的增删查改)
/// 联系方式：liley@foxmail.com 秋秋:897250000
/// </summary>
using System.Web;

namespace Yiwan.Utilities
{
    /// <summary>
    /// Session操作类(Session的增删查改)
    /// 1、Get(string name)获取Session对象
    /// 2、Set(string name, object val)设置Session
    /// 3、Remove(string name)删除Session
    /// 4、Clear()清空Session
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 清空所有的Session
        /// </summary>
        public static void Clear(HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            Context.Session.Clear();
        }

        /// <summary>
        /// 清空所有的Session
        /// </summary>
        public static void RemoveAll(HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            Clear(Context);
        }

        /// <summary>
        /// 删除一个指定的Session
        /// </summary>
        /// <param name="name">Session的名称</param>
        public static void Remove(string name, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            Context.Session.Remove(name);
        }

        /// <summary>
        /// 根据Session名获取Session对象
        /// </summary>
        /// <param name="name">Session的名称</param>
        public static object Get(string name, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            return Context.Session[name];
        }

        /// <summary>
        /// 设置一个Session
        /// </summary>
        /// <param name="name">Session的名称</param>
        /// <param name="val">Session的值</param>
        public static void Set(string name, object val, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            Context.Session.Remove(name);
            Context.Session.Add(name, val);
        }

        /// <summary>
        /// 设置一个Session
        /// </summary>
        /// <param name="name">Session的名称</param>
        /// <param name="val">Session的值</param>
        /// <param name="timeout">在??分钟后失效</param>
        public static void Set(string name, object val, int timeout, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            Set(name, val, Context);
            Context.Session.Timeout = timeout;
        }

        /// <summary>
        /// 设置Session过期时间
        /// </summary>
        /// <param name="timeout">在??分钟后失效</param>
        public static void SetTimeout(int timeout, HttpContext Context = null)
        {
            Context = Context ?? HttpContext.Current;
            Context.Session.Timeout = timeout;
        }

    }
}
