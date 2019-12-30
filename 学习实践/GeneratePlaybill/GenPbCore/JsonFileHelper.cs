using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenPbCore
{
    /// <summary>
    /// Json文件读写
    /// 引用Newtonsoft.Json
    /// </summary>
    public static class JsonFileHelper
    {
        static readonly ConcurrentBag<PhysicalFileProvider> cacheFileProviders = new ConcurrentBag<PhysicalFileProvider>();
        static readonly ConcurrentBag<string> watchFiles = new ConcurrentBag<string>();
        static readonly ConcurrentDictionary<string, JToken> cacheSettings = new ConcurrentDictionary<string, JToken>();

        /// <summary>
        /// 检查指定文件及路径是否已经创建FileProvider，如无则创建
        /// </summary>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        static async Task<string> CheckAndGetFileProvider(string file)
        {
            string path = AppContext.BaseDirectory;
            Regex regexFile = new Regex(@"^(?<fpath>([a-zA-Z]:\\)([\s\.\-\w]+\\)*)(?<fname>[\w]+.[\w]+)");
            Match matchResult = regexFile.Match(file); // 匹配文件路径
            if (matchResult.Success)
            {
                path = matchResult.Result("${fpath}");
                file = matchResult.Result("${fname}");
            }

            if (file.IndexOf('.') != -1 && !file.ToLower().EndsWith(".json")) throw new Exception($"仅支持.json文件[{file}]");
            if (!file.ToLower().EndsWith(".json")) file += ".json";
            if (!File.Exists(path + file)) throw new Exception($"文件未找到[{path + file}]");

            // 查询 文件提供者 缓存列表是否已有，如果无，则新建并加入列表
            PhysicalFileProvider fileProvider = null;
            foreach (var fp in cacheFileProviders) if (fp.Root == path) fileProvider = fp;
            if (fileProvider == null)
            {
                fileProvider = new PhysicalFileProvider(path);
                cacheFileProviders.Add(fileProvider);
            }

            // 查询 监视文件列表是否已有，如果无，则新建监视回调并加入监视文件列表
            bool inWatchs = false;
            foreach (var f in watchFiles) if (f == path + file) inWatchs = true;

            if (!inWatchs)
            {
                await LoadFileAsync(fileProvider, path, file);
                ChangeToken.OnChange(() => fileProvider.Watch(file), async () => await LoadFileAsync(fileProvider, path, file));
                watchFiles.Add(path + file);
            }

            return path + file;
        }

        /// <summary>
        /// 加载文件到内存；本函数也是监听文件变动的处理函数
        /// </summary>
        /// <param name="fileProvider">文件提供者</param>
        /// <param name="path">文件路径</param>
        /// <param name="file">文件名</param>
        /// <param name="onReload">是否是二次重试</param>
        /// <returns></returns>
        static async Task LoadFileAsync(PhysicalFileProvider fileProvider, string path, string file, bool onReload = false)
        {
            using Stream stream = fileProvider.GetFileInfo(file).CreateReadStream();
            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);

            if (buffer.Length > 0)
            {
                // 处理utf8的bom头
                if (buffer.Length > 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                {
                    var listBuffer = new List<byte>(buffer);
                    listBuffer.RemoveRange(0, 3);
                    buffer = listBuffer.ToArray();
                }

                var jsonStr = Encoding.UTF8.GetString(buffer);
                var settings = (JToken)JsonConvert.DeserializeObject(jsonStr);

                cacheSettings[path + file] = settings;
            }
            else if (!onReload) await LoadFileAsync(fileProvider, path, file, true); // 加载文件失败重试一次
        }

        /// <summary>
        /// 解析指定json文件，并获取值
        /// <para>解析失败则返回default(T)</para>
        /// </summary>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static async Task<string> Get(string key, string file)
        {
            return await Get<string>(key, file);
        }

        /// <summary>
        /// 解析指定json文件，并获取值
        /// <para>解析失败则返回default(T)</para>
        /// </summary>
        /// <typeparam name="T">返回类型，如果涉及返回List，请试用GetList</typeparam>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static async Task<T> Get<T>(string key, string file)
        {
            try
            {
                string fileKey = await CheckAndGetFileProvider(file);
                if (cacheSettings[fileKey] != null)
                {
                    string[] childKeys = key.Split(':');
                    JToken jToken = cacheSettings[fileKey];
                    foreach (var k in childKeys)
                    {
                        if (k.Trim().Equals("")) continue;
                        jToken = jToken[k];
                    }

                    if (typeof(T).FullName.StartsWith("System.Collections.Generic.List"))
                        throw new Exception("泛型T是一个List<T>类型，请使用GetList<T>");



                    if (jToken != null)
                    {
                        switch (jToken.Type)
                        {
                            case JTokenType.Null:
                            case JTokenType.Undefined:
                                return default;
                            case JTokenType.Integer:
                            case JTokenType.Float:
                            case JTokenType.String:
                            case JTokenType.Boolean:
                            case JTokenType.Date:
                            case JTokenType.Guid:
                            case JTokenType.TimeSpan:
                                return JsonConvert.DeserializeObject<T>($"'{jToken.ToString()}'");
                            case JTokenType.Object:
                                return JsonConvert.DeserializeObject<T>(jToken.ToString());
                        }
                    }
                }
            }
            catch { }
            return default;
        }

        /// <summary>
        /// 解析指定json文件，并获取集合
        /// <para>解析失败则返回default(T)</para>
        /// </summary>
        /// <typeparam name="T">返回集合元素类型</typeparam>
        /// <param name="key">json键，层级键值使用冒号(例 system:website:seo)</param>
        /// <param name="file">文件路径 支持1完整路径2文件名3文件名不含扩展名；非完整路径时默认在程序根目录查找</param>
        public static async Task<List<T>> GetList<T>(string key, string file)
        {
            try
            {
                string fileKey = await CheckAndGetFileProvider(file);
                if (cacheSettings[fileKey] != null)
                {
                    string[] childKeys = key.Split(':');
                    var jToken = cacheSettings[fileKey];
                    foreach (var k in childKeys)
                    {
                        if (k.Trim().Equals("")) continue;
                        jToken = jToken[k];
                    }

                    if (jToken.Type == JTokenType.Array)
                    {
                        List<T> list = new List<T>();
                        foreach (var j in (JArray)jToken)
                        {
                            T jval = default;
                            switch (j.Type)
                            {
                                case JTokenType.Integer:
                                case JTokenType.Float:
                                case JTokenType.String:
                                case JTokenType.Boolean:
                                case JTokenType.Date:
                                case JTokenType.Guid:
                                case JTokenType.TimeSpan:
                                    jval = JsonConvert.DeserializeObject<T>($"'{j.ToString()}'");
                                    break;
                                case JTokenType.Object:
                                    jval = JsonConvert.DeserializeObject<T>(j.ToString());
                                    break;
                            }
                            list.Add(jval);
                        }
                        return list;
                    }
                }
            }
            catch { }
            return default;
        }
    }
}
