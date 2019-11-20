using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
        public int DbNum { get; }
        private readonly ConnectionMultiplexer _conn;
        private string PrefixKey { get; set; }

        #region 构造函数

        public RedisHelper(int dbNum, string readWriteHosts = null)
        {
            DbNum = dbNum;
            _conn =
                string.IsNullOrWhiteSpace(readWriteHosts) ?
                RedisConnectionHelper.Instance :
                RedisConnectionHelper.GetConnectionMultiplexer(readWriteHosts);
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
            return sub.Publish(channel, Convert2Json(msg));
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
        /// <param name="customKey"></param>
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
            var prefixKey = PrefixKey ?? RedisConnectionHelper.PrefixKey;
            return prefixKey + oldKey;
        }

        private T Do<T>(Func<IDatabase, T> func)
        {
            try
            {
                var database = _conn.GetDatabase(DbNum);
                return func(database);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        /// <summary>
        /// 序列化对象为Json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private string Convert2Json<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        /// <summary>
        /// 反序列化Json字符串为Object对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private T Convert2Object<T>(RedisValue value)
        {
            if (value.IsNull) return default;
            if (typeof(T).Name.Equals(typeof(string).Name))
            {
                return JsonConvert.DeserializeObject<T>($"'{value}'");
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        private List<T> Convert2List<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = Convert2Object<T>(item);
                result.Add(model);
            }
            return result;
        }

        #endregion 辅助方法
    }
}
