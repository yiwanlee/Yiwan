using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;

namespace Yiwan.Helpers.Http
{
    /// <summary>
    /// Http返回参数类  Copyright：http://www.httphelper.com/
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; private set; }
        public void SetCookieCollection(CookieCollection cookies)
        {
            CookieCollection = new CookieCollection();
            if (cookies != null) CookieCollection.Add(cookies);
        }
        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html { get; set; }

        private byte[] ResultByte { get; set; }
        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] GetResultByte()
        {
            return (byte[])ResultByte.Clone();
        }
        /// <summary>
        /// 设置Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public void SetResultByte(byte[] value)
        {
            if (value == null) ResultByte = Array.Empty<byte>();
            else ResultByte = (byte[])value.Clone();
        }
        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header { get; private set; }
        public void SetHeader(WebHeaderCollection header)
        {
            Header = new WebHeaderCollection();
            if (header != null) Header.Add(header);
        }
        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string StatusDescription { get; set; }
        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 最后访问的URl
        /// </summary>
        public string ResponseUri { get; set; }
        /// <summary>
        /// 获取重定向的URl
        /// </summary>
        public string RedirectUrl
        {
            get
            {
                try
                {
                    if (Header != null && Header.Count > 0)
                    {
                        if (Header.AllKeys.Any(k => k.ToLower(CultureInfo.InvariantCulture).Contains("location")))
                        {
                            string baseurl = Header["location"].ToString(CultureInfo.InvariantCulture).Trim();
                            string locationurl = baseurl.ToLower(CultureInfo.InvariantCulture);
                            if (!string.IsNullOrWhiteSpace(locationurl))
                            {
                                bool b = locationurl.StartsWith("http://", StringComparison.CurrentCulture) || locationurl.StartsWith("https://", StringComparison.CurrentCulture);
                                if (!b)
                                {
                                    baseurl = new Uri(new Uri(ResponseUri), baseurl).AbsoluteUri;
                                }
                            }
                            return baseurl;
                        }
                    }
                }
                catch { }
                return string.Empty;
            }
        }
        /// <summary>
        /// HttpItem类，用于存储实际执行所用到的HttpItem对象
        /// </summary>
        public HttpItem Item { get; set; }
    }
}
