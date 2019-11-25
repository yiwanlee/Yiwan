using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.YouzanAPI
{
    /// <summary>
    /// 积分API 管理用户积分
    /// </summary>
    public class Points
    {
        /// <summary>
        /// 获取用户积分
        /// youzan.crm.fans.points.get
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.crm.fans.points/3.0.1/get
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/points/youzan.crm.fans.points.get
        /// </summary>
        /// <param name="fans_id">粉丝id fans_id/open_user_id/mobile 三选一传入</param>
        /// <param name="mobile">手机号 fans_id/open_user_id/mobile 三选一传入</param>
        /// <param name="open_user_id">三方用户ID，三方打通帐号后可以使用，目前仅app开店用户可以使用。 fans_id/open_user_id/mobile 三选一传入</param>
        public static Tuple<bool, JObject> Get(long fans_id = 0, string mobile = "", string open_user_id = "")
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (fans_id > 0) dict.Add("fans_id", fans_id);
            if (!string.IsNullOrWhiteSpace(mobile)) dict.Add("mobile", mobile);
            if (!string.IsNullOrWhiteSpace(open_user_id)) dict.Add("open_user_id", open_user_id);

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.crm.fans.points.get", "3.0.1", "GET", dict, null));
            return result;
        }

        /// <summary>
        /// 给客户同步积分
        /// youzan.crm.customer.points.sync
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.crm.customer.points/3.1.0/sync
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/points/youzan.crm.customer.points.sync
        /// </summary>
        /// <param name="account_id">帐号ID,默认openid</param>
        /// <param name="points">积分值</param>
        /// <param name="reason">积分变动原因</param>
        /// <param name="account_type">帐号类型（与帐户ID配合使用: 2=粉丝(原fansId),3:手机号,4:三方帐号(原open_user_id);6:微信open_id）</param>
        /// <param name="biz_value">用于幂等支持（幂等时效三个月, 超过三个月的相同值调用不保证幂等）</param>
        public static Tuple<bool, JObject> Sync(string account_id = "", int points = 0, string reason = "", int account_type = 6, string biz_value = "")
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(account_id)) dict.Add("account_id", account_id);
            int[] typeArray = new int[] { 2, 3, 4, 6 };
            if (typeArray.Contains(account_type)) dict.Add("account_type", account_type);
            points = points < 0 ? 0 : points;
            if (points >= 0) dict.Add("points", points);
            if (!string.IsNullOrWhiteSpace(reason)) dict.Add("reason", reason);
            if (!string.IsNullOrWhiteSpace(biz_value)) dict.Add("biz_value", biz_value);

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.crm.customer.points.sync", "3.1.0", "GET", dict, null));
            return result;
        }
    }
}
