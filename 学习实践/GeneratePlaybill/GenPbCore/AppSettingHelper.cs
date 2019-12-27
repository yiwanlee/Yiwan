using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GenPbCore
{
    public class AppSettingHelper
    {
        const string _file = "appsettings.json";
        static IFileProvider _fileProvider = null;
        static JObject _settings = null;

        static AppSettingHelper()
        {
            Console.WriteLine("AppSettingHelper");
            _fileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
            ChangeToken.OnChange(() => _fileProvider.Watch(_file), async () => await LoadFileAsync());
        }
        static async Task LoadFileAsync()
        {
            Console.WriteLine($"文件变更 [{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]");
            using Stream stream = _fileProvider.GetFileInfo(_file).CreateReadStream();
            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);

            if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
            {
                var listBuffer = new List<byte>(buffer);
                listBuffer.RemoveRange(0, 3);
                buffer = listBuffer.ToArray();
            }

            var jsonStr = Encoding.UTF8.GetString(buffer);
            _settings = (JObject)JsonConvert.DeserializeObject(jsonStr);
        }

        public static async Task<string> Get(string key)
        {
            if (_settings == null) await LoadFileAsync();
            return _settings?[key].ToString();
        }

        /// <summary>
        /// 根据节点读取Json返回实体对象
        /// </summary>
        /// <returns></returns>
        public static string Read<T>(string section)
        {
            return (_fileProvider == null).ToString();



            try
            {
                byte[] buffer;
                var fprov = new PhysicalFileProvider(AppContext.BaseDirectory);
                using (var readStream = fprov.GetFileInfo("appsettings.json").CreateReadStream())
                {
                    buffer = new byte[readStream.Length];
                    readStream.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);

                //using (var file = File.OpenText(AppContext.BaseDirectory + "appsettings.json"))
                //{
                //    string s = file.ReadToEnd();
                //    var jo = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(s);
                //    using (var reader = new JsonTextReader(file))
                //    {
                //        var jObj = (JObject)JObject.ReadFrom(reader, new JsonLoadSettings
                //        {
                //            CommentHandling = CommentHandling.Ignore,
                //            DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace,
                //            LineInfoHandling = LineInfoHandling.Ignore
                //        });
                //        if (!string.IsNullOrWhiteSpace(section))
                //        {
                //            var secJt = jObj[section];
                //            if (secJt != null)
                //            {
                //                return JsonConvert.DeserializeObject<T>(secJt.ToString());
                //            }
                //        }
                //        else
                //        {
                //            return JsonConvert.DeserializeObject<T>(jObj.ToString());
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return default(T);
        }
    }
}
