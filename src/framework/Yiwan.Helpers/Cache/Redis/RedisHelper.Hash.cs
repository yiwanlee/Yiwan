using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yiwan.Helpers.Cache
{
    public partial class RedisHelper
    {
        #region 同步方法

        /// <summary>
        /// 判断某个数据是否已经存在
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        public bool HashExists(string key, string dataKey, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.HashExists(key, dataKey, flags));
        }

        /// <summary>
        /// 获取Hash键数量
        /// </summary>
        /// <param name="key">Redis的键</param>
        public long HashLength(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.HashLength(key, flags));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        /// <param name="value">Hash属性的值</param>
        public bool HashSet<T>(string key, string dataKey, T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db =>
            {
                string json = Utilities.JsonUtility.ConvertToJson(value);
                return db.HashSet(key, dataKey, json, when, flags);
            });
        }

        /// <summary>
        /// 存储数据到hash表，集合
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        /// <param name="value">Hash属性的值</param>
        public void HashSet(string key, List<KeyValuePair<string, object>> listEntry, CommandFlags flags = CommandFlags.None)
        {
            if (listEntry == null || listEntry.Count == 0) return;

            key = AddPrefixKey(key);
            IDatabase db = _conn.GetDatabase(DbNum);

            List<HashEntry> entrys = new List<HashEntry>();
            foreach (KeyValuePair<string, object> item in listEntry)
            {
                entrys.Add(new HashEntry(item.Key, Utilities.JsonUtility.ConvertToJson(item.Value)));
            }

            db.HashSet(key, entrys.ToArray(), flags);
        }

        /// <summary>
        /// 移除hash表中的某值
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        public bool HashDelete(string key, string dataKey, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.HashDelete(key, dataKey, flags));
        }

        /// <summary>
        /// 移除hash表中的多个键数据
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKeys">Hash属性的键集合</param>
        public long HashDelete(string key, List<RedisValue> dataKeys, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.HashDelete(key, dataKeys.ToArray(), flags));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        public T HashGet<T>(string key, string dataKey, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db =>
            {
                string value = db.HashGet(key, dataKey, flags);
                return Utilities.JsonUtility.ConvertToObject<T>(value);
            });
        }

        /// <summary>
        /// 为数字增长value
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        /// <param name="value">增加的value,可以为负</param>
        /// <returns>增长后的值</returns>
        public double HashIncrement(string key, string dataKey, double value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.HashIncrement(key, dataKey, value, flags));
        }

        /// <summary>
        /// 为数字减少value
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        /// <param name="value">减少的value,可以为负</param>
        /// <returns>减少后的值</returns>
        public double HashDecrement(string key, string dataKey, double value = -1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.HashDecrement(key, dataKey, value, flags));
        }

        /// <summary>
        /// 获取hash所有键
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public List<T> HashKeys<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db =>
            {
                return db.HashKeys(key, flags).Select(redisKey => Utilities.JsonUtility.ConvertToObject<T>(redisKey)).ToList();
            });
        }

        /// <summary>
        /// 获取hash所有值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public List<T> HashValues<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db =>
            {
                return db.HashValues(key, flags).Select(redisValue => Utilities.JsonUtility.ConvertToObject<T>(redisValue)).ToList();
            });
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 判断某个数据是否已经存在
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        public async Task<bool> HashExistsAsync(string key, string dataKey, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.HashExistsAsync(key, dataKey, flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取Hash键数量
        /// </summary>
        /// <param name="key">Redis的键</param>
        public async Task<long> HashLengthAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.HashLengthAsync(key, flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        /// <param name="value">Hash属性的值</param>
        public async Task<bool> HashSetAsync<T>(string key, string dataKey, T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db =>
            {
                string json = Utilities.JsonUtility.ConvertToJson(value);
                return db.HashSetAsync(key, dataKey, json, when, flags).ConfigureAwait(false);
            });
        }

        /// <summary>
        /// 移除hash表中的某值
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        public async Task<bool> HashDeleteAsync(string key, string dataKey, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.HashDeleteAsync(key, dataKey, flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 移除hash表中的多个键数据
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKeys">Hash属性的键集合</param>
        public async Task<long> HashDeleteAsync(string key, List<RedisValue> dataKeys, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.HashDeleteAsync(key, dataKeys.ToArray(), flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        public async Task<T> HashGetAsync<T>(string key, string dataKey, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            string value = await Do(db => db.HashGetAsync(key, dataKey, flags)).ConfigureAwait(false);
            return Utilities.JsonUtility.ConvertToObject<T>(value);
        }

        /// <summary>
        /// 为数字增长value
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        /// <param name="value">增加的value,可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.HashIncrementAsync(key, dataKey, value, flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 为数字减少value
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="dataKey">Hash属性的键</param>
        /// <param name="value">减少的value,可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double value = 1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.HashDecrementAsync(key, dataKey, value, flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取hash所有键
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public async Task<List<T>> HashKeysAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            RedisValue[] keys = await Do(db => db.HashKeysAsync(key, flags)).ConfigureAwait(false);
            return Utilities.JsonUtility.ConvertToList<T>(keys);
        }

        /// <summary>
        /// 获取hash所有键
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public async Task<List<T>> HashValuesAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            RedisValue[] values = await Do(db => db.HashValuesAsync(key, flags)).ConfigureAwait(false);
            return Utilities.JsonUtility.ConvertToList<T>(values);
        }

        #endregion 异步方法
    }
}
