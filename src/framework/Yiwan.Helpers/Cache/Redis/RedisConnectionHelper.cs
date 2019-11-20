using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Helpers.Cache
{
    /// <summary>
    /// 类 名 称：RedisConnectionHelper    
    /// 类 说 明：ConnectionMultiplexer对象管理帮助类
    /// 完成日期：2019-04-24
    /// 编码作者：赵立立 liley@foxmail.com QQ:897250000
    /// </summary>
    public static class RedisConnectionHelper
    {
        private static readonly object lockOjbect = new object(); // 执行锁        
        private static string RedisConnectionString; // 格式例 127.0.0.1:6379,password=123,allowadmin=true
        private static ConnectionMultiplexer _instance;
        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();

        public static string PrefixKey { get; set; }; // 自定义Key的前缀，每个Key都会自动增加此前缀

        public static void Initialize(string redisHosts, string prefixKey = null)
        {
            RedisConnectionString = redisHosts;
            PrefixKey = prefixKey ?? "";
        }

        /// <summary>
        /// 单例获取
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RedisConnectionString)) return null;
                if (_instance == null && !string.IsNullOrWhiteSpace(RedisConnectionString))
                {
                    lock (lockOjbect)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = GetManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 缓存获取
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = GetManager(connectionString);
            }
            return ConnectionCache[connectionString];
        }

        /// <summary>
        /// 获取redis服务器的一组相互关联的连接
        /// </summary>
        /// <param name="connectionString">Redis连接字符串</param>
        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            connectionString = connectionString ?? RedisConnectionString;

            var config = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                AllowAdmin = true,
                ConnectTimeout = 2000,
                SyncTimeout = 2000,
                Password = connectionString.Split(',')[1].Split('=')[1],//Redis数据库密码
                EndPoints = { connectionString.Split(',')[0] }// connectionString 为IP:Port 如”192.168.2.110:6379”
            };

            var connect = ConnectionMultiplexer.Connect(config);

            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;


            return connect;
        }

        #region 注册的事件

        /// <summary>
        /// Redis配置更改时
        /// </summary>
        /// <param name="e">与redis端点相关的事件信息:Event information related to redis endpoints</param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine("Redis配置更改了: " + e.EndPoint);
        }

        /// <summary>
        /// 发生Redis错误时
        /// </summary>
        /// <param name="e">来自redis服务器的错误通知:Notification of errors from the redis server</param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine("Redis错误: " + e.Message);
        }

        /// <summary>
        /// 连接失败后重新建立连接前
        /// </summary>
        /// <param name="e">包含关于服务器连接失败的信息:Contains information about a server connection failure</param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("尝试恢复Redis连接: " + e.EndPoint);
        }

        /// <summary>
        /// 重新连接失败时
        /// </summary>
        /// <param name="e">包含关于服务器连接失败的信息:Contains information about a server connection failure</param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("Redis连接失败:失败的端点: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 在集群更改,在重定位哈希槽时
        /// Contains information about individual hash-slot relocations
        /// </summary>
        /// <param name="e">包含有关单个Hash槽重定位的信息:Contains information about individual hash-slot relocations</param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine("Hash槽移动了(HashSlotMoved):新EndPoint" + e.NewEndPoint + ",旧EndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// 每当发生内部错误时(主要用于调试)
        /// Raised whenever an internal error occurs (this is primarily for debugging)
        /// </summary>
        /// <param name="e">描述内部错误(主要用于调试):Describes internal errors (mainly intended for debugging)</param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine("内部错误(InternalError):Message:" + e.Exception.Message);
        }

        #endregion 注册的事件
    }
}
