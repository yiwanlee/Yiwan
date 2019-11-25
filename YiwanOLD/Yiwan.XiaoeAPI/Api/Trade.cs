using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.XiaoeAPI
{
    public class Trade
    {
        /// <summary>
        /// 验证用户是否已经购买过对应的商品
        /// 接口链接：http://api.xiaoe-tech.com/xe.get.user.orders/1.0.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=4125
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="payment_type">付费类型：2-单笔、3-付费产品包、4-团购、5-单笔的购买赠送、6-产品包的购买赠送、7-问答提问、8-问答偷听、9-购买会员、10-会员的购买赠送、11-付费活动报名、12-打赏类型</param>
        /// <param name="resource_type">资源类型：0-无（不通过资源的购买入口）、1-图文、2-音频、3-视频、4-直播、5-活动报名、6-专栏/会员、7-社群、8-大专栏、20-电子书、21-实物商品</param>
        /// <param name="product_id">资源id</param>
        /// <returns></returns>
        public static bool ValidBuyed(string user_id, int payment_type, int resource_type, string product_id)
        {
            SortedDictionary<string, object> data = new SortedDictionary<string, object>();
            data.Add("user_id", user_id);
            data.Add("data", new SortedDictionary<string, object>
            {
                { "payment_type", payment_type.ToString() },
                { "resource_type", resource_type.ToString() },
                { "product_id", product_id },
                { "order_state", 1 }
            });

            var rs = XiaoeClient.Post("xe.get.user.orders", data);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(rs.Item2));
            if (!rs.Item1) return false;
            int total = Convert.ToInt32(rs.Item2["data"]["total"]);
            return total > 0;
        }

        /// <summary>
        /// 验证用户是否已经购买过对应的商品
        /// 接口链接：http://api.xiaoe-tech.com/xe.get.user.orders/1.0.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=4125
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="payment_type">付费类型：2-单笔、3-付费产品包、4-团购、5-单笔的购买赠送、6-产品包的购买赠送、7-问答提问、8-问答偷听、9-购买会员、10-会员的购买赠送、11-付费活动报名、12-打赏类型</param>
        /// <param name="resource_type">资源类型：0-无（不通过资源的购买入口）、1-图文、2-音频、3-视频、4-直播、5-活动报名、6-专栏/会员、7-社群、8-大专栏、20-电子书、21-实物商品</param>
        /// <param name="product_id">资源id</param>
        /// <returns></returns>
        public static bool Orders(string user_id)
        {
            SortedDictionary<string, object> data = new SortedDictionary<string, object>();
            data.Add("user_id", user_id);

            var rs = XiaoeClient.Post("xe.get.user.orders", data);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(rs.Item2));
            if (!rs.Item1) return false;
            int total = Convert.ToInt32(rs.Item2["data"]["total"]);
            return total > 0;
        }
    }
}
