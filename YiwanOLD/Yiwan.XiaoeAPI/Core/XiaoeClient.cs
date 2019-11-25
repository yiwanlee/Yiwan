using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Yiwan.Utilities;
using Yiwan.Utilities.Cache;

namespace Yiwan.XiaoeAPI
{
    /// <summary>
    /// 小鹅通Api客户端
    /// 需要先在AppSettings中配置Xiaoe_ClientID,Xiaoe_ClientSecret这些是必须的,
    /// AppSettings中的Redis连接字符串Xiaoe_Cache_Redis是可选的如无配置则使用内存缓存
    /// </summary>
    public class XiaoeClient
    {
        private static readonly object lockObject = new object();
        private static readonly bool _useRedis = false;

        public static string ClientID = string.Empty;
        public static string ClientSecret = string.Empty;

        //静态初始化 或 Lazy<T> 可保证在使用的时候才创建
        static XiaoeClient()
        {
            _useRedis = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Xiaoe_Cache_Redis"]);

            ClientID = ConfigurationManager.AppSettings["Xiaoe_ClientID"];
            ClientSecret = ConfigurationManager.AppSettings["Xiaoe_ClientSecret"];
        }

        public static string Xiaoe_Token()
        {
            #region 验证ClientID,ClientSecret,KdtId
            if (string.IsNullOrWhiteSpace(ClientID))
            {
                throw new Exception("AppSettings中没有找到Xiaoe_ClientID配置项或者该配置项为空字符串");
            }
            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                throw new Exception("AppSettings中没有找到Xiaoe_ClientSecret配置项或者该配置项为空字符串");
            }
            #endregion 验证ClientID,ClientSecret,KdtId

            try
            {
                string cacheKey = "Yiwan:Xiaoe:Token:" + ClientID;
                string token = string.Empty;
                RedisHelper redis = new RedisHelper(0);
                if (_useRedis) token = redis.StringGet(cacheKey);
                else token = Webcache.Get(cacheKey);

                if (string.IsNullOrWhiteSpace(token))
                {
                    lock (lockObject)
                    {
                        if (_useRedis) token = redis.StringGet(cacheKey);
                        else token = Webcache.Get(cacheKey);

                        if (string.IsNullOrWhiteSpace(token))
                        {
                            using (HttpClient httpClient = new HttpClient())
                            {
                                string url = $"https://api.xiaoe-tech.com/token?app_id={ClientID}&secret_key={ClientSecret}&grant_type=client_credential";
                                HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
                                // 确认响应成功，否则抛出异常  
                                response.EnsureSuccessStatusCode();
                                // 异步读取响应为字符串 
                                string result = response.Content.ReadAsStringAsync().Result;
                                JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                                if (Convert.ToInt32(jo["code"]) > 0) throw new Exception(jo["msg"].ToString());
                                token = jo["data"]["access_token"].ToString();
                                if (_useRedis)
                                {
                                    redis.StringSet(cacheKey, token, TimeSpan.FromSeconds(7000));
                                }
                                else
                                {
                                    Webcache.Set(cacheKey, token, TimeSpan.FromSeconds(7000));
                                }
                                //用完要记得释放
                                httpClient.Dispose();
                            }
                        }
                    }
                }
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("查询小鹅通Token出错!", ex);
            }
        }

        /// <summary>
        /// 发送请求，获取数据
        /// </summary>
        /// <param name="cmd">调用的命令</param>
        /// <param name="data">参数字典</param>
        /// <param name="version">API版本号，默认1.0</param>
        /// <param name="useType">数据的使用场景 默认0 可根据实际实际情况传入0-服务端自用，1-iOS，2-android，3-pc浏览器，4-手机浏览器</param>
        public static Tuple<bool, JObject> Post(string cmd, IDictionary<string, object> data, string version = "1.0.0", int useType = 0)
        {
            string url = "http://api.xiaoe-tech.com/" + cmd + "/" + version;

            data.Add("access_token", Xiaoe_Token());
            data.Add("use_type", useType);
            Console.WriteLine(JsonConvert.SerializeObject(data));

            HttpHelper http = new HttpHelper();
            var rs = http.GetHtml(new HttpItem { URL = url, Method = "POST", ContentType = "application/json", Encoding = Encoding.UTF8, Postdata = JsonConvert.SerializeObject(data) });
            return RefactResult(Yiwan.Utilities.Texter.Unicode2String(rs.Html));
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
