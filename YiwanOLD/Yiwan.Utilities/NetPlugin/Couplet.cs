using System;
using System.Text;

/// <summary>
/// 网络小插件小游戏
/// </summary>
namespace Yiwan.Utilities.NetPlugin
{
    //using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 对对联
    /// </summary>
    public class Couplet
    {
        /// <summary>
        /// 对对联
        /// </summary>
        /// <param name="left">上联</param>
        //public static Tuple<bool, string, string> Made(string left)
        //{
        //    try
        //    {
        //        left = ChineseConverter.Convert(left, ChineseConversionDirection.TraditionalToSimplified); //转换繁体
        //        MatchCollection match = Regex.Matches(left, "[\u4e00-\u9fa5]+"); //匹配汉字
        //        StringBuilder sbLeft = new StringBuilder();
        //        for (int i = 0; i < match.Count; i++) sbLeft.Append(match[i].Value); //组合下联
        //        left = sbLeft.ToString(); //过滤后的上联
        //        if (left.Length > 50) return new Tuple<bool, string, string>(false, "您的上联太长了，企鹅君实在无能为力", "");
        //        var http = new HttpHelper();
        //        //创建Httphelper参数对象
        //        HttpItem item = new HttpItem()
        //        {
        //            URL = "https://ai-backend.binwang.me/chat/couplet/" + left,//URL     必需项    
        //            Method = "GET",//URL     可选项 默认为Get   
        //            ContentType = "application/json",//返回类型    可选项有默认值
        //        };
        //        //请求的返回值对象
        //        HttpResult result = http.GetHtml(item);
        //        //获取请请求的Html
        //        string json = result.Html;
        //        //获取请求的Cookie
        //        //string cookie = result.Cookie;
        //        var jobj = (JObject)JsonConvert.DeserializeObject(json);
        //        return new Tuple<bool, string, string>(true, left, jobj["output"].ToString());
        //        //return new Tuple<bool, string, string>(true, left, json);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Tuple<bool, string, string>(false, "对不起，网络超时了", ex.Message);
        //    }
        //}
    }
}
