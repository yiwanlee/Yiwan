using QRCoder;
using System;
using System.Drawing;


namespace GenPbFrame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(1000);

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start(); //  开始监视代码

            QrUtils.Qr02();

            stopwatch.Stop();
            Console.WriteLine($"执行总时间：{ stopwatch.ElapsedMilliseconds}毫秒");

            while (true) Console.ReadKey();
        }
    }
}
