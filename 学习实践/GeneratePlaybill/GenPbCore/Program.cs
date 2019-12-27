using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenPbCore
{
    class Program
    {
        async static Task Main(string[] args)
        {
            #region 二维码
            if (true)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start(); // 开始监视代码

                QrUtils.Qr01();

                stopwatch.Stop();
                Console.WriteLine($"执行总时间：{ stopwatch.ElapsedMilliseconds}毫秒");
            }

            if (true)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start(); // 开始监视代码

                QrUtils.Qr02();

                stopwatch.Stop();
                Console.WriteLine($"执行总时间：{ stopwatch.ElapsedMilliseconds}毫秒");
            }
            Console.WriteLine(AppContext.BaseDirectory + "appsettings.json");
            #endregion 二维码




            var s = CheckFileProvider(@"ab");

            Console.WriteLine(s);


            while (true) Console.ReadLine();
        }

        static string CheckFileProvider(string file)
        {
            string path = AppContext.BaseDirectory;

            Regex regexFile = new Regex(@"^(?<fpath>([a-zA-Z]:\\)([\s\.\-\w]+\\)*)(?<fname>[\w]+.[\w]+)");
            Match matchResult = regexFile.Match(file);
            if (matchResult.Success)
            {
                path = matchResult.Result("${fpath}");
                file = matchResult.Result("${fname}");
            }

            if (file.IndexOf('.') != -1 && !file.ToLower().EndsWith(".json")) throw new Exception($"仅支持.json文件[{file}]");

            if (!file.ToLower().EndsWith(".json")) file += ".json";
            var fileProvider = new PhysicalFileProvider(path);
            var dir = fileProvider.Root;
            return path + file;
        }
    }
}
