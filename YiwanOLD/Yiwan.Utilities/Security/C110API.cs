using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yiwan.Utilities.Cache;

namespace Yiwan.Utilities
{
    public static class C110API
    {
        public static string Appid = "1261187121", Appkey = "552fb12f9c2046edb889a5b3e6e6a96d", Host = "http://testapi.diy.heku123.com", Account = "qiebaba", Password = "qiebaba";

        public static Tuple<int, string, string, string> GetToken()
        {
            try
            {
                RedisHelper redis = new RedisHelper(1);
                var cacheToken = redis.StringGet<string>("C110:TOKEN");
                if (string.IsNullOrWhiteSpace(cacheToken)) //查询Token
                {
                    long timestamp = GetTimestamp();
                    var postData = new Dictionary<string, object>
                    {
                        { "Account", Account},
                        { "Password", Password },
                        { "AppID", Appid }
                    };
                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(postData);
                    string jsonSHA1 = Security.SHA1_Encrypt(Appid + Appkey + jsonData + timestamp);

                    string url = $"{Host}/api/ApiDealerAccount/GetToken";
                    HttpHelper http = new HttpHelper();
                    var rs = http.GetHtml(new HttpItem { URL = $"{url}?Signature={jsonSHA1}&Timestamp={timestamp}", Method = "POST", ContentType = "application/x-www-form-urlencoded", Postdata = SortParams(postData) });
                    cacheToken = rs.Html;
                    redis.StringSet("C110:TOKEN", cacheToken, TimeSpan.FromMinutes(20));
                }
                var jsonToken = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(cacheToken);
                Console.WriteLine(cacheToken);
                int code = Convert.ToInt32(jsonToken["code"].ToString());
                string msg = jsonToken["msg"].ToString();
                string token = "", editor = "";
                if (code == 1)
                {
                    token = jsonToken["data"]["Token"].ToString();
                    editor = jsonToken["data"]["EditorUrl"].ToString();
                }
                return new Tuple<int, string, string, string>(code == 1 ? 0 : 1, token, editor, msg); //反转code，code变为error含义，=0时正确
            }
            catch (Exception ex)
            {
                NLogger.Current.Error(ex);
                return new Tuple<int, string, string, string>(999, "", "", ex.Message);
            }
        }

        public static string GetEditor(string tid)
        {
            var appToken = GetToken();
            string token = appToken.Item2;
            string editor = appToken.Item3;

            long timestamp = GetTimestamp();
            var postData = new Dictionary<string, object>
            {
                { "AppID", Appid},
                { "Token", token },
                { "PreOrderNo", tid }
            };
            string sortData = SortValues(postData);
            string sortSHA1 = Security.SHA1_Encrypt(Appid + Appkey + sortData + timestamp);

            return editor + $"?Signature={sortSHA1}&Timestamp={timestamp}&AppID={Appid}&Token={token}&PreOrderNo={tid}";
        }

        public static string Signature(Dictionary<string, object> data, bool useJson = false)
        {
            long timestamp = GetTimestamp();
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            string sortData = SortParams(data);
            string jsonSHA1 = Security.SHA1_Encrypt(Appid + Appkey + jsonData + timestamp);
            string sortSHA1 = Security.SHA1_Encrypt(Appid + Appkey + sortData + timestamp);
            return useJson ? jsonSHA1 : sortSHA1;
        }

        #region 私有函数
        public static long GetTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToUniversalTime().Ticks) / 10000;
        }
        public static string SortParams(Dictionary<string, object> data, bool useSort = true)
        {
            var sortdict = useSort ? (from dict in data orderby dict.Key ascending select dict) : (from dict in data select dict);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, object> kv in sortdict)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value == null ? "" : kv.Value.ToString();
                str.Append(pkey + "=" + pvalue + "&");
            }
            string result = str.ToString().Substring(0, str.ToString().Length - 1);
            return result;
        }
        public static string SortValues(Dictionary<string, object> data, bool useSort = true)
        {
            var sortdict = useSort ? (from dict in data orderby dict.Key ascending select dict) : (from dict in data select dict);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, object> kv in sortdict)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value.ToString();
                str.Append(pvalue);
            }
            string result = str.ToString();
            return result;
        }
        #endregion 私有函数
    }
}
