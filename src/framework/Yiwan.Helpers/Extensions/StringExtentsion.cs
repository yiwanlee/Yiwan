using System.Text.RegularExpressions;

namespace Yiwan.Helpers.Extensions
{
    public static class StringExtentsion
    {
        //扩展方法必须是静态的，第一个参数必须加上this
        public static bool IsEmail(this string input)
        {
            return Regex.IsMatch(input, @"^\w+@\w+\.\w+$");
        }
    }
}
