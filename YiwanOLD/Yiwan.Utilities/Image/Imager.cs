using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Utilities
{
    public class Imager
    {
        /// <summary>    
        /// 调用此函数后使此两种图片合并，类似相册，有个    
        /// 背景图，中间贴自己的目标图片    
        /// </summary>    
        /// <param name="imgBack">粘贴的源图片</param>    
        /// <param name="destImg">粘贴的目标图片绝对路径</param>    
        public static Image CombinImage(Image imgBack, string destImg, int destWidth = 0, int destHeight = 0)
        {
            Image imgFore = Image.FromFile(destImg);        //照片图片
            if (destWidth != 0 && destHeight != 0) imgFore = KiResizeImage(imgFore, destWidth, destHeight, 0);

            return CombinImage(imgBack, imgFore);
        }
        public static Image CombinImage(Image imgBack, Image imgFore)
        {
            int x = imgBack.Width / 2 - imgFore.Width / 2 > 0 ? imgBack.Width / 2 - imgFore.Width / 2 : 0;
            int y = imgBack.Width / 2 - imgFore.Width / 2 > 0 ? imgBack.Width / 2 - imgFore.Width / 2 : 0;

            return CombinImage(imgBack, imgFore, x, y);
        }
        /// <summary>    
        /// 调用此函数后使此两种图片合并，类似相册，有个    
        /// 背景图，中间贴自己的目标图片    
        /// </summary>
        /// <param name="imgBack">粘贴的源图片</param>
        /// <param name="destImg">粘贴的目标图片</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public static System.Drawing.Image CombinImage(System.Drawing.Image imgBack, System.Drawing.Image imgFore, int x, int y)
        {
            //把背景图添加进画板
            Graphics g = Graphics.FromImage(imgBack);
            //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    
            g.DrawImage(imgFore, x, y, imgFore.Width, imgFore.Height);
            g.Dispose();
            GC.Collect();

            return imgBack;
        }

        public static System.Drawing.Image CombinImageReverse(System.Drawing.Image imgBack, System.Drawing.Image imgFore, int x, int y)
        {
            var foreImg = (System.Drawing.Image)imgFore.Clone();
            //把背景图添加进画板
            Graphics g = Graphics.FromImage(imgFore);
            //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     
            g.DrawImage(imgFore, 0, 0, imgFore.Width, imgFore.Height);

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    
            g.DrawImage(imgBack, x, y, imgBack.Width, imgBack.Height);
            g.DrawImage(foreImg, 0, 0, foreImg.Width, foreImg.Height);
            g.Dispose();
            GC.Collect();

            return imgFore;
        }
        /// <summary>    
        /// Resize图片    
        /// </summary>    
        /// <param name="bmp">原始Bitmap</param>    
        /// <param name="newW">新的宽度</param>    
        /// <param name="newH">新的高度,为空则按比例</param>    
        /// <param name="Mode">保留着，暂时未用</param>    
        /// <returns>处理以后的图片</returns>    
        public static System.Drawing.Image KiResizeImage(System.Drawing.Image bmp, int width, int height = 0, int Mode = 0)
        {
            try
            {
                if (height == 0) height = width * bmp.Height / bmp.Width;
                Image b = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, width, height), new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
        public static Bitmap KiResizeImage(Bitmap bmp, int width, int height = 0, int Mode = 0)
        {
            try
            {
                if (height == 0) height = width * bmp.Height / bmp.Width;
                Bitmap b = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量    
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, width, height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        public static System.Drawing.Image CombinText(System.Drawing.Image imgBack, string text, string font, int fontSize, Color color, int x, int y)
        {
            //把背景图添加进画板
            Graphics g = Graphics.FromImage(imgBack);
            //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);

            using (Font f = new Font(font, fontSize))
            {
                using (Brush b = new SolidBrush(color))
                {
                    g.DrawString(text, f, b, x, y);
                }
            }

            g.Dispose();
            GC.Collect();

            return imgBack;
        }
    }
}
