using System;

namespace Yiwan.Utilities
{
    public static class ElmahLog
    {
        public static string Log(string message)
        {
            var Context = System.Web.HttpContext.Current;
            try { var req = System.Web.HttpContext.Current.Request; }
            catch (System.Web.HttpException) { Context = null; }

            return Elmah.ErrorLog.GetDefault(Context).Log(new Elmah.Error(new Exception("#日志#:" + message)));
        }
        public static string Log(string message, Exception innerException)
        {
            var Context = System.Web.HttpContext.Current;
            try { var req = System.Web.HttpContext.Current.Request; }
            catch (System.Web.HttpException) { Context = null; }

            return Elmah.ErrorLog.GetDefault(Context).Log(new Elmah.Error(new Exception("#日志#:" + message, innerException)));
        }

        public static string Error(string message)
        {
            var Context = System.Web.HttpContext.Current;
            try { var req = System.Web.HttpContext.Current.Request; }
            catch (System.Web.HttpException) { Context = null; }

            return Elmah.ErrorLog.GetDefault(Context).Log(new Elmah.Error(new Exception("#错误#:" + message)));
        }
        public static string Error(string message, Exception innerException)
        {
            var Context = System.Web.HttpContext.Current;
            try { var req = System.Web.HttpContext.Current.Request; }
            catch (System.Web.HttpException) { Context = null; }

            return Elmah.ErrorLog.GetDefault(Context).Log(new Elmah.Error(new Exception("#错误#:" + message, innerException)));
        }
        public static string Error(Exception exception)
        {
            var Context = System.Web.HttpContext.Current;
            try { var req = System.Web.HttpContext.Current.Request; }
            catch (System.Web.HttpException) { Context = null; }

            return Elmah.ErrorLog.GetDefault(Context).Log(new Elmah.Error(exception));
        }
    }
}