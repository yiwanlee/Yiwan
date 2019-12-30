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
            //Console.WriteLine(AppContext.BaseDirectory + "appsettings.json");
            #endregion 二维码


            while (true)
            {

                var s = await JsonFileHelper.Get("DB:DBType", $"appsettings.json");
                Console.WriteLine(s);

                var s2 = await JsonFileHelper.Get<int>("DB:DBNum", $"appsettings.json");
                Console.WriteLine(s2);

                List<Student> ls = await JsonFileHelper.GetList<Student>("school:students", "appsettings.json");
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ls));




                Console.ReadLine();
            }
        }
        class Student
        {
            public string name { get; set; }
            public int age { get; set; }
        }

    }


}
