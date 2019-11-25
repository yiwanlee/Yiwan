using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.XiaoeAPI
{
    public class User
    {
        /// <summary>
        /// 获取用户信息接口：查询用户信息
        /// 接口链接：https://api.xiaoe-tech.com/xe.user.info.get/1.0.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=4162
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="payment_type">付费类型：2-单笔、3-付费产品包、4-团购、5-单笔的购买赠送、6-产品包的购买赠送、7-问答提问、8-问答偷听、9-购买会员、10-会员的购买赠送、11-付费活动报名、12-打赏类型</param>
        /// <param name="resource_type">资源类型：0-无（不通过资源的购买入口）、1-图文、2-音频、3-视频、4-直播、5-活动报名、6-专栏/会员、7-社群、8-大专栏、20-电子书、21-实物商品</param>
        /// <param name="product_id">资源id</param>
        public static (bool isSuccess, JObject data) Get(string unionid = "", string user_id = "", string phone = "")
        {
            SortedDictionary<string, object> data = new SortedDictionary<string, object>();
            if (!string.IsNullOrEmpty(user_id)) data.Add("user_id", user_id);

            SortedDictionary<string, object> dataChild = new SortedDictionary<string, object>();
            dataChild.Add("field_list", new string[] { "name", "nickname", "wx_avatar", "gender", "city", "province", "country", "phone", "wx_account" });
            if (!string.IsNullOrEmpty(unionid)) dataChild.Add("wx_union_id", unionid);
            if (!string.IsNullOrEmpty(phone)) dataChild.Add("phone", phone);

            data.Add("data", dataChild);

            //Console.WriteLine(JsonConvert.SerializeObject(data));

            var rs = XiaoeClient.Post("xe.user.info.get", data);
            return (rs.Item1, rs.Item2);
        }

        /// <summary>
        /// 注册新用户
        /// 接口链接：https://api.xiaoe-tech.com/xe.user.register/1.0.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=4157
        /// </summary>
        /// <param name="unionid">微信 union_id</param>
        /// <param name="nickname">微信 用户昵称</param>
        /// <param name="avatar">用户头像链接</param>
        /// <param name="country">国家</param>
        /// <param name="province">省份</param>
        /// <param name="city">城市</param>
        /// <param name="gender">性别</param>
        public static Tuple<bool, JObject> Create(string unionid, string nickname = "", string avatar = "", string country = "", string province = "", string city = "", int gender = 0, string phone = "")
        {
            SortedDictionary<string, object> data = new SortedDictionary<string, object>();
            Dictionary<string, object> dataChild = new Dictionary<string, object>();
            dataChild.Add("wx_union_id", unionid);
            //dataChild.Add("phone", phone);
            //dataChild.Add("avatar", avatar);
            //dataChild.Add("nickname", nickname);
            //dataChild.Add("country", country);
            //dataChild.Add("province", province);
            //dataChild.Add("city", city);
            //dataChild.Add("gender", gender);

            data.Add("data", dataChild);

            var rs = XiaoeClient.Post("xe.user.register", data);
            if (rs.Item1)
            {
                string user_id = rs.Item2["data"]["user_id"].ToString();
                var rsUpdate = Update(user_id, nickname, avatar, country, province, city, gender, phone);
                //Console.WriteLine("rsUpdate：" + Newtonsoft.Json.JsonConvert.SerializeObject(rsUpdate));
            }
            return rs;
        }

        /// <summary>
        /// 更新用户信息
        /// 接口链接：https://api.xiaoe-tech.com/xe.user.info.update/1.0.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=4161
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="nickname">微信 用户昵称</param>
        /// <param name="avatar">用户头像链接</param>
        /// <param name="country">国家</param>
        /// <param name="province">省份</param>
        /// <param name="city">城市</param>
        /// <param name="gender">性别</param>
        public static Tuple<bool, JObject> Update(string user_id, string nickname = "", string avatar = "", string country = "", string province = "", string city = "", int gender = 0, string phone = "")
        {
            SortedDictionary<string, object> data = new SortedDictionary<string, object>();
            data.Add("user_id", user_id);
            Dictionary<string, object> dataChild = new Dictionary<string, object>();
            Dictionary<string, object> dataChild_Child = new Dictionary<string, object>();
            //dataChild_Child.Add("phone", phone);
            dataChild_Child.Add("avatar", avatar);
            //dataChild_Child.Add("nickname", nickname);
            //dataChild_Child.Add("country", country);
            //dataChild_Child.Add("province", province);
            //dataChild_Child.Add("city", city);
            //dataChild_Child.Add("gender", gender);

            dataChild.Add("update_data", dataChild_Child);
            data.Add("data", dataChild);

            var rs = XiaoeClient.Post("xe.user.info.update", data);
            return rs;
        }
    }
}
