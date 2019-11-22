using System.Text.RegularExpressions;

namespace Yiwan.Helpers.Extensions
{
    public static class StringExtentsion
    {
        /// <summary>
        /// 转换为Url安全的Base64字符串
        /// </summary>
        /// <param name="ignoreEqualSign">是否替换掉补位的=号</param>
        public static string ToBase64UrlSafe(this string input, bool ignoreEqualSign = true)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            string safed = input.Replace("+", "-").Replace("/", "_");
            if (ignoreEqualSign) return safed.Replace("=", "");
            else return safed;
        }

        /// <summary>
        /// 转换为Url不安全的原始Base64字符串
        /// </summary>
        public static string ToBase64UrlUnSafe(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            string unsafed = input.Replace("-", "+").Replace("_", "/");
            int mod4 = unsafed.Length % 4;
            if (mod4 > 0) unsafed += ("====".Substring(0, 4 - mod4));
            return unsafed;
        }

        //扩展方法必须是静态的，第一个参数必须加上this
        public static bool IsEmail(this string input)
        {
            return Regex.IsMatch(input, @"^\w+@\w+\.\w+$");
        }
    }
}
