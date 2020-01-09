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

                var s2 = JsonFileHelper.Get("DB:DBType", $"appsettings.json");
                Console.WriteLine("同步Get:" + s2);

                var s1 = await JsonFileHelper.GetAsync("DB:DBType", $"appsettings.json");
                Console.WriteLine("异步Get:" + s1);
                


                //List<Student> ls1 = await JsonFileHelper.GetListAsync<Student>("school:students", "appsettings2.json");
                //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ls1));
                //Console.WriteLine("0--------------");
                //Console.ReadLine();

                //var s11 = JsonFileHelper.Get("DB:DBType", $"appsettings.json");
                //Console.WriteLine(s11);

                //var s22 = JsonFileHelper.Get<int>("DB:DBNum", $"appsettings.json");
                //Console.WriteLine(s22);

                //List<Student> ls11 = JsonFileHelper.GetList<Student>("school:students", "appsettings.json");
                //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ls11));


                Console.WriteLine("!--------------------------------------");
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
