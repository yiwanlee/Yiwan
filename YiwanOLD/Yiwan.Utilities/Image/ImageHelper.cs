/// <summary>
/// 类说明：Image图片帮助类
/// 联系方式：liley@foxmail.com 秋秋:897250000
/// 需要引用类库 System.Web 4.0
/// </summary>
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Utilities
{
    public class ImageHelper
    {
        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        // <summary> 
        /// 字节流转换成图片 
        /// </summary> 
        /// <param name="byt">要转换的字节流</param> 
        /// <returns>转换得到的Image对象</returns> 
        public static Image BytToImg(byte[] byt)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byt);
                Image img = Image.FromStream(ms);
                return img;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///  图片转换成字节流 
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] b = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return b;
        }

        /// <summary>
        /// 把图片Url转化成Image对象
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static Image Url2Img(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return null;
                }

                WebRequest webreq = WebRequest.Create(imageUrl);
                WebResponse webres = webreq.GetResponse();
                Stream stream = webres.GetResponseStream();
                Image image;
                image = Image.FromStream(stream);
                stream.Close();

                return image;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 把本地图片路径转成Image对象
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static Image ImagePath2Img(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    return null;
                }

                byte[] bytes = ImageToByteArray(Image.FromFile(imagePath));
                Image image = BytToImg(bytes);
                return image;
            }
            catch
            {
                return null;
            }
        }
    }
}
