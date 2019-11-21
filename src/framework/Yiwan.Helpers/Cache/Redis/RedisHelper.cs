using StackExchange.Redis;
using System;

namespace Yiwan.Helpers.Cache
{
    /// <summary>
    /// 类 名 称：RedisHelper    
    /// 类 说 明：Redis操作类
    /// 完成日期：2019-04-24
    /// 编码作者：赵立立 liley@foxmail.com QQ:897250000
    /// </summary>
    public partial class RedisHelper
    {
        /// <summary>
        /// 数据库序号，从0开始
        /// </summary>
        public int DbNum { get; }

        private readonly ConnectionMultiplexer _conn;

        private string PrefixKey { get; set; } //KEY的前缀

        #region 构造函数

        public RedisHelper(int dbNum, string readWriteHosts = null)
        {
            DbNum = dbNum;
            _conn =
                string.IsNullOrWhiteSpace(readWriteHosts) ?
                RedisConnectionHelper.Instance :
                RedisConnectionHelper.GetConnectionMultiplexer(readWriteHosts);

            if (_conn == null) throw new RedisConnectionException(ConnectionFailureType.None, "Connection为空，请检查链接字符串，或检查是否全局执行RedisConnectionHelper.Initialize");
        }
        #endregion 构造函数

        #region 发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            ISubscriber sub = _conn.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
            {
                if (handler == null)
                {
                    Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                }
                else
                {
                    handler(channel, message);
                }
            });
        }

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long Publish<T>(string channel, T msg)
        {
            ISubscriber sub = _conn.GetSubscriber();
            return sub.Publish(channel, Utilities.JsonUtility.ConvertToJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            ISubscriber sub = _conn.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            ISubscriber sub = _conn.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion 发布订阅

        #region 其他

        /// <summary>
        /// 创建事务
        /// </summary>
        public ITransaction CreateTransaction()
        {
            return GetDatabase().CreateTransaction();
        }

        /// <summary>
        /// 获取Redis数据库，根据dbNum
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _conn.GetDatabase(DbNum);
        }

        public IServer GetServer(string hostAndPort)
        {
            return _conn.GetServer(hostAndPort);
        }

        /// <summary>
        /// 设置前缀
        /// </summary>
        public void SetPrefixKey(string prefixKey)
        {
            PrefixKey = prefixKey;
        }

        #endregion 其他

        #region 辅助方法

        /// <summary>
        /// 为Key添加前缀
        /// </summary>
        /// <param name="oldKey">添加前缀之前的Key</param>
        private string AddPrefixKey(string oldKey)
        {
            string prefixKey = PrefixKey ?? RedisConnectionHelper.PrefixKey;
            return prefixKey + oldKey;
        }

        private T Do<T>(Func<IDatabase, T> func)
        {
            try
            {
                IDatabase database = _conn.GetDatabase(DbNum);
                return func(database);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        #endregion 辅助方法
    }
}
