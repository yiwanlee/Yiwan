using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Yiwan.Helpers.Security
{
    /// <summary>
    /// Symmetric对称加密实现类，包含AES、DES、RC2、Rijndael、TripleDESC
    /// 其中的实现中所有的KEY、IV均由传入的key字符串基于SHA256生成
    /// </summary>
    public static class SymmetricHelper
    {
        internal static byte[] Subbytes(this byte[] bytes, int index, int length)
        {
            if (bytes == null) return null;
            byte[] newBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                if (bytes.Length > index + i) newBytes[i] = bytes[index + i];
            }
            return newBytes;
        }

        /// <summary>
        /// 基础对称加密算法，可解密，支持AES、DES、RC2、Rijndael、TripleDESC
        /// </summary>
        /// <param name="algorithm">对应算法</param>
        /// <param name="plainText">明文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>返回base64加密的字符串</returns>
        internal static string BaseEncrypt(SymmetricAlgorithm algorithm, string plainText, string key,
            CipherMode cipherMode, PaddingMode paddingMode, Encoding encoding = null)
        {
            if (algorithm == null) return string.Empty;
            if (string.IsNullOrWhiteSpace(key)) key = "@yiwanlee";

            byte[] plainBytes;
            byte[] cipherBytes;
            algorithm.Key = HashHelper.SHA256ForBytes(key, encoding).Subbytes((key.Length % algorithm.Key.Length) + 1, algorithm.Key.Length);
            algorithm.IV = HashHelper.SHA256ForBytes(key, encoding).Subbytes((key.Length % algorithm.IV.Length) + 1, algorithm.IV.Length);
            algorithm.Mode = cipherMode;
            algorithm.Padding = paddingMode;

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream())
            {
                bf.Serialize(stream, plainText);
                plainBytes = stream.ToArray();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                // 定义用于加密转换的流
                CryptoStream cs = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
                // 写入加密后的序列
                cs.Write(plainBytes, 0, plainBytes.Length);
                // 关闭当前流并释放任何资源
                cs.Close();
                // 将加密的消息保存到一个字节数组中
                cipherBytes = ms.ToArray();
                // 关闭memorystream对象
                ms.Close();
            }
            string base64Text = Convert.ToBase64String(cipherBytes);

            return base64Text;
        }

        /// <summary>
        /// 基础对称解密算法，支持AES、DES、RC2、Rijndael、TripleDESC
        /// </summary>
        /// <param name="algorithm">对应算法</param>
        /// <param name="base64Text">BASE64的密文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>解密出来的文本</returns>
        internal static string BaseDecrypt(SymmetricAlgorithm algorithm, string base64Text, string key,
            CipherMode cipherMode, PaddingMode paddingMode, Encoding encoding = null)
        {
            if (algorithm == null) return string.Empty;
            if (string.IsNullOrWhiteSpace(key)) key = "@yiwanlee";

            byte[] plainBytes;
            // 将base64字符串转换为字节数组。
            byte[] cipherBytes = Convert.FromBase64String(base64Text);
            algorithm.Key = HashHelper.SHA256ForBytes(key, encoding).Subbytes((key.Length % algorithm.Key.Length) + 1, algorithm.Key.Length);
            algorithm.IV = HashHelper.SHA256ForBytes(key, encoding).Subbytes((key.Length % algorithm.IV.Length) + 1, algorithm.IV.Length);
            algorithm.Mode = cipherMode;
            algorithm.Padding = paddingMode;

            using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
            {
                using (CryptoStream cs = new CryptoStream(memoryStream,
                    algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    plainBytes = new byte[cipherBytes.Length];
                    cs.Read(plainBytes, 0, cipherBytes.Length);
                }
            }

            string recoveredMessage;
            using (MemoryStream stream = new MemoryStream(plainBytes, false))
            {
                BinaryFormatter bf = new BinaryFormatter();
                recoveredMessage = bf.Deserialize(stream).ToString();
            }

            return recoveredMessage;
        }

        /// <summary>
        /// Aes对称算法 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>返回base64加密的字符串</returns>
        public static string AesEncrypt(string plainText, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (Aes aes = Aes.Create())
            {
                return BaseEncrypt(aes, plainText, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// Aes对称算法 解密
        /// </summary>
        /// <param name="base64Text">BASE64的密文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>解密出来的文本</returns>
        public static string AesDecrypt(string base64Text, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (Aes aes = Aes.Create())
            {
                return BaseDecrypt(aes, base64Text, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// DES对称算法 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>返回base64加密的字符串</returns>
        public static string DESEncrypt(string plainText, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (DES des = DES.Create())
            {
                return BaseEncrypt(des, plainText, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// DES对称算法 解密
        /// </summary>
        /// <param name="base64Text">BASE64的密文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>解密出来的文本</returns>
        public static string DESDecrypt(string base64Text, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (DES des = DES.Create())
            {
                return BaseDecrypt(des, base64Text, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// RC2对称算法 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>返回base64加密的字符串</returns>
        public static string RC2Encrypt(string plainText, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (RC2 rc2 = RC2.Create())
            {
                return BaseEncrypt(rc2, plainText, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// RC2对称算法 解密
        /// </summary>
        /// <param name="base64Text">BASE64的密文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>解密出来的文本</returns>
        public static string RC2Decrypt(string base64Text, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (RC2 rc2 = RC2.Create())
            {
                return BaseDecrypt(rc2, base64Text, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// Rijndael对称算法 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>返回base64加密的字符串</returns>
        public static string RijndaelEncrypt(string plainText, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (Rijndael rj = Rijndael.Create())
            {
                return BaseEncrypt(rj, plainText, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// Rijndael对称算法 解密
        /// </summary>
        /// <param name="base64Text">BASE64的密文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>解密出来的文本</returns>
        public static string RijndaelDecrypt(string base64Text, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (Rijndael rj = Rijndael.Create())
            {
                return BaseDecrypt(rj, base64Text, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// TripleDES对称算法 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>返回base64加密的字符串</returns>
        public static string TripleDESEncrypt(string plainText, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (TripleDES tdes = TripleDES.Create())
            {
                return BaseEncrypt(tdes, plainText, key, cipherMode, paddingMode, encoding);
            }
        }

        /// <summary>
        /// TripleDES对称算法 解密
        /// </summary>
        /// <param name="base64Text">BASE64的密文</param>
        /// <param name="key">用于加解密的Key</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <returns>解密出来的文本</returns>
        public static string TripleDESDecrypt(string base64Text, string key,
            CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, Encoding encoding = null)
        {
            using (TripleDES tdes = TripleDES.Create())
            {
                return BaseDecrypt(tdes, base64Text, key, cipherMode, paddingMode, encoding);
            }
        }
    }
}
