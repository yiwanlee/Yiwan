using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.YouzanAPI
{
    /// <summary>
    /// 用户标签API
    /// </summary>
    public class UserTags
    {
        /// <summary>
        /// 根据微信粉丝用户的 weixin_openid 或 fans_id 绑定对应的标签
        /// youzan.users.weixin.follower.tags.add
        /// 服务商接入：https://open.youzan.com/api/entry/youzan.users.weixin.follower.tags/3.0.0/add
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/tags/youzan.users.weixin.follower.tags.add
        /// </summary>
        /// <param name="weixin_openid">微信粉丝用户的openid。调用时，参数 weixin_openid 和 fans_id 选其一即可</param>
        /// <param name="fans_id">微信粉丝用户ID。调用时，参数 weixin_openid 和 fans_id 选其一即可</param>
        /// <param name="tags">标签名，多个标签名用“,”分隔</param>
        /// <param name="fields">需要返回的微信粉丝对象字段，如nick,avatar等。可选值：CrmWeixinFans微信粉丝结构体中所有字段均可返回；多个字段用“,”分隔。如果为空则返回所有</param>
        public static Tuple<bool, JObject> TagsAdd(string weixin_openid, string tags, long fans_id = 0, string fields = "")
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "tags", tags },
                { "fields", fields }
            };
            if (!string.IsNullOrWhiteSpace(weixin_openid)) dict.Add("weixin_openid", weixin_openid);
            if (fans_id > 0) dict.Add("fans_id", fans_id.ToString());

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.users.weixin.follower.tags.add", "3.0.0", "POST", dict, null));
            return result;
        }

        /// <summary>
        /// 根据微信粉丝用户的 weixin_openid 或 fans_id 删除对应的标签
        /// youzan.users.weixin.follower.tags.delete
        /// 服务商接入：https://open.youzan.com/api/entry/youzan.users.weixin.follower.tags/3.0.0/delete
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/tags/youzan.users.weixin.follower.tags.delete
        /// </summary>
        /// <param name="weixin_openid">微信粉丝用户的openid。调用时，参数 weixin_openid 和 fans_id 选其一即可</param>
        /// <param name="fans_id">微信粉丝用户ID。调用时，参数 weixin_openid 和 fans_id 选其一即可</param>
        /// <param name="tags">标签名，多个标签名用“,”分隔</param>
        public static Tuple<bool, JObject> TagsDelete(string weixin_openid, string tags, long fans_id = 0)
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "weixin_openid", weixin_openid},
                { "tags", tags }
            };
            if (string.IsNullOrWhiteSpace(weixin_openid))
            {
                dict = new Dictionary<string, object>
                {
                    { "fans_id",fans_id },
                    { "tags", tags }
                };
            }
            var result = YozClient.RefactResult(yzClient.Invoke("youzan.users.weixin.follower.tags.delete", "3.0.0", "POST", dict, null));
            return result;
        }

        /// <summary>
        /// 获取单个粉丝标签集合
        /// youzan.users.weixin.follower.tags.get
        /// 服务商接入：https://open.youzan.com/api/entry/youzan.users.weixin.follower.tags/3.0.0/get
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/tags/youzan.users.weixin.follower.tags.get
        /// </summary>
        /// <param name="weixin_openid">微信粉丝用户的openid。调用时，参数 weixin_openid 和 fans_id 选其一即可</param>
        /// <param name="fans_id">微信粉丝用户ID。调用时，参数 weixin_openid 和 fans_id 选其一即可</param>
        public static Tuple<bool, JObject> TagsGet(string weixin_openid, long fans_id = 0)
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(weixin_openid))
            {
                dict.Add("fans_id", User.GetUserID(weixin_openid).ToString());
            }
            else if (fans_id > 0) dict.Add("fans_id", fans_id.ToString());

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.users.weixin.follower.tags.get", "3.0.0", "GET", dict, null));
            return result;
        }

        public static bool TagsValid(string weixin_openid, string tagname)
        {
            try
            {
                var rsTags = TagsGet(weixin_openid);
                if (rsTags.Item1)
                {
                    var tags = (JArray)rsTags.Item2["tags"];
                    for (int i = 0; i < tags.Count; i++) if (tags[i]["tag_name"].ToString().Equals(tagname)) return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}
