using QRCoder;
using System;
using System.Drawing;
using System.Text;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;

namespace GenPbCore
{
    public class QrUtils
    {
        static string QRS = "The text which should be encodedThe text which should be encodedThe text which should be encoded.";

        public static void Qr01()
        {
            return;
            for (int i = 0; i < 100; i++)
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRS + i, QRCodeGenerator.ECCLevel.M);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap img = qrCode.GetGraphic(20);
                qrCode.Dispose();
            }
        }

        public static void Qr02()
        {
            return;
            for (int i = 0; i < 100; i++)
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;
                qrCodeEncoder.QRCodeVersion = 8;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                Bitmap img = qrCodeEncoder.Encode(QRS + i, Encoding.Default);
                qrCodeEncoder = null;
            }
        }
    }
}
