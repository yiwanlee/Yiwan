using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Yiwan.Utilities;
using Yiwan.Utilities.Cache;

namespace Yiwan.XiaoeAPI.Old
{
    /// <summary>
    /// 小鹅通Api客户端
    /// 需要先在AppSettings中配置Xiaoe_ClientID,Xiaoe_ClientSecret这些是必须的,
    /// AppSettings中的Redis连接字符串Xiaoe_Cache_Redis是可选的如无配置则使用内存缓存
    /// </summary>
    public class XiaoeClient
    {
        public static string ClientID = string.Empty;
        public static string ClientSecret = string.Empty;

        //静态初始化 或 Lazy<T> 可保证在使用的时候才创建
        static XiaoeClient()
        {
            ClientID = ConfigurationManager.AppSettings["Xiaoe_ClientID"];
            ClientSecret = ConfigurationManager.AppSettings["Xiaoe_ClientSecret"];
        }

        /// <summary>
        /// 发送请求，获取数据
        /// </summary>
        /// <param name="cmd">调用的命令</param>
        /// <param name="data">参数字典</param>
        /// <param name="version">API版本号，默认1.0</param>
        /// <param name="useType">数据的使用场景 默认0 可根据实际实际情况传入0-服务端自用，1-iOS，2-android，3-pc浏览器，4-手机浏览器</param>
        public static Tuple<bool, JObject> Post(string cmd, IDictionary<string, object> data, string version = "1.0", int useType = 0)
        {
            string url = "http://api.xiaoe-tech.com/open/" + cmd + "/" + version;

            SortedDictionary<string, object> prams = new SortedDictionary<string, object>
            {
                { "app_id", ClientID },
                { "use_type", useType },
                { "data", data },
                { "timestamp", Yiwan.Utilities.DateTimeHelper.GetTimestamp() }
            };
            prams.Add("sign", GetSign(prams));
            Console.WriteLine(JsonConvert.SerializeObject(prams));

            HttpHelper http = new HttpHelper();
            var rs = http.GetHtml(new HttpItem { URL = url, Method = "POST", ContentType = "application/json", Encoding = Encoding.UTF8, Postdata = JsonConvert.SerializeObject(prams) });
            return RefactResult(Yiwan.Utilities.Texter.Unicode2String(rs.Html));
        }

        public static string GetSign(IDictionary<string, object> data)
        {
            string strs = "";
            foreach (string k in data.Keys)
            {
                strs += strs == "" ? "" : "&";
                if (data[k] is bool || data[k] is char || data[k] is decimal || data[k] is double || data[k] is float || data[k] is int || data[k] is uint || data[k] is long || data[k] is ulong || data[k] is short || data[k] is ushort || data[k] is string)
                    strs += k + "=" + data[k].ToString();
                else strs += k + "=" + JsonConvert.SerializeObject(data[k]);
            }
            strs += "&app_secret=" + ClientSecret;
            string sign = Yiwan.Utilities.Security.MD5(strs, Encoding.UTF8).ToLower();
            return sign;
        }

        /// <summary>
        /// 重构结果，使其更易用。
        /// 如果返回error_response,
        /// 则Item1为false,否则为true,
        /// Item2为JObject类型的实际结果，不管是成功还是失败
        /// </summary>
        /// <param name="jsonRes">接口API返回的JSON格式的结果字符串</param>
        public static Tuple<bool, JObject> RefactResult(string jsonRes)
        {
            try
            {
                var jobjRes = JObject.Parse(jsonRes);
                if (jobjRes.ContainsKey("code") && jobjRes["code"].ToString().Equals("0"))
                    return new Tuple<bool, JObject>(true, jobjRes);
                else return new Tuple<bool, JObject>(false, jobjRes);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, JObject>(false, JObject.Parse(JsonConvert.SerializeObject(ex)));
            }
        }
    }
}
