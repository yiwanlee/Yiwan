using QRCoder;
using System;
using System.Drawing;

namespace GenPbCore
{
    class Program
    {
        static void Main(string[] args)
        {

            if (true)
            {
                QRCodeGenerator qrGenerator1 = new QRCodeGenerator();
                QRCodeData qrCodeData1 = qrGenerator1.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode1 = new QRCode(qrCodeData1);
                Bitmap qrCodeImage1 = qrCode1.GetGraphic(20);

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start(); // 开始监视代码

                QrUtils.Qr01();

                stopwatch.Stop();
                Console.WriteLine($"执行总时间：{ stopwatch.ElapsedMilliseconds}毫秒");
            }

            if (true)
            {
                QRCodeGenerator qrGenerator1 = new QRCodeGenerator();
                QRCodeData qrCodeData1 = qrGenerator1.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode1 = new QRCode(qrCodeData1);
                Bitmap qrCodeImage1 = qrCode1.GetGraphic(20);

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start(); // 开始监视代码

                QrUtils.Qr02();

                stopwatch.Stop();
                Console.WriteLine($"执行总时间：{ stopwatch.ElapsedMilliseconds}毫秒");
            }
        }
    }
}
