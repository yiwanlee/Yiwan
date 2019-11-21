using System.Drawing;
using System.IO;

namespace Yiwan.Helpers.Http.Helper
{
    internal class ImageHelper
    {
        /// <summary>
        /// 将字节数组转为图片
        /// </summary>
        /// <param name=" b">字节数组</param>
        /// <returns>返回图片</returns>
        internal static System.Drawing.Image ByteToImage(byte[] b)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(b))
                {
                    return Image.FromStream(ms, true);
                }
            }
            catch { return null; }
        }
    }
}
