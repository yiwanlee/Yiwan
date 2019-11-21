using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yiwan.Helpers.Cache
{
    public partial class RedisHelper
    {
        #region 同步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <param name="count">count>0从头部删除，count<0从尾部删除</param>
        public void ListRemove<T>(string key, T value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            Do(db => db.ListRemove(key, Utilities.JsonUtility.ConvertToJson(value), count, flags));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="start">开始的位置</param>
        /// <param name="stop">结束的位置，包含</param>
        public List<T> ListRange<T>(string key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(redis =>
            {
                RedisValue[] values = redis.ListRange(key, start, stop, flags);
                return Utilities.JsonUtility.ConvertToList<T>(values);
            });
        }

        /// <summary>
        /// 右进
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        public void ListRightPush<T>(string key, T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            Do(db => db.ListRightPush(key, Utilities.JsonUtility.ConvertToJson(value), when, flags));
        }

        /// <summary>
        /// 右出
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public T ListRightPop<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db =>
            {
                RedisValue value = db.ListRightPop(key, flags);
                return Utilities.JsonUtility.ConvertToObject<T>(value);
            });
        }

        /// <summary>
        /// 左进
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        public void ListLeftPush<T>(string key, T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            Do(db => db.ListLeftPush(key, Utilities.JsonUtility.ConvertToJson(value), when, flags));
        }

        /// <summary>
        /// 左出
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public T ListLeftPop<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db =>
            {
                RedisValue value = db.ListLeftPop(key, flags);
                return Utilities.JsonUtility.ConvertToObject<T>(value);
            });
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">Redis的键</param>
        public long ListLength(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(redis => redis.ListLength(key, flags));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <param name="count">count>0从头部删除，count<0从尾部删除</param>
        public async Task<long> ListRemoveAsync<T>(string key, T value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.ListRemoveAsync(key, Utilities.JsonUtility.ConvertToJson(value), count, flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="start">开始的位置</param>
        /// <param name="stop">结束的位置，包含</param>
        public async Task<List<T>> ListRangeAsync<T>(string key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            RedisValue[] values = await Do(redis => redis.ListRangeAsync(key, start, stop, flags)).ConfigureAwait(false);
            return Utilities.JsonUtility.ConvertToList<T>(values);
        }

        /// <summary>
        /// 右进
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        public async Task<long> ListRightPushAsync<T>(string key, T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.ListRightPushAsync(key, Utilities.JsonUtility.ConvertToJson(value), when, flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 右出
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public async Task<T> ListRightPopAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            RedisValue value = await Do(db => db.ListRightPopAsync(key, flags)).ConfigureAwait(false);
            return Utilities.JsonUtility.ConvertToObject<T>(value);
        }

        /// <summary>
        /// 左进
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        public async Task<long> ListLeftPushAsync<T>(string key, T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.ListLeftPushAsync(key, Utilities.JsonUtility.ConvertToJson(value), when, flags)).ConfigureAwait(false);
        }

        /// <summary>
        /// 左出
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        public async Task<T> ListLeftPopAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            RedisValue value = await Do(db => db.ListLeftPopAsync(key, flags)).ConfigureAwait(false);
            return Utilities.JsonUtility.ConvertToObject<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">Redis的键</param>
        public async Task<long> ListLengthAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(redis => redis.ListLengthAsync(key, flags)).ConfigureAwait(false);
        }

        #endregion 异步方法
    }
}
