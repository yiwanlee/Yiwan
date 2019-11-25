using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Utilities
{
    public class SMSer
    {
        public static Dictionary<string, object> SendSMS(string tels, string tmplId, params string[] data)
        {
            Dictionary<string, object> ret;

            SMS.CCPRestSDK api = new SMS.CCPRestSDK();
            //ip格式如下，不带https://
            bool isInit = api.init("app.cloopen.com", "8883");
            api.setAccount("8a48b5514fb1a66a014fb24dfc5a012e", "1ad50fa194bd463286bcd615faddedd7");
            api.setAppId("8a48b5514fb1a66a014fb256dd2f0152");

            try
            {
                if (isInit)
                {
                    ret = api.SendTemplateSMS(tels, tmplId, data);
                }
                else
                {
                    ret = new Dictionary<string, object> { { "statusCode", "999" }, { "statusMsg", "初始化失败" }, { "data", null } };
                }
            }
            catch (Exception ex)
            {
                ret = new Dictionary<string, object> { { "statusCode", "998" }, { "statusMsg", "发生异常：" + ex.Message }, { "data", null } };
            }
            return ret;
        }
        public static string SendCodeSMS(string tel)
        {
            string code = new Random().Next(100000, 999999).ToString();
            //return code;
            var ret = SendSMS(tel, "135430", code, "5分钟");
            if (!ret["statusCode"].ToString().Equals("000000")) code = "";
            return code;
        }
    }
}
