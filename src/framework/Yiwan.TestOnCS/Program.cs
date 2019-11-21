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


            string name = "liley";
            byte[] bytes = Encoding.UTF8.GetBytes(name);

            StringBuilder sbRes = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sbRes.Append(bytes[i].ToString("x", CultureInfo.InvariantCulture));
            }
            Console.WriteLine(sbRes.ToString());

            Console.WriteLine(BitConverter.ToString(bytes));

            Console.WriteLine(Encoding.UTF8.GetString(bytes));

            Console.WriteLine("MD5：" + Helpers.Security.MD5Helper.GenerateMD5("liley2"));
            Console.WriteLine("MD5：" + Helpers.Security.MD5Helper.GetMD5("liley2"));
            Console.WriteLine(Helpers.Security.MD5Helper.ToMD532("liley"));

            string original = "Here is some data to encrypt!";

            // Create a new instance of the AesManaged
            // class.  This generates a new key and initialization 
            // vector (IV).
            using (AesManaged myAes = new AesManaged())
            {
                byte[] bts = Helpers.Security.MD5Helper.HexToByte(Helpers.Security.MD5Helper.GenerateMD5("liley"));

                Console.WriteLine($"KEY:{BitConverter.ToString(myAes.Key).Replace("-", "")}");
                Console.WriteLine($"IV:{BitConverter.ToString(myAes.IV)}");
                // Encrypt the string to an array of bytes.
                byte[] encrypted = Helpers.Security.MD5Helper.EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

                Console.WriteLine($"KEY2:{BitConverter.ToString(encrypted).Replace("-", "")}");

                // Decrypt the bytes to a string.
                string roundtrip = Helpers.Security.MD5Helper.DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

                //Display the original data and the decrypted data.
                Console.WriteLine("Original:   {0}", original);
                Console.WriteLine("Round Trip: {0}", roundtrip);
            }






















            while (true) Console.ReadKey();
        }
    }
}
