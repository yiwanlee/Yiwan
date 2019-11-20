using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Helpers.Utilities
{
    public class JsonUtility
    {
        /// <summary>
        /// 序列化Object对象为Json字符串
        /// </summary>
        public static string ConvertToJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        /// <summary>
        /// 反序列化Json字符串为Object对象
        /// </summary>
        public static T ConvertToObject<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;
            else if (typeof(T).Name.Equals(typeof(string).Name, StringComparison.CurrentCulture))
            {
                return JsonConvert.DeserializeObject<T>($"'{value}'");
            }
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
