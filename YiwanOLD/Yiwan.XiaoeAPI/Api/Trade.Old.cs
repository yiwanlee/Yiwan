using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yiwan.Utilities;

namespace Yiwan.XiaoeAPI.Old
{
    public class Trade
    {
        /// <summary>
        /// 下单：客户平台在本地下单的同时在小鹅通下单，以此达到用户购买的资源各个平台得到共享
        /// 接口链接：https://api.xiaoe-tech.com/open/orders.create/1.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=26
        /// </summary>
        /// <param name="user_id">购买商品的用户id</param>
        /// <param name="out_order_id">商户填入自己的订单号</param>
        /// <param name="payment_type">购买方式：2-单笔（单个商品）、3-付费产品包（专栏会员等）</param>
        /// <param name="resource_type">单笔购买时为必要参数，资源类型：1-图文、2-音频、3-视频、4-直播</param>
        /// <param name="resource_id">单笔购买时为必要参数，资源id</param>
        /// <param name="product_id">购买产品包时为必要参数，产品包id</param>
        public static Tuple<bool, JObject> Create(string user_id, string out_order_id, int payment_type, string product_id = "", int resource_type = 0, string resource_id = "")
        {
            try
            {
                SortedDictionary<string, object> data = new SortedDictionary<string, object>();
                data.Add("payment_type", payment_type.ToString());
                if (payment_type == 2)
                {
                    data.Add("resource_type", resource_type.ToString());
                    data.Add("resource_id", resource_id);
                }
                else if (payment_type == 3)
                {
                    data.Add("product_id", product_id);
                }
                else
                {
                    throw new Exception("payment_type类型只能为2、3");
                }
                data.Add("user_id", user_id);
                data.Add("out_order_id", out_order_id);

                var rs = XiaoeClient.Post("orders.create", data);

                if (rs.Item1 == true)
                {
                    string orderid = rs.Item2["data"]["order_id"].ToString();
                    var rsState = Update(orderid);
                    if (rsState.Item1 != true) return rsState;
                }
                return rs;
            }
            catch (Exception)
            {
                return new Tuple<bool, JObject>(false, null);
            }
        }

        /// <summary>
        /// 订单支付状态更新
        /// 接口链接：https://api.xiaoe-tech.com/open/orders.state.update/1.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=6
        /// </summary>
        /// <param name="user_id">购买商品的用户id</param>
        /// <param name="out_order_id">商户填入自己的订单号</param>
        /// <param name="payment_type">购买方式：2-单笔（单个商品）、3-付费产品包（专栏会员等）</param>
        /// <param name="resource_type">单笔购买时为必要参数，资源类型：1-图文、2-音频、3-视频、4-直播</param>
        /// <param name="resource_id">单笔购买时为必要参数，资源id</param>
        /// <param name="product_id">购买产品包时为必要参数，产品包id</param>
        public static Tuple<bool, JObject> Update(string order_id, bool order_state = true)
        {
            SortedDictionary<string, object> data = new SortedDictionary<string, object>();
            data.Add("order_id", order_id);
            data.Add("order_state", order_state ? 1 : 2);

            return XiaoeClient.Post("orders.state.update", data);
        }
    }
}
