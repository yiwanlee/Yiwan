using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Yiwan.Utilities
{
    public class ColorConversionHelper
    {
        /// <summary>
        ///  RGB转HSB
        /// </summary>
        /// <returns>返回：HSB值集合</returns>
        public static List<float> RGB2HSB(int red, int green, int blue)
        {
            List<float> hsbList = new List<float>();
            System.Drawing.Color dColor = System.Drawing.Color.FromArgb(red, green, blue);
            hsbList.Add(dColor.GetHue());
            hsbList.Add(dColor.GetSaturation());
            hsbList.Add(dColor.GetBrightness());
            return hsbList;
        }

        /// <summary>
        ///  RGB转HSB
        /// </summary>
        /// <returns>返回：HSB值集合</returns>
        public static List<float> RGB2HSB(Color color)
        {
            return RGB2HSB(Convert.ToInt32(color.R), Convert.ToInt32(color.G), Convert.ToInt32(color.B));
        }

        /// <summary>
        ///  RGB转HSB
        /// </summary>
        /// <returns>返回：HSB值集合</returns>
        public static List<float> RGB2HSB(string hexColor)
        {
            try
            {
                //hexColor = hexColor.Replace("#", "");
                //Color color = Color.FromArgb(
                //    Int32.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                //    Int32.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                //    Int32.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                return RGB2HSB(System.Drawing.ColorTranslator.FromHtml(hexColor));
            }
            catch (Exception ex)
            {
                throw new Exception("HexColor转换成Color对象失败", ex);
            }
        }

        /// <summary>
        /// HSB转RGB
        /// </summary>
        /// <param name="hue">色调</param>
        /// <param name="saturation">饱和度</param>
        /// <param name="brightness">亮度</param>
        /// <returns>返回：Color</returns>
        public static Color HSB2RGB(float hue, float saturation, float brightness)
        {
            int r = 0, g = 0, b = 0;
            if (saturation == 0)
            {
                r = g = b = (int)(brightness * 255.0f + 0.5f);
            }
            else
            {
                float h = (hue - (float)Math.Floor(hue)) * 6.0f;
                float f = h - (float)Math.Floor(h);
                float p = brightness * (1.0f - saturation);
                float q = brightness * (1.0f - saturation * f);
                float t = brightness * (1.0f - (saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(t * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 1:
                        r = (int)(q * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 2:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(t * 255.0f + 0.5f);
                        break;
                    case 3:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(q * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 4:
                        r = (int)(t * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 5:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(q * 255.0f + 0.5f);
                        break;
                }
            }
            return Color.FromArgb(Convert.ToByte(255), Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }

        public static string Color2Hex(Color color)
        {
            return System.Drawing.ColorTranslator.ToHtml(color);
        }
        
    }
}
