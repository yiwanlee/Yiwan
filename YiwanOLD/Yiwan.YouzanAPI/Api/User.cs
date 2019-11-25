using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.YouzanAPI
{
    /// <summary>
    /// 用户API
    /// 用户是有赞层面通过技术手段上能够识别的自然人。包含查询用户信息相关的接口
    /// </summary>
    public class User
    {
        /// <summary>
        /// 根据after_fans_id正序批量查询微信粉丝用户信息
        /// youzan.users.weixin.followers.info.pull
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.users.weixin.followers.info/3.0.0/pull
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/user/youzan.users.weixin.followers.info.pull
        /// </summary>
        /// <param name="after_fans_id">用于拉取该粉丝编码之后的查询条件。第一次查询可传入0，之后每次查询可传入上次查询里返回的last_fans_id，直到返回结果里的has_next为false</param>
        /// <param name="fields">需要返回的除微信粉丝基础信息外的资产信息。枚举值：points，trade，level。points可获取“points”字段，trade可获取”traded_num,trade_money”两个字段，level可获取”level_info”字段信息。传多个枚举值需用“,”分隔，如果该字段为空则只返回粉丝基础信息。默认为空。(“fields”字段传入枚举值越多，查询数据耗费时间越长。）</param>
        /// <param name="page_size">每页条数，最大值：50 如果接口频繁抛异常，且入参无误，请减小page_size并重试。 </param>
        public static Tuple<bool, JObject> Pull(long after_fans_id, string fields, int page_size)
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "after_fans_id",after_fans_id },
                { "fields", fields},
                { "page_size", page_size }
            };
            var result = YozClient.RefactResult(yzClient.Invoke("youzan.users.weixin.followers.info.pull", "3.0.0", "GET", dict, null));
            return result;
        }

        /// <summary>
        /// 根据微信粉丝用户的 weixin_openid 或 fans_id 获取用户信息
        /// youzan.users.weixin.follower.get
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.users.weixin.follower/3.0.0/get
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/user/youzan.users.weixin.follower.get
        /// </summary>
        /// <param name="weixin_openid">微信粉丝用户的openid</param>
        /// <param name="fans_id">微信粉丝用户ID。 调用时，参数 weixin_openid 和 fans_id 选其一即可</param>
        /// <param name="fields">需要返回的微信粉丝对象字段，如nick,avatar等。可选值：CrmWeixinFans微信粉丝结构体中所有字段均可返回；多个字段用“,”分隔。如果为空则返回所有</param>
        /// <returns></returns>
        public static Tuple<bool, JObject> Get(string weixin_openid = "", long fans_id = 0, string fields = "")
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(weixin_openid)) dict.Add("weixin_openid", weixin_openid);
            if (fans_id > 0) dict.Add("fans_id", fans_id);
            if (!string.IsNullOrWhiteSpace(fields)) dict.Add("fields", weixin_openid);

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.users.weixin.follower.get", "3.0.0", "GET", dict, null));
            return result;
        }

        public static long GetUserID(string weixin_openid)
        {
            try
            {
                var rsUser = Get(weixin_openid);
                if (rsUser.Item1) return Convert.ToInt64(rsUser.Item2["user"]["user_id"]);
                else return 0;
            }
            catch { return 0; }
        }
    }
}
