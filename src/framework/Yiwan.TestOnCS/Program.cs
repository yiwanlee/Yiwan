using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Yiwan.TestOnCS
{
    internal class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("样式", "IDE0060:删除未使用的参数", Justification = "<挂起>")]
        private static void Main(string[] args)
        {
            #region Utilities测试 ▼▼▼▼▼

            Console.WriteLine("Utilities测试 ▼▼▼▼▼\r\n");

            Utilities.JsonUtilityTest.Do();

            Console.WriteLine("\r\nUtilities测试 ▲▲▲▲▲");

            #endregion Utilities测试 ▲▲▲▲▲

            string key = "liley一万里芳华帅锅";
            string key2 = "芳华万里一帅锅liley";
            Console.WriteLine(nameof(Helpers.Security.HashHelper.MD5) + "：\t\t" + Helpers.Security.HashHelper.MD5(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.MD5) + "：\t\t" + Helpers.Security.HashHelper.MD5(key2));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.RIPEMD160) + "：\t" + Helpers.Security.HashHelper.RIPEMD160(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.RIPEMD160) + "：\t" + Helpers.Security.HashHelper.RIPEMD160(key2).Length);
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA1) + "：\t" + Helpers.Security.HashHelper.SHA1(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA1) + "：\t" + Helpers.Security.HashHelper.SHA1(key2).Length);
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA256) + "：\t" + Helpers.Security.HashHelper.SHA256(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA256) + "：\t" + Helpers.Security.HashHelper.SHA256(key2));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA384) + "：\t" + Helpers.Security.HashHelper.SHA384(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA384) + "：\t" + Helpers.Security.HashHelper.SHA384(key2).Length);
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA512) + "：\t" + Helpers.Security.HashHelper.SHA512(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA512) + "：\t" + Helpers.Security.HashHelper.SHA512(key2).Length);






















            while (true) Console.ReadKey();
        }
    }
}
