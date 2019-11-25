/// <summary>
/// 类说明：七牛云操作类
/// 联系方式：liley@foxmail.com 秋秋:897250000
/// 额外依赖：Qiniu(Nuget)
/// </summary>
using System;

namespace Yiwan.Utilities.CloudStorage
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Qiniu.Http;
    using Qiniu.Storage;
    using Qiniu.Util;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// 七牛云存储帮助类
    /// 默认配置文件从AppSettings中读取(Qiniu_AK、Qiniu_SK、Qiniu_Bucket、Qiniu_Domain)
    /// </summary>
    public class QiniuHelper
    {
        /// <summary>
        /// 来自AppSettings["Qiniu_AK"]
        /// </summary>
        private static string AK = ConfigurationManager.AppSettings["Qiniu_AK"];
        /// <summary>
        /// 来自AppSettings["Qiniu_SK"]
        /// </summary>
        private static string SK = ConfigurationManager.AppSettings["Qiniu_SK"];
        /// <summary>
        /// 来自AppSettings["Qiniu_Bucket"]
        /// </summary>
        private static string Bucket = ConfigurationManager.AppSettings["Qiniu_Bucket"];
        /// <summary>
        /// 来自AppSettings["Qiniu_Domain"]
        /// </summary>
        public static string Domain = ConfigurationManager.AppSettings["Qiniu_Domain"];

        /// <summary>
        /// 来自AppSettings["Qiniu_Zone"]
        /// </summary>
        private static Zone _Zone = ConfigurationManager.AppSettings["Qiniu_Zone"] == "ZONE_CN_East" ? Zone.ZONE_CN_East :
            (ConfigurationManager.AppSettings["Qiniu_Zone"] == "ZONE_CN_North" ? Zone.ZONE_CN_North :
            (ConfigurationManager.AppSettings["Qiniu_Zone"] == "ZONE_CN_South" ? Zone.ZONE_CN_South : Zone.ZONE_US_North)
            ); // 省事用三元

        /// <summary>
        /// 隐藏构造函数
        /// </summary>
        static QiniuHelper() { }

        /// <summary>
        /// 全局初始化一次即可，建议在Application_Start中执行
        /// </summary>
        /// <param name="ak">七牛AK</param>
        /// <param name="sk">七牛SK</param>
        /// <param name="bucket">七牛Bucket</param>
        /// <param name="domain">七牛绑定的域名</param>
        public static void Init(string ak, string sk, string bucket, string domain, Zone _zone)
        {
            if (string.IsNullOrWhiteSpace(ak) || string.IsNullOrWhiteSpace(ak) || string.IsNullOrWhiteSpace(ak) || string.IsNullOrWhiteSpace(ak) || _zone == null)
            {
                throw new Exception("无法初始化，请检查传入参数是否为null或空字符串");
            }

            AK = ak;
            SK = sk;
            Bucket = bucket;
            Domain = domain;
            _Zone = _zone;
        }

        static void ValidInit()
        {
            if (string.IsNullOrWhiteSpace(AK) || string.IsNullOrWhiteSpace(SK) || string.IsNullOrWhiteSpace(Bucket) || string.IsNullOrWhiteSpace(Domain) || _Zone == null)
            {
                throw new Exception("QiniuHelper没有初始化，请执行QiniuHelper.Init()进行初始化，建议在Application_Start中执行");
            }
        }

        /// <summary>
        /// 获取上传凭证Upload_Token，默认的AK/SK/Bucket从配置AppSettings中Qiniu_AK/Qiniu_SK/Qiniu_Bucket
        /// </summary>
        public static string GetUploadToken()
        {
            ValidInit(); // 验证初始化

            Mac mac = new Mac(AK, SK);
            // 上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = Bucket;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(7200);
            // 生成上传凭证
            return Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        }

        public static string GetUploadToken(string AK, string SK, string Bucket)
        {
            Mac mac = new Mac(AK, SK);
            // 上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = Bucket;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(7200);
            // 生成上传凭证
            return Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        }

        /// <summary>
        /// 上传网络路径的文件到七牛云,默认的AK/SK/Bucket从配置AppSettings中Qiniu_AK/Qiniu_SK/Qiniu_Bucket
        /// </summary>
        /// <param name="fileurl">文件url</param>
        /// <param name="prefix">七牛路径/前缀</param>
        /// <param name="ext">扩展名(不含.)</param>
        public static string Upload(string fileurl, string prefix, string ext)
        {
            return Upload(AK, SK, Bucket, _Zone, fileurl, prefix, ext);
        }

        public static Task<string> UploadAsync(string fileurl, string prefix, string ext)
        {
            return Task.Run(() =>
            {
                return Upload(AK, SK, Bucket, _Zone, fileurl, prefix, ext);
            });
        }

        /// <summary>
        /// 上传网络路径的文件到七牛云
        /// </summary>
        /// <param name="fileurl">文件url</param>
        /// <param name="prefix">七牛路径/前缀</param>
        /// <param name="ext">扩展名(不含.)</param>
        public static string Upload(string ak, string sk, string bucket, Zone zone, string fileurl, string prefix, string ext)
        {
            try
            {
                string saveKey = prefix + DateTime.Now.ToString("yyyyMMddHHmmssffff") + (new Random()).Next().ToString().Substring(0, 4) + "." + ext;
                Mac mac = new Mac(ak, sk);
                //设置
                Config config = new Config
                {
                    Zone = zone,
                    UseHttps = true,
                    UseCdnDomains = true
                };

                BucketManager bucketManager = new BucketManager(mac, config);

                FetchResult ret = bucketManager.Fetch(fileurl, bucket, saveKey);
                if (ret.Code != (int)HttpCode.OK)
                    return "fetch error: " + ret.ToString();
                else return Domain + "/" + ((JObject)JsonConvert.DeserializeObject(ret.Text))["key"].ToString();
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}:{3}", "QiniuHelper", "上传网络文件", fileurl, JsonConvert.SerializeObject(ex));
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// 上传流文件到七牛云,默认的AK/SK/Bucket从配置AppSettings中Qiniu_AK/Qiniu_SK/Qiniu_Bucket
        /// </summary>
        /// <param name="bytes">字节组</param>
        /// <param name="prefix">七牛路径/前缀</param>
        /// <param name="ext">扩展名(不含.)</param>
        public static string Upload(byte[] bytes, string prefix, string ext, string filename = "")
        {
            return Upload(AK, SK, Bucket, _Zone, bytes, prefix, ext, filename);
        }

        /// <summary>
        /// 上传流文件到七牛云,默认的AK/SK/Bucket从配置AppSettings中Qiniu_AK/Qiniu_SK/Qiniu_Bucket
        /// </summary>
        /// <param name="bytes">字节组</param>
        /// <param name="prefix">七牛路径/前缀</param>
        /// <param name="ext">扩展名(不含.)</param>
        public static string Upload(string ak, string sk, string bucket, Zone zone, byte[] bytes, string prefix, string ext, string filename = "")
        {
            ValidInit(); // 验证初始化

            try
            {
                string saveKey = prefix + DateTime.Now.ToString("yyyyMMddHHmmssffff") + (new Random()).Next().ToString().Substring(0, 4) + "." + ext;
                if (filename != "") saveKey = prefix + filename + "." + ext;
                //设置
                Config config = new Config
                {
                    Zone = zone,
                    UseHttps = true,
                    UseCdnDomains = true
                };

                UploadManager upm = new UploadManager(config);
                HttpResult ret = upm.UploadData(bytes, saveKey, GetUploadToken(ak, sk, bucket), null);

                if (ret.Code != (int)HttpCode.OK)
                    return "formuplaod error: " + ret.ToString();
                else return Domain + "/" + ((JObject)JsonConvert.DeserializeObject(ret.Text))["key"].ToString();
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!:{2}", "QiniuHelper", "上传流文件", JsonConvert.SerializeObject(ex));
                throw new Exception(msg);
            }
        }
    }
}
