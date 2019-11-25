using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Utilities.Cache
{
    public partial class RedisHelper
    {
        #region 同步方法

        /// <summary>
        /// 添加SortedSet
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <param name="score">排序分数</param>
        public bool SortedSetAdd<T>(string key, T value, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(redis => redis.SortedSetAdd(key, Convert2Json<T>(value), score, when, flags));
        }

        /// <summary>
        /// 删除SortedSet
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        public bool SortedSetRemove<T>(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(redis => redis.SortedSetRemove(key, Convert2Json(value), flags));
        }

        /// <summary>
        /// 获取全部SortedSet的值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="start">开始的位置</param>
        /// <param name="stop">结束的位置，包含</param>
        /// <param name="order">排序方式</param>
        public List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(redis =>
            {
                var values = redis.SortedSetRangeByRank(key, start, stop, order, flags);
                return Convert2List<T>(values);
            });
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="min">最小的score分数</param>
        /// <param name="max">最大的score分数</param>
        /// <param name="exclude">排斥不包含，默认不排斥包含min，max，Start不包含min，Stop不包含max，Both都不包含</param>
        public long SortedSetLength(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(redis => redis.SortedSetLength(key, min, max, exclude, flags));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 添加SortedSet
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <param name="score">排序分数</param>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(redis => redis.SortedSetAddAsync(key, Convert2Json<T>(value), score, when, flags));
        }

        /// <summary>
        /// 删除SortedSet
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(redis => redis.SortedSetRemoveAsync(key, Convert2Json(value), flags));
        }

        /// <summary>
        /// 获取全部SortedSet的值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">Redis的键</param>
        /// <param name="start">开始的位置</param>
        /// <param name="stop">结束的位置，包含</param>
        /// <param name="order">排序方式</param>
        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            var values = await Do(redis => redis.SortedSetRangeByRankAsync(key, start, stop, order, flags));
            return Convert2List<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="key">Redis的键</param>
        /// <param name="min">最小的score分数</param>
        /// <param name="max">最大的score分数</param>
        /// <param name="exclude">排斥不包含，默认不排斥包含min，max，Start不包含min，Stop不包含max，Both都不包含</param>
        public async Task<long> SortedSetLengthAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(redis => redis.SortedSetLengthAsync(key, min, max, exclude, flags));
        }

        #endregion 异步方法
    }
}
