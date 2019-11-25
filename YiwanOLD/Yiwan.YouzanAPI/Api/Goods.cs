using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.YouzanAPI
{
    /// <summary>
    /// 商品API 管理商品，商品信息更新、上下架等
    /// </summary>
    public class Goods
    {
        /// <summary>
        /// 获取出售中的商品列表
        /// youzan.items.onsale.get
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.items.onsale/3.0.0/get
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/item/youzan.items.onsale.get
        /// </summary>
        /// <param name="order_by">排序方式。格式为column:asc/desc，目前排序字段：1.创建时间：created_time，2.更新时间：update_time，3.价格：price，4.销量：sold_num</param>
        /// <param name="page_no">页码，不传或为0时默认设置为1</param>
        /// <param name="page_size">每页条数，最大300个，不传或为0时默认设置为40</param>
        /// <param name="q">搜索字段。搜索商品的title</param>
        /// <param name="tag_id">商品标签的ID</param>
        /// <param name="update_time_start">商品初始更新时间，单位为ms</param>
        /// <param name="update_time_end">商品终止更新时间，单位为ms</param>
        public static Tuple<bool, JObject> GetOnSale(string order_by = "", int page_no = 1, int page_size = 20, string q = "", int tag_id = 0, long update_time_start = 0, long update_time_end = 0)
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "page_no", page_no },
                { "page_size", page_size }
            };
            if (!string.IsNullOrWhiteSpace(order_by)) dict.Add("order_by", order_by);
            if (!string.IsNullOrWhiteSpace(q)) dict.Add("q", q);
            if (tag_id > 0) dict.Add("tag_id", tag_id);
            if (update_time_start > 0 && update_time_end > 0)
            {
                dict.Add("update_time_start", update_time_start);
                dict.Add("update_time_end", update_time_end);
            }

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.items.onsale.get", "3.0.0", "GET", dict, null));
            return result;
        }

        /// <summary>
        /// 获取仓库中的商品列表
        /// youzan.items.inventory.get
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.items.inventory/3.0.0/get
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/item/youzan.items.inventory.get
        /// </summary>
        /// <param name="banner">分类字段。可选值：for_shelved（已下架的）/ sold_out（已售罄的）</param>
        /// <param name="order_by">排序方式。格式为column:asc/desc，目前排序字段：1.创建时间：created_time，2.更新时间：update_time，3.价格：price，4.销量：sold_num</param>
        /// <param name="page_no">页码，不传或为0时默认设置为1</param>
        /// <param name="page_size">每页条数，最大300个，不传或为0时默认设置为40</param>
        /// <param name="q">搜索字段。搜索商品的title</param>
        /// <param name="tag_id">商品标签的ID</param>
        /// <param name="update_time_start">商品初始更新时间，单位为ms</param>
        /// <param name="update_time_end">商品终止更新时间，单位为ms</param>
        public static Tuple<bool, JObject> GetOnInventory(string banner = "", string order_by = "", int page_no = 1, int page_size = 20, string q = "", int tag_id = 0, long update_time_start = 0, long update_time_end = 0)
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "page_no", page_no },
                { "page_size", page_size }
            };
            if (!string.IsNullOrWhiteSpace(banner)) dict.Add("banner", banner);
            if (!string.IsNullOrWhiteSpace(order_by)) dict.Add("order_by", order_by);
            if (!string.IsNullOrWhiteSpace(q)) dict.Add("q", q);
            if (tag_id > 0) dict.Add("tag_id", tag_id);
            if (update_time_start > 0 && update_time_end > 0)
            {
                dict.Add("update_time_start", update_time_start);
                dict.Add("update_time_end", update_time_end);
            }

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.items.inventory.get", "3.0.0", "GET", dict, null));
            return result;
        }

        /// <summary>
        /// 分页查询商品列表
        /// youzan.item.search
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.item/3.0.0/search
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/item/youzan.item.search
        /// </summary>
        /// <param name="item_ids">作为查询条件的商品ID列表，以逗号分隔，如1,2</param>
        /// <param name="page_no">页码，不传或为0时默认设置为1</param>
        /// <param name="page_size">每页条数，最大300个，不传或为0时默认设置为40</param>
        /// <param name="q">搜索字段。搜索商品的title</param>
        /// <param name="show_sold_out">是否在售: 0: 在售 1: 售罄或部分售罄 2: 全部</param>
        /// <param name="tag_ids">作为查询的分组ID列表，以逗号分隔，如1,2</param>
        public static Tuple<bool, JObject> Search(string item_ids = "", int page_no = 1, int page_size = 20, string q = "", int show_sold_out = 2, string tag_ids = "")
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "page_no", page_no },
                { "page_size", page_size }
            };
            if (!string.IsNullOrWhiteSpace(item_ids)) dict.Add("item_ids", item_ids);
            if (!string.IsNullOrWhiteSpace(q)) dict.Add("q", q);
            if (show_sold_out >= 0 && show_sold_out <= 2) dict.Add("show_sold_out", show_sold_out);
            if (!string.IsNullOrWhiteSpace(tag_ids)) dict.Add("tag_ids", tag_ids);

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.item.search", "3.0.0", "POST", dict, null));
            return result;
        }

        /// <summary>
        /// 获取单个商品信息
        /// youzan.item.get
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.item/3.0.0/get
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/item/youzan.item.get
        /// </summary>
        /// <param name="item_id">商品ID，同时存在alias时，以item_id为准，可以通过列表接口如youzan.items.onsale.get （查询出售中商品）和 youzan.items.inventory.get （查询仓库中商品）获取到</param>
        /// <param name="alias">商品别名，同时存在item_id时，则无效，可以通过列表接口如youzan.items.onsale.get （查询出售中商品）和 youzan.items.inventory.get （查询仓库中商品）获取到</param>
        public static Tuple<bool, JObject> Get(int item_id = 0, string alias = "")
        {
            if (string.IsNullOrWhiteSpace(alias) && item_id <= 0) return new Tuple<bool, JObject>(false, null);

            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (item_id > 0) dict.Add("item_id", item_id);
            if (!string.IsNullOrWhiteSpace(alias) && item_id <= 0) dict.Add("alias", alias);


            var result = YozClient.RefactResult(yzClient.Invoke("youzan.item.get", "3.0.0", "GET", dict, null));
            return result;
        }
    }
}
