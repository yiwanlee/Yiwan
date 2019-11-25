/// <summary>
/// 类说明：验证码 Captcha
/// 联系方式：liley@foxmail.com 秋秋:897250000
/// 额外依赖：System.Web 4.0
/// </summary>
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Utilities
{
    public class Captcha
    {
        /// <summary>
        /// 验证码类型
        /// </summary>
        public enum CaptchaType
        {
            /// <summary>
            /// 基础简单的验证码
            /// </summary>
            Simple = 1
        }
        public static string RandomCode(int codeLength = 4)
        {
            //排除了i,I,l,L,O,o,Q,0
            string[] arrChars = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,G,K,M,N,P,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,g,k,m,n,p,q,r,s,t,u,v,w,x,y,z".Split(',');
            return string.Join("", arrChars.OrderBy(q => Guid.NewGuid()).Take(codeLength).ToList());//随机验证码字符串
        }

        /// <summary>
        /// 创建验证码图
        /// </summary>
        /// <param name="randomCode">随机Code</param>
        /// <param name="captchaType">默认简单类型</param>
        public static Image Create(string randomCode, CaptchaType captchaType = CaptchaType.Simple)
        {
            Bitmap image = new Bitmap(randomCode.Length * 33, 40);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
                }
                Font font = new Font("Arial", 21, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                PointF pf = new Point(8, 3);//设置PointF和SizeF
                SizeF charSize;
                char[] charCode = randomCode.ToCharArray();
                foreach (char c in charCode)
                {
                    //获取字符尺寸
                    charSize = g.MeasureString(c.ToString(), font);
                    //逐一写入字符
                    g.DrawString(c.ToString(), font, brush, pf);
                    //设置字间距
                    pf.X += (charSize.Width - 0);
                    //设置行高
                    if (pf.X > 1000)
                    {
                        pf.X = 10;
                        pf.Y += (charSize.Height + 5);
                    }
                }

                //g.DrawString(randomCode, font, brush, 3, 2);

                //画图片的前景干扰线
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //保存图片数据
                return image;
            }
            finally
            {
                g.Dispose();
            }
        }

        /// <summary>
        /// 创建验证码图
        /// </summary>
        /// <param name="codeLength">随机Code长度</param>
        /// <param name="captchaType">默认简单类型</param>
        public static Image Create(int codeLength = 4, CaptchaType captchaType = CaptchaType.Simple)
        {
            return Create(RandomCode(codeLength), captchaType);
        }
    }
}
