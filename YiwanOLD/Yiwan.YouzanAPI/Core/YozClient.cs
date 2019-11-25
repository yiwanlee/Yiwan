using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.YouzanAPI
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Configuration;
    using System.Net.Http;
    using YZOpenSDK;
    using Yiwan.Utilities.Cache;

    /// <summary>
    /// 有赞Api客户端
    /// 需要先在AppSettings中配置Yoz_ClientID,Yoz_ClientSecret,Yoz_KdtID这些是必须的,
    /// AppSettings中的Redis连接字符串Yoz_Cache_Redis是可选的如无配置则使用内存缓存
    /// 190531 修复了使用YZClient导致token失效后无法提交成功的错误
    /// </summary>
    public sealed class YozClient
    {
        private static readonly object lockObject = new object();
        private static readonly bool _useRedis = false;

        public static string ClientID = string.Empty;
        public static string ClientSecret = string.Empty;
        public static string KdtId = string.Empty;

        //静态初始化 或 Lazy<T> 可保证在使用的时候才创建
        static YozClient()
        {
            _useRedis = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Yoz_Cache_Redis"]);

            ClientID = ConfigurationManager.AppSettings["Yoz_ClientID"];
            ClientSecret = ConfigurationManager.AppSettings["Yoz_ClientSecret"];
            KdtId = ConfigurationManager.AppSettings["Yoz_KdtID"];
        }

        public static YZClient Client()
        {
            Auth auth = new Token(YZ_Token());
            return new DefaultYZClient(auth);
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
                if (jobjRes.ContainsKey("error_response"))
                    return new Tuple<bool, JObject>(false, (JObject)jobjRes["error_response"]);
                else return new Tuple<bool, JObject>(true, (JObject)jobjRes["response"]);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, JObject>(false, JObject.Parse(JsonConvert.SerializeObject(ex)));
            }
        }

        public static string YZ_Token()
        {
            #region 验证ClientID,ClientSecret,KdtId
            if (string.IsNullOrWhiteSpace(ClientID))
            {
                throw new Exception("AppSettings中没有找到Yoz_ClientID配置项或者该配置项为空字符串");
            }
            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                throw new Exception("AppSettings中没有找到Yoz_ClientSecret配置项或者该配置项为空字符串");
            }
            if (string.IsNullOrWhiteSpace(KdtId))
            {
                throw new Exception("AppSettings中没有找到Yoz_KdtID配置项或者该配置项为空字符串");
            }
            #endregion 验证ClientID,ClientSecret,KdtId

            try
            {
                string cacheKey = "Yiwan:Youzan:Token:" + KdtId + ":" + ClientID;
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
                                string url = "https://open.youzan.com/oauth/token";
                                // 创建Post请求参数
                                List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>
                                {
                                    new KeyValuePair<string, string>("client_id", ClientID),
                                    new KeyValuePair<string, string>("client_secret",ClientSecret),
                                    new KeyValuePair<string, string>("grant_type", "silent"),
                                    new KeyValuePair<string, string>("kdt_id", KdtId)
                                };
                                HttpContent postContent = new FormUrlEncodedContent(paramList);
                                HttpResponseMessage response = httpClient.PostAsync(new Uri(url), postContent).Result;
                                // 确认响应成功，否则抛出异常  
                                response.EnsureSuccessStatusCode();
                                // 异步读取响应为字符串 
                                string result = response.Content.ReadAsStringAsync().Result;
                                JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                                token = jo["access_token"].ToString();
                                if (_useRedis)
                                {
                                    redis.StringSet(cacheKey, jo["access_token"].ToString(), TimeSpan.FromSeconds(Convert.ToInt32(jo["expires_in"]) - 600));
                                }
                                else
                                {
                                    Webcache.Set(cacheKey, jo["access_token"].ToString(), TimeSpan.FromSeconds(Convert.ToInt32(jo["expires_in"]) - 600));
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
                throw new Exception("查询有赞Token出错!", ex);
            }
        }
    }
}
