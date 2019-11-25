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
        /// 添加Set
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <param name="score">排序分数</param>
        public bool SetAdd<T>(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.SetAdd(key, Convert2Json(value), flags));
        }

        /// <summary>
        /// 删除Set
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        public bool SetRemove<T>(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.SetRemove(key, Convert2Json(value), flags));
        }

        /// <summary>
        /// 获取全部Set的值
        /// </summary>
        /// <param name="key">Redis的键</param>
        public List<T> SetMembers<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db =>
            {
                var values = db.SetMembers(key, flags);
                return Convert2List<T>(values);
            });
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">Redis的键</param>
        public long SetLength(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.SetLength(key, flags));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 添加Set
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        /// <param name="score">排序分数</param>
        public async Task<bool> SetAddAsync<T>(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.SetAddAsync(key, Convert2Json(value), flags));
        }

        /// <summary>
        /// 删除Set
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="value">保存的值</param>
        public async Task<bool> SetRemoveAsync<T>(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.SetRemoveAsync(key, Convert2Json(value), flags));
        }

        /// <summary>
        /// 获取全部Set的值
        /// </summary>
        /// <param name="key">Redis的键</param>
        public async Task<List<T>> SetMembersAsync<T>(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            var values = await Do(db => db.SetMembersAsync(key, flags));
            return Convert2List<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">Redis的键</param>
        public async Task<long> SetLengthAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.SetLengthAsync(key, flags));
        }

        #endregion 异步方法
    }
}
