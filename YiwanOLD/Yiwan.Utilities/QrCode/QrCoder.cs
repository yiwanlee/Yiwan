using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Utilities
{
    public static class QrCoder
    {
        /// <summary>
        /// 生成二维码，返回Bitmap
        /// </summary>
        public static Bitmap Encode(string content)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            encoder.QRCodeScale = 4;
            encoder.QRCodeVersion = 0;

            return encoder.Encode(content, Encoding.UTF8);
        }

        public static string EncodeToQiniu(string content, string prefix, int width = 0)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    Bitmap qrc = width == 0 ? Encode(content) : Imager.KiResizeImage(Encode(content), width);
                    qrc.Save(ms, ImageFormat.Jpeg);
                    var rsByte = ms.ToArray();
                    qrc.Dispose();
                    string url = CloudStorage.QiniuHelper.Upload(rsByte, prefix, "jpg");
                    if (url.IndexOf("error:") != -1) return "";
                    else return url;
                }
                catch (Exception ex)
                {
                    NLogger.Current.Error(ex);
                    return "";
                }
            }

        }
    }
}
