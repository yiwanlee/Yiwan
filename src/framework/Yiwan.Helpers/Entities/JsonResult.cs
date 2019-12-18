using Newtonsoft.Json;

namespace Yiwan.Helpers.Entities
{
    public class JsonResult
    {
        /// <summary>
        /// 响应结果码
        /// 0:成功无错误 1:失败未指定错误 以及其他结果码
        /// </summary>
        [JsonProperty("code")]
        public int code { get; set; }

        /// <summary>
        /// 返回信息提示，成功失败均可显示的友好文字，可为空
        /// </summary>
        [JsonProperty("msg")]
        public string msg { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        [JsonProperty("data")]
        public object data { get; set; }

        /// <summary>
        /// 若请求失败，此处记录失败详细原因，若code为1，此处可以为空
        /// </summary>
        [JsonProperty("description")]
        public string description { get; set; }

        /// <summary>
        /// ToJSONString
        /// </summary>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 构造函数
        /// 返回仅状态码和信息提示等的结果(无数据)
        /// </summary>
        public JsonResult() { }

        /// <summary>
        /// 构造函数
        /// 返回仅状态码和信息提示等的结果(无数据)
        /// </summary>
        public JsonResult(int code, string msg = null, string description = null)
        {
            this.code = code;
            this.msg = msg;
            this.description = description;
        }

        /// <summary>
        /// 构造函数
        /// 返回状态码和返回数据等的结果
        /// </summary>
        public JsonResult(int code, object data, string msg = null, string description = null)
        {
            this.code = code;
            this.data = data;
            this.msg = msg;
            this.description = description;
        }
    }
}
