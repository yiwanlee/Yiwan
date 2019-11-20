using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Helpers.Cache
{
    public partial class RedisHelper
    {
        #region 同步方法

        /// <summary>
        /// 存储key-value键值对
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        public bool StringSet<T>(string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            string json = Convert2Json(value);
            return Do(db => db.StringSet(key, json, expiry, when, flags));
        }

        /// <summary>
        /// 存储多个key-value键值对
        /// </summary>
        /// <param name="keyValues">键值对集合</param>
        public bool StringSet(List<KeyValuePair<RedisKey, RedisValue>> keyValues, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddPrefixKey(p.Key), p.Value)).ToList();
            return Do(db => db.StringSet(newkeyValues.ToArray(), when, flags));
        }


        /// <summary>
        /// 获取对应key键的value值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public string StringGet(string key, CommandFlags flags = CommandFlags.None)
        {
            return StringGet<string>(key, flags);
        }

        /// <summary>
        /// 获取对应key键的value值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public T StringGet<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => Convert2Object<T>(db.StringGet(key, flags)));
        }

        /// <summary>
        /// 获取多个Key对应的value值
        /// </summary>
        /// <param name="keys">Redis的键集合</param>
        public RedisValue[] StringGet(List<string> keys, CommandFlags flags = CommandFlags.None)
        {
            RedisKey[] newKeys = keys.Select(AddPrefixKey).ToList().Select(redisKey => (RedisKey)redisKey).ToArray(); ;
            return Do(db => db.StringGet(newKeys, flags));
        }

        /// <summary>
        /// 为对应的值增长value
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值，可以为负</param>
        /// <returns>增长后的值</returns>
        public double StringIncrement(string key, double value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.StringIncrement(key, value, flags));
        }

        /// <summary>
        /// 为对应的值减少val
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值，可以为负</param>
        /// <returns>减少后的值</returns>
        public double StringDecrement(string key, double value = -1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.StringDecrement(key, value, flags));
        }

        /// <summary>
        /// 在字符串尾部追加指定的value
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <returns>返回字符串长度</returns>
        public long StringAppend(string key, string value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.StringAppend(key, value, flags));
        }

        /// <summary>
        /// 将key设置为value并返回key处存储的旧值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值，可以为负</param>
        /// <returns>存储的旧值</returns>
        public T StringGetSet<T>(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            string json = Convert2Json(value);
            return Do(db => Convert2Object<T>(db.StringGetSet(key, json, flags)));
        }

        /// <summary>
        /// 获取对应key键的value值的长度
        /// </summary>
        /// <param name="key">Redis的键</param>
        public long StringLength(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.StringLength(key, flags));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 存储key-value键值对，异步
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值，可以为负</param>
        /// <param name="expiry">过期时间</param>
        public async Task<bool> StringSetAsync<T>(string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            string json = Convert2Json(value);
            return await Do(db => db.StringSetAsync(key, json, expiry, when, flags));
        }

        /// <summary>
        /// 存储多个key-value键值对，异步
        /// </summary>
        /// <param name="keyValues">键值对</param>
        public async Task<bool> StringSetAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddPrefixKey(p.Key), p.Value)).ToList();
            return await Do(db => db.StringSetAsync(newkeyValues.ToArray(), when, flags));
        }

        /// <summary>
        /// 获取key对应的value值，异步
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public async Task<T> StringGetAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            string result = await Do(db => db.StringGetAsync(key, flags));
            return Convert2Object<T>(result);
        }

        /// <summary>
        /// 获取多个key对应的value值，异步
        /// </summary>
        /// <param name="keys">Redis的键集合</param>
        public async Task<RedisValue[]> StringGetAsync(List<string> keys, CommandFlags flags = CommandFlags.None)
        {
            RedisKey[] newKeys = keys.Select(AddPrefixKey).ToList().Select(redisKey => (RedisKey)redisKey).ToArray();
            return await Do(db => db.StringGetAsync(newKeys, flags));
        }

        /// <summary>
        /// 为对应的值增长value，异步
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值，可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> StringIncrementAsync(string key, double value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.StringIncrementAsync(key, value, flags));
        }

        /// <summary>
        /// 为对应的值减少value，异步
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值，可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> StringDecrementAsync(string key, double value = -1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.StringDecrementAsync(key, value, flags));
        }

        /// <summary>
        /// 在字符串尾部追加指定的value，异步
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <returns>返回字符串长度</returns>
        public async Task<long> StringAppendAsync(string key, string value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.StringAppendAsync(key, value, flags));
        }

        /// <summary>
        /// 将key设置为value并返回key处存储的旧值，异步
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值，可以为负</param>
        /// <returns>存储的旧值</returns>
        public async Task<T> StringGetSetAsync<T>(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            string json = Convert2Json(value);
            RedisValue result = await Do(db => db.StringGetSetAsync(key, json, flags));
            return Convert2Object<T>(result);
        }

        /// <summary>
        /// 获取对应key键的value值的长度，异步
        /// </summary>
        /// <param name="key">Redis的键</param>
        public async Task<long> StringLengthAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.StringLengthAsync(key, flags));
        }

        #endregion 异步方法
    }
}
