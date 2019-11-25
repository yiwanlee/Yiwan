using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Yiwan.Utilities
{
    public class ColorHelper
    {

        /// <summary>
        ///  颜色距离（颜色相近度）
        /// </summary>
        /// <returns>返回：float类型的距离差</returns>
        public static float ColorDistance1(Color color1, Color color2)
        {
            List<float> hsb1 = ColorConversionHelper.RGB2HSB(color1);
            float h1 = hsb1[0] / 360;
            float s1 = hsb1[1];
            float b1 = hsb1[2];
            List<float> hsb2 = ColorConversionHelper.RGB2HSB(color2);
            float h2 = hsb2[0] / 360;
            float s2 = hsb2[1];
            float b2 = hsb2[2];

            float cd = Math.Abs(h1 - h2);//色相差的绝对值
            cd = cd + Math.Abs(s1 - s2);//加上饱和度差的绝对值
            cd = cd + Math.Abs(b1 - b2);//加上亮度的绝对值
            return cd;
        }
        /// <summary>
        ///  颜色距离（颜色相近度）
        /// </summary>
        /// <returns>返回：float类型的距离差</returns>
        public static double ColorDistance2(Color color1, Color color2)
        {
            //(255 - abs(r1 - r2) * 0.297 - abs(g1 - g2) * 0.593 - abs(b1 - b2) * 0.11) / 255
            int r1 = Convert.ToInt32(color1.R);
            int g1 = Convert.ToInt32(color1.G);
            int b1 = Convert.ToInt32(color1.B);
            int r2 = Convert.ToInt32(color2.R);
            int g2 = Convert.ToInt32(color2.G);
            int b2 = Convert.ToInt32(color2.B);
            return (Math.Abs(r1 - r2) * 0.297 + Math.Abs(g1 - g2) * 0.593 + Math.Abs(b1 - b2) * 0.11) / 255;
        }
        /// <summary>
        ///  颜色距离（颜色相近度）
        /// </summary>
        /// <returns>返回：float类型的距离差</returns>
        public static double ColorDistance3(Color color1, Color color2)
        {
            //(255 - abs(r1 - r2) * 0.297 - abs(g1 - g2) * 0.593 - abs(b1 - b2) * 0.11) / 255
            //sqrt(r3 * r3 + g3 * g3 + b3 * b3)/sqrt(255*255+255*255+255*255)
            int r1 = Convert.ToInt32(color1.R);
            int g1 = Convert.ToInt32(color1.G);
            int b1 = Convert.ToInt32(color1.B);
            int r2 = Convert.ToInt32(color2.R);
            int g2 = Convert.ToInt32(color2.G);
            int b2 = Convert.ToInt32(color2.B);

            int r3 = r1 - r2;
            int g3 = g1 - g2;
            int b3 = b1 - b2;

            return Math.Sqrt(r3 * r3 + g3 * g3 + b3 * b3)
                    / Math.Sqrt(255 * 255 * 3);
        }
    }
}
