/// <summary>
/// 类说明：Encrypt加解密操作类
/// 联系方式：liley@foxmail.com 秋秋:897250000
/// </summary>
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Yiwan.Helpers.Security
{
    /// <summary>
    /// Encrypt 的摘要说明。
    /// </summary>
    public static class Security
    {
        const string defaultKey = "Yiwanlee|liley@foxmail.com"; //缺省密钥字符串

        #region 使用 给定密钥字符串 加密/解密string
        /// <summary>
        /// 使用给定密钥字符串加密string
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥（为空则使用缺省密钥）</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key = "")
        {
            key = key.Length < 9 ? defaultKey : key;//最少长度为9/也可能是8
            byte[] buff = Encoding.UTF8.GetBytes(original);
            byte[] kb = Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
            {
                Key = MD5Base64(kb),
                Mode = CipherMode.ECB
            };

            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
        }

        /// <summary>
        /// 使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥（为空则使用缺省密钥）</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encrypted, string key = "")
        {
            if (string.IsNullOrWhiteSpace(encrypted)) return "";
            key = key.Length < 9 ? defaultKey : key;//最少长度为9/也可能是8
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
            {
                Key = MD5Base64(kb),
                Mode = CipherMode.ECB
            };

            return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
        }
        #endregion 使用 给定密钥字符串 加密/解密string

        #region 使用 给定密钥 Url安全的
        /// <summary>
        /// 使用给定密钥加密（Url安全的）
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="ignoreEqualSign">是否替换掉补位的=号</param>
        /// <returns>密文</returns>
        public static string EncryptUrlSafe(string original, string key = "", bool ignoreEqualSign = true)
        {
            key = key.Length < 9 ? defaultKey : key;//最少长度为9/也可能是8
            if (ignoreEqualSign)
                return Encrypt(original, key).Replace("+", "-").Replace("/", "_").Replace("=", "");
            else
                return Encrypt(original, key).Replace("+", "-").Replace("/", "_");
        }

        /// <summary>
        /// 使用给定密钥解密数据（Url安全的）
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string DecryptUrlSafe(string encrypted, string key = "")
        {
            if (string.IsNullOrWhiteSpace(encrypted)) return "";
            key = key.Length < 9 ? defaultKey : key;//最少长度为9/也可能是8
            encrypted = encrypted.Replace("-", "+").Replace("_", "/");
            int mod4 = encrypted.Length % 4;
            if (mod4 > 0) encrypted += ("====".Substring(0, 4 - mod4));
            return Decrypt(encrypted, key);
        }
        #endregion 使用 给定密钥 Url安全的

        #region 生成MD5摘要
        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        public static string MD5Base64(string original, Encoding encoding = null)
        {
            if (encoding == null)
                return Convert.ToBase64String(MD5Base64(Encoding.UTF8.GetBytes(original)));
            else
                return Convert.ToBase64String(MD5Base64(encoding.GetBytes(original)));
        }

        /// <summary>
        /// 生成MD5摘要,私有
        /// </summary>
        private static byte[] MD5Base64(byte[] original)
        {
            using (MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider())
            {
                return hashmd5.ComputeHash(original);
            }
        }

        public static string MD5(string text, Encoding encoding = null)
        {
            if (encoding == null)
                return BitConverter.ToString(MD5Base64(Encoding.Unicode.GetBytes(text))).Replace("-", "");
            else
                return BitConverter.ToString(MD5Base64(encoding.GetBytes(text))).Replace("-", "");
        }

        #endregion 生成MD5摘要

        #region 3DES加解密
        public static string Encrypt3DES(string text, string key = "")
        {
            key = key.Length < 9 ? defaultKey : key;//最少长度为9/也可能是8

            var keyHex = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var ivHex = Encoding.UTF8.GetBytes(key.Substring(4, 4) + key.Substring(0, 4));
            var textHex = Encoding.UTF8.GetBytes(text);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(keyHex, ivHex), CryptoStreamMode.Write);

            cs.Write(textHex, 0, textHex.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        public static string Decrypt3DES(string text, string key = "")
        {
            key = key.Length < 9 ? defaultKey : key;//最少长度为9/也可能是8

            if (string.IsNullOrWhiteSpace(text)) return text;

            var keyHex = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var ivHex = Encoding.UTF8.GetBytes(key.Substring(4, 4) + key.Substring(0, 4));
            var textHex = Convert.FromBase64String(text);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(keyHex, ivHex), CryptoStreamMode.Write);

            cs.Write(textHex, 0, textHex.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion 3DESC加解密

        #region Base64 加解码
        /// <summary>
        /// Base64编码，参数2为空则默认Encoding.UTF8
        /// </summary>
        public static string EncodeBase64(string value, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// Base64编码（URL安全的），参数2为空则默认Encoding.UTF8
        /// </summary>
        public static string EncodeBase64UrlSafe(string value, Encoding encoding = null)
        {
            return EncodeBase64(value, encoding).Replace("+", "-").Replace("/", "_");
        }
        /// <summary>
        /// Base64解码，参数2为空则默认Encoding.UTF8
        /// </summary>
        public static string DecodeBase64(string value, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] bytes = Convert.FromBase64String(value);
            return encoding.GetString(bytes);
        }
        /// <summary>
        /// Base64解码（URL安全的），参数2为空则默认Encoding.UTF8
        /// </summary>
        public static string DecodeBase64UrlSafe(string value, Encoding encoding = null)
        {
            return DecodeBase64(value.Replace("-", "+").Replace("_", "/"), encoding);
        }
        #endregion Base64 加解码

        #region SHA1算法
        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="encrypted">未加密等待验证原始字符串</param>
        /// <param name="encode">编码方式，忽略则为UTF-8</param>
        public static string SHA1_Encrypt(string encrypted, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;
            byte[] byteEncrypted = encode.GetBytes(encrypted);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            byteEncrypted = iSHA.ComputeHash(byteEncrypted);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in byteEncrypted)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString().ToUpper();
        }

        /// <summary>
        /// SHA1字符串验证
        /// </summary>
        /// <param name="encrypted">未加密等待验证原始字符串</param>
        /// <param name="hashed">SHA1加密后的</param>
        /// <param name="encode">编码方式</param>
        public static bool SHA1_Verify(string encrypted, string hashed, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;
            string hashEncrypted = SHA1_Encrypt(encrypted, encode);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashEncrypted, hashed)) return true;
            else return false;
        }
        #endregion SHA1算法
    }
}
