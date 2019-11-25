using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.XiaoeAPI.Old
{
    public class User
    {
        /// <summary>
        /// 获取用户信息接口
        /// 接口链接：https://api.xiaoe-tech.com/open/users.getinfo/1.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=42
        /// </summary>
        /// <param name="unionid">微信开放平台的union_id</param>
        /// <param name="user_id">小鹅通用户id</param>
        /// <param name="phone">用户绑定的手机号</param>
        public static Tuple<bool, JObject> Get(string unionid = "", string user_id = "", string phone = "")
        {
            SortedDictionary<string, object> data = new SortedDictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(unionid)) data.Add("wx_union_id", unionid);
            if (!string.IsNullOrWhiteSpace(user_id)) data.Add("user_id", user_id);
            if (!string.IsNullOrWhiteSpace(phone)) data.Add("phone", phone);

            return XiaoeClient.Post("users.getinfo", data);
        }

        /// <summary>
        /// 用户注册接口
        /// 接口链接：https://api.xiaoe-tech.com/open/users.register/1.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=2
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
            data.Add("wx_union_id", unionid);
            //data.Add("avatar", avatar);
            //data.Add("gender", gender);
            //data.Add("nick_name", nickname);
            //data.Add("city", city);
            //data.Add("province", province);
            //data.Add("country", country);
            //data.Add("phone", phone);

            var rs = XiaoeClient.Post("users.register", data);

            var rsGet = Get(unionid);
            if (rsGet.Item1)
            {
                //Console.WriteLine("rsGet", JsonConvert.SerializeObject(rsGet.Item2));
                JArray datas = (JArray)rsGet.Item2["data"];
                if (datas.Count > 0)
                {
                    string user_id = datas[0]["user_id"].ToString();
                    var rsUpdate = Update(user_id, nickname, avatar, country, province, city, gender, phone);
                    //Console.WriteLine("rsUpdate", JsonConvert.SerializeObject(rsUpdate.Item2));
                }
            }
            return rs;
        }

        /// <summary>
        /// 更新用户信息
        /// 接口链接：https://api.xiaoe-tech.com/open/users.modifyinfo/1.0
        /// 文档页面：http://doc.xiaoeknow.com/web/#/2?page_id=18
        /// </summary>
        /// <param name="unionid">微信 union_id</param>
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
            data.Add("avatar", avatar);
            //data.Add("gender", gender);
            //data.Add("nick_name", nickname);
            //data.Add("city", city);
            //data.Add("province", province);
            //data.Add("country", country);
            //data.Add("phone", phone);

            return XiaoeClient.Post("users.modifyinfo", data);
        }
    }
}
