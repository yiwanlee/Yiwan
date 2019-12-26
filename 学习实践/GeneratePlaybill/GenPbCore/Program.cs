using Microsoft.Extensions.Configuration;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

            string s = await AppSettingHelper.Get("DB");

            Console.WriteLine("文件变更了嘛：" + s);


            while (true) Console.ReadKey();
        }
    }
}
