using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Utilities.Cache
{
    public partial class RedisHelper
    {
        /// <summary>
        /// 查询指定key是否存在
        /// </summary>
        /// <param name="key">Redis的键</param>
        public bool KeyExists(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.KeyExists(key, flags));
        }

        /// <summary>
        /// 查询指定key是否存在，异步
        /// </summary>
        /// <param name="key">Redis的键</param>
        public async Task<bool> KeyExistsAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.KeyExistsAsync(key, flags));
        }

        /// <summary>
        /// 重命名key
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="newKey">新的Redis的键</param>
        public bool KeyRename(string key, string newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.KeyRename(key, newKey, when, flags));
        }

        /// <summary>
        /// 重命名key，异步
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="newKey">新的Redis的键</param>
        public async Task<bool> KeyRenameAsync(string key, string newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.KeyRenameAsync(key, newKey, when, flags));
        }

        /// <summary>
        /// 为指定key设置expiry超时时间
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="expiry">超时时间</param>
        public bool KeyExpire(string key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.KeyExpire(key, expiry, flags));
        }

        /// <summary>
        /// 为指定key设置expiry超时时间，异步
        /// </summary>
        /// <param name="key">Redis的键</param>
        /// <param name="expiry">超时时间</param>
        public async Task<bool> KeyExpireAsync(string key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.KeyExpireAsync(key, expiry, flags));
        }

        /// <summary>
        /// 删除指定key
        /// </summary>
        /// <param name="key">Redis的键</param>
        public bool KeyDelete(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return Do(db => db.KeyDelete(key, flags));
        }

        /// <summary>
        /// 删除指定key，异步
        /// </summary>
        /// <param name="key">Redis的键</param>
        public async Task<bool> KeyDeleteAsync(string key, CommandFlags flags = CommandFlags.None)
        {
            key = AddPrefixKey(key);
            return await Do(db => db.KeyDeleteAsync(key, flags));
        }

        /// <summary>
        /// 清空指定数据库
        /// </summary>
        /// <param name="database">数据库位置</param>
        public void FlushDatabase(int database = 0, CommandFlags flags = CommandFlags.None)
        {
            EndPoint[] endPoints = _conn.GetEndPoints();
            if (endPoints.Length > 0) _conn.GetServer(endPoints[0]).FlushDatabase(database, flags);
        }

        /// <summary>
        /// 清空指定数据库，异步
        /// </summary>
        /// <param name="database">数据库位置</param>
        public async void FlushDatabaseAsync(int database = 0, CommandFlags flags = CommandFlags.None)
        {
            EndPoint[] endPoints = _conn.GetEndPoints();
            if (endPoints.Length > 0)
            {
                var serv = _conn.GetServer(endPoints[0]);
                await serv.FlushDatabaseAsync(database, flags);
            }
        }

        /// <summary>
        /// 清空所有数据库
        /// </summary>
        public void FlushAllDatabases(CommandFlags flags = CommandFlags.None)
        {
            EndPoint[] endPoints = _conn.GetEndPoints();
            if (endPoints.Length > 0) _conn.GetServer(endPoints[0]).FlushAllDatabases(flags);
        }

        /// <summary>
        /// 清空所有数据库，异步
        /// </summary>
        public async void FlushAllDatabasesAsync(CommandFlags flags = CommandFlags.None)
        {
            EndPoint[] endPoints = _conn.GetEndPoints();
            if (endPoints.Length > 0) await _conn.GetServer(endPoints[0]).FlushAllDatabasesAsync(flags);
        }
    }
}
