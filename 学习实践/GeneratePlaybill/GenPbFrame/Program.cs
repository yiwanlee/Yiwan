using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors;
using System;
using System.Drawing;
using System.IO;

namespace GenPbFrame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (true)
            {
                
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start(); //  开始监视代码

                for (int i = 0; i < 20; i++)
                {
                    byte[] pbBytes = File.ReadAllBytes(AppContext.BaseDirectory + "/1911292.jpg");

                    var qrimg = System.Drawing.Image.FromFile(AppContext.BaseDirectory + "/qrcode.jpg");
                    ImageLayer qr = new ImageLayer
                    {
                        Image = qrimg,
                        Size = new Size(159, 159),
                        Position = new Point(542, 1087)
                    };
                    using (MemoryStream inStream = new MemoryStream(pbBytes))
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                            {
                                // Load, resize, set the format and quality and save an image.
                                imageFactory.Load(inStream)
                                            .Overlay(qr)
                                           .Save(outStream);
                                //.Save($"d://AA{DateTime.Now.ToString("yyyyMMddHHmmss") + (new Random().Next(1000, 9999))}.jpg");

                            }
                        }
                    }
                }

                stopwatch.Stop();
                Console.WriteLine($"执行总时间：{ stopwatch.ElapsedMilliseconds}毫秒");
            }







            while (true) Console.ReadKey();
        }
    }
}
