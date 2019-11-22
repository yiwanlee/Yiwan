using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Yiwan.Helpers.Security;
using Yiwan.Helpers.Extensions;

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

            #region HashHelper测试 ▼▼▼▼▼

            Console.WriteLine("HashHelper测试 ▼▼▼▼▼\r\n");

            string key = "liley一万里芳华帅锅@@@";
            string key2 = "芳华万里一帅锅liley";
            Console.WriteLine(nameof(Helpers.Security.HashHelper.MD5) + "：\t\t" + Helpers.Security.HashHelper.MD5(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.MD5) + "：\t\t" + Helpers.Security.HashHelper.MD5(key2));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.MD160) + "：\t\t" + Helpers.Security.HashHelper.MD160(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.MD160) + "：\t\t" + Helpers.Security.HashHelper.MD160(key2).Length);
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA1) + "：\t\t" + Helpers.Security.HashHelper.SHA1(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA1) + "：\t\t" + Helpers.Security.HashHelper.SHA1(key2).Length);
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA256) + "：\t" + Helpers.Security.HashHelper.SHA256(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA256) + "：\t" + Helpers.Security.HashHelper.SHA256(key2));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA384) + "：\t" + Helpers.Security.HashHelper.SHA384(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA384) + "：\t" + Helpers.Security.HashHelper.SHA384(key2).Length);
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA512) + "：\t" + Helpers.Security.HashHelper.SHA512(key));
            Console.WriteLine(nameof(Helpers.Security.HashHelper.SHA512) + "：\t" + Helpers.Security.HashHelper.SHA512(key2).Length);

            Console.WriteLine("\r\nHashHelper测试 ▲▲▲▲▲");

            var aesStr = Helpers.Security.SymmetricHelper.AesEncrypt(key, null);
            Console.WriteLine($"AES加密结果：{aesStr.ToBase64UrlSafe()}");
            Console.WriteLine($"AES加密结果：{aesStr.ToBase64UrlUnSafe()}");
            Console.WriteLine($"AES解密结果：{Helpers.Security.SymmetricHelper.AesDecrypt(aesStr, null)}");

            var desStr = Helpers.Security.SymmetricHelper.DESEncrypt(key, null);
            Console.WriteLine($"DES加密结果：{desStr}");
            Console.WriteLine($"DES解密结果：{Helpers.Security.SymmetricHelper.DESDecrypt(desStr, null)}");

            var rc2Str = Helpers.Security.SymmetricHelper.RC2Encrypt(key, null);
            Console.WriteLine($"RC2加密结果：{rc2Str}");
            Console.WriteLine($"RC2解密结果：{Helpers.Security.SymmetricHelper.RC2Decrypt(rc2Str, null)}");

            var rjStr = Helpers.Security.SymmetricHelper.RijndaelEncrypt(key, null);
            Console.WriteLine($"RIJ加密结果：{rjStr}");
            Console.WriteLine($"RIJ解密结果：{Helpers.Security.SymmetricHelper.RijndaelDecrypt(rjStr, null)}");

            var tdesStr = Helpers.Security.SymmetricHelper.TripleDESEncrypt(key, null);
            Console.WriteLine($"TDE加密结果：{tdesStr}");
            Console.WriteLine($"TDE解密结果：{Helpers.Security.SymmetricHelper.TripleDESDecrypt(tdesStr, null)}");
            #endregion HashHelper测试 ▲▲▲▲▲

            string origPath = AppContext.BaseDirectory + "/测试1.txt";
            string savePath = AppContext.BaseDirectory + "/测试2.txt";
            string decrPath = AppContext.BaseDirectory + "/测试3.txt";

            Helpers.Security.SymmetricHelper.TripleDESEncryptFile(origPath, savePath, null);
            Helpers.Security.SymmetricHelper.TripleDESDecryptFile(savePath, decrPath, null);




















            while (true) Console.ReadKey();
        }
    }
}
