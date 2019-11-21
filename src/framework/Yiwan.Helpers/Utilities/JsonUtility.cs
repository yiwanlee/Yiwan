using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Yiwan.Helpers.Utilities
{
    public static class JsonUtility
    {
        /// <summary>
        /// 序列化Object对象为Json字符串
        /// </summary>
        public static string ConvertToJson<T>(T value)
        {
            if (value == null) return null;
            else return value is string ? value.ToString() : JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// 反序列化Json字符串为Object对象
        /// </summary>
        public static T ConvertToObject<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;
            else if (typeof(T).Name.Equals(typeof(string).Name, StringComparison.CurrentCulture))
                return JsonConvert.DeserializeObject<T>($"'{value}'");
            else return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 反序列化Json字符串为Object对象（用于RedisHelper）
        /// </summary>
        public static T ConvertToObject<T>(RedisValue value)
        {
            if (value.IsNull) return default;
            else if (typeof(T).Name.Equals(typeof(string).Name, StringComparison.CurrentCulture))
                return JsonConvert.DeserializeObject<T>($"'{value}'");
            else return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 反序列化Json字符串集合为Object集合
        /// </summary>
        public static List<T> ConvertToList<T>(string[] values)
        {
            List<T> result = new List<T>();

            if (values == null || values.Length == 0) return result;

            foreach (string item in values)
            {
                T model = ConvertToObject<T>(item);
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 反序列化Json字符串集合为Object集合（用于RedisHelper）
        /// </summary>
        public static List<T> ConvertToList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();

            if (values == null || values.Length == 0) return result;

            foreach (RedisValue item in values)
            {
                T model = ConvertToObject<T>(item);
                result.Add(model);
            }
            return result;
        }
    }
}
