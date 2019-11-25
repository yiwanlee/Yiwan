using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.YouzanAPI
{
    /// <summary>
    /// 商品分组API 管理商品分组
    /// </summary>
    public class GoodsCategory
    {
        /// <summary>
        /// 查询商品分组
        /// youzan.itemcategories.tags.get
        /// 服务商接入：https://open.youzan.com/api/oauthentry/youzan.itemcategories.tags/3.0.0/get
        /// 文档页面：https://open.youzan.com/v3/apicenter/doc-api-main/1/2/item_category/youzan.itemcategories.tags.get
        /// </summary>
        /// <param name="is_sort">是否排序</param>
        public static Tuple<bool, JObject> GetTags(bool is_sort = false)
        {
            var yzClient = YozClient.Client();
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "is_sort", is_sort}
            };

            var result = YozClient.RefactResult(yzClient.Invoke("youzan.itemcategories.tags.get", "3.0.0", "GET", dict, null));
            return result;
        }
    }
}
