using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.YouzanAPI
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// 订单管理API
    /// 商家管理订单
    /// </summary>
    public class Trade
    {
        /// <summary>
        /// 用于订单查询的时间类型
        /// </summary>
        public enum TimeType
        {
            /// <summary>
            /// 交易创建时间
            /// </summary>
            CREATED = 1,
            /// <summary>
            /// 交易状态更新时间
            /// </summary>
            UPDATED = 2
        }
        /// <summary>
        /// 增加/修改订单备注
        /// youzan.trade.memo.update
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.trade.memo/3.0.0/update
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/trade/youzan.trade.memo.update
        /// </summary>
        /// <param name="tid">订单号</param>
        /// <param name="memo">订单备注（会自动替换#为空，因为#会发生未知错误提示Token失效）</param>
        /// <param name="flag">订单备注加星标，取值为1-5</param>
        public static Tuple<bool, JObject> MemoUpdate(string tid, string memo = "", string flag = "")
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "tid",tid },
                { "memo", memo.Replace("#","") }
            };
            if (int.TryParse(flag, out int flagNum)) //判断flag值是否合法
            {
                if (flagNum >= 0 && flagNum <= 5) dict.Add("flag", flagNum);
            }
            var result = YozClient.RefactResult(yzClient.Invoke("youzan.trade.memo.update", "3.0.0", "GET", dict, null));
            return result;
        }

        /// <summary>
        /// 订单列表4.0接口
        /// youzan.trades.sold.get
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.trades.sold/4.0.0/get
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/trade/youzan.trades.sold.get
        /// </summary>
        /// <param name="start_time">开始时间，例:2017-01-01 12:00:00; 开始时间和结束时间的跨度不能大于3个月; 结束时间必须大于开始时间; 开始时间和结束时间必须成对出现</param>
        /// <param name="end_time">结束时间，例:2017-01-01 12:00:00; 开始时间和结束时间的跨度不能大于3个月; 结束时间必须大于开始时间; 开始时间和结束时间必须成对出现</param>
        /// <param name="time_type">查询时间的类型(交易创建时间/交易状态更新时间)</param>
        /// <param name="page_no">页码，默认1，从1开始，最大不能超过100</param>
        /// <param name="page_size">每页条数，默认20，最大不能超过100，建议使用默认分页 20</param>
        /// <param name="fans_id">粉丝id</param>
        /// <param name="fans_type">粉丝类型</param>
        /// <param name="buyer_id">买家id</param>
        /// <param name="tid">订单号</param>
        /// <param name="status">订单状态，一次只能查询一种状态 待付款：WAIT_BUYER_PAY 待发货：WAIT_SELLER_SEND_GOODS 等待买家确认：WAIT_BUYER_CONFIRM_GOODS 订单完成：TRADE_SUCCESS 订单关闭：TRADE_CLOSE 退款中：TRADE_REFUND</param>
        /// <param name="type">订单类型 NORMAL：普通订单 PEERPAY：代付 GIFT：我要送人 FX_CAIGOUDAN：分销采购单 PRESENT：赠品 WISH：心愿单 QRCODE：二维码订单 QRCODE_3RD：线下收银台订单 FX_MERGED：合并付货款 VERIFIED：1分钱实名认证 PINJIAN：品鉴 REBATE：返利 FX_QUANYUANDIAN：全员开店 FX_DEPOSIT：保证金 PF：批发 GROUP：拼团 HOTEL：酒店 TAKE_AWAY：外卖 CATERING_OFFLINE：堂食点餐 CATERING_QRCODE：外卖买单 BEAUTY_APPOINTMENT：美业预约单 BEAUTY_SERVICE：美业服务单 KNOWLEDGE_PAY：知识付费 GIFT_CARD：礼品卡</param>
        /// <param name="item_id">商品id</param>
        /// <param name="keywords">通用搜索关键字</param>
        /// <param name="goods_title">商品名称</param>
        /// <param name="receiver_name">收货人昵称</param>
        /// <param name="receiver_phone">收货人手机号</param>
        /// <param name="need_order_url">是否需要返回订单详情url</param>
        /// <param name="express_type">物流类型搜索 同城送订单：LOCAL_DELIVERY 自提订单：SELF_FETCH 快递配送：EXPRESS</param>
        /// <param name="order_source">来源</param>
        /// <param name="offline_id">门店id</param>
        public static Tuple<bool, JObject> SoldGet(DateTime? start_time, DateTime? end_time, TimeType? time_type, int page_no = 1, int page_size = 20,
            long fans_id = 0, int fans_type = 0, long buyer_id = 0, string tid = "", string status = "", string type = "",
            long item_id = 0, string keywords = "", string goods_title = "", string receiver_name = "", string receiver_phone = "",
            bool need_order_url = true, string express_type = "", string order_source = "", long offline_id = 0)
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "page_no",page_no },
                { "page_size", page_size },
                { "need_order_url", need_order_url }
            };
            #region 根据是否默认值确定是否添加到请求条件的dict中
            //时间
            if (start_time != null && time_type == TimeType.CREATED)
            {
                dict.Add("start_created", start_time);
                dict.Add("end_created", end_time);
            }
            else if (start_time != null && time_type == TimeType.UPDATED) //这里也可以不用加条件
            {
                dict.Add("start_update", start_time);
                dict.Add("end_update", end_time);
            }
            if (fans_id > 0) dict.Add("fans_id", fans_id); //粉丝ID
            if (fans_type > 0) dict.Add("fans_type", fans_type); //粉丝类型
            if (buyer_id > 0) dict.Add("buyer_id", buyer_id); //买家id
            if (!string.IsNullOrWhiteSpace(tid)) dict.Add("tid", tid); //订单号
            if (!string.IsNullOrWhiteSpace(status)) dict.Add("status", status); //订单状态
            if (!string.IsNullOrWhiteSpace(type)) dict.Add("type", type); //订单类型
            if (item_id > 0) dict.Add("item_id", item_id); //商品id
            if (!string.IsNullOrWhiteSpace(keywords)) dict.Add("keywords", keywords); //通用搜索关键字
            if (!string.IsNullOrWhiteSpace(goods_title)) dict.Add("goods_title", goods_title); //商品名称
            if (!string.IsNullOrWhiteSpace(receiver_name)) dict.Add("receiver_name", receiver_name); //收货人昵称
            if (!string.IsNullOrWhiteSpace(receiver_phone)) dict.Add("receiver_phone", goods_title); //收货人手机号
            if (!string.IsNullOrWhiteSpace(express_type)) dict.Add("express_type", express_type); //物流类型
            if (!string.IsNullOrWhiteSpace(order_source)) dict.Add("order_source", order_source); //来源
            if (offline_id > 0) dict.Add("offline_id", offline_id); //门店id
            #endregion 根据是否默认值确定是否添加到请求条件的dict中

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.trades.sold.get", "4.0.0", "GET", dict, null));
            return result;
        }
    }
}
