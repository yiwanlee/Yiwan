/// <summary>
/// 类说明：MD5Helper
/// 联系方式：liley@foxmail.com 秋秋:897250000
/// </summary>

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Yiwan.Helpers.Security
{
    /// <summary>
    /// Hash加密实现类，包含MD5、MD160、SHA1、SHA256、SHA384、SHA512，不可逆
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// Hash加密的基础算法
        /// </summary>
        internal static byte[] BaseForBytes(HashAlgorithm hashAlg, string txt, Encoding encoding = null)
        {
            if (hashAlg == null) return null;
            if (encoding == null) encoding = Encoding.Default;
            byte[] buffer = encoding.GetBytes(txt);
            return hashAlg.ComputeHash(buffer);
        }

        /// <summary>
        /// 标准的MD5算法实现，长度32，已不安全不推荐使用(请使用SHA256替代)
        /// 速度快于SHA1，强度低于SHA1
        /// </summary>
        public static byte[] MD5ForBytes(string txt, Encoding encoding = null)
        {
            using (MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                return BaseForBytes(md5, txt, encoding);
            }
        }

        /// <summary>
        /// 标准的MD5算法实现，长度32，已不安全不推荐使用(请使用SHA256替代)
        /// 速度快于SHA1，强度低于SHA1
        /// </summary>
        public static string MD5(string txt, Encoding encoding = null)
        {
            byte[] newBuffer = MD5ForBytes(txt, encoding);
            return BitConverter.ToString(newBuffer).Replace("-", "");
        }

        /// <summary>
        /// 基于RIPEMD160的MD160实现，弱加密算法，长度40，已不安全不推荐使用
        /// </summary>
        public static byte[] MD160ForBytes(string txt, Encoding encoding = null)
        {
            using (RIPEMD160 md160 = RIPEMD160.Create())
            {
                return BaseForBytes(md160, txt, encoding);
            }
        }

        /// <summary>
        /// 基于RIPEMD160的MD160实现，弱加密算法，长度40，已不安全不推荐使用
        /// </summary>
        public static string MD160(string txt, Encoding encoding = null)
        {
            byte[] newBuffer = MD160ForBytes(txt, encoding);
            return BitConverter.ToString(newBuffer).Replace("-", "");
        }

        /// <summary>
        /// 基于SHA1的实现，弱加密算法，长度40，已不安全不推荐使用
        /// 速度慢于MD5，强度高于MD5
        /// </summary>
        public static byte[] SHA1ForBytes(string txt, Encoding encoding = null)
        {
            using (SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
            {
                return BaseForBytes(sha1, txt, encoding);
            }
        }

        /// <summary>
        /// 基于SHA1的实现，弱加密算法，长度40，已不安全不推荐使用
        /// 速度慢于MD5，强度高于MD5
        /// </summary>
        public static string SHA1(string txt, Encoding encoding = null)
        {
            byte[] newBuffer = SHA1ForBytes(txt, encoding);
            return BitConverter.ToString(newBuffer).Replace("-", "");
        }

        /// <summary>
        /// 基于SHA256的实现，安全性更高，长度64，推荐使用
        /// </summary>
        public static byte[] SHA256ForBytes(string txt, Encoding encoding = null)
        {
            using (SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return BaseForBytes(sha256, txt, encoding);
            }
        }

        /// <summary>
        /// 基于SHA256的实现，安全性更高，长度64，推荐使用
        /// </summary>
        public static string SHA256(string txt, Encoding encoding = null)
        {
            byte[] newBuffer = SHA256ForBytes(txt, encoding);
            return BitConverter.ToString(newBuffer).Replace("-", "");
        }

        /// <summary>
        /// 基于SHA384的实现，安全性更高，长度96，推荐使用
        /// </summary>
        public static byte[] SHA384ForBytes(string txt, Encoding encoding = null)
        {
            using (SHA384 sha384 = System.Security.Cryptography.SHA384.Create())
            {
                return BaseForBytes(sha384, txt, encoding);
            }
        }

        /// <summary>
        /// 基于SHA384的实现，安全性更高，长度96，推荐使用
        /// </summary>
        public static string SHA384(string txt, Encoding encoding = null)
        {
            byte[] newBuffer = SHA384ForBytes(txt, encoding);
            return BitConverter.ToString(newBuffer).Replace("-", "");
        }

        /// <summary>
        /// 基于SHA512的实现，安全性更高，长度128，推荐使用
        /// </summary>
        public static byte[] SHA512ForBytes(string txt, Encoding encoding = null)
        {
            using (SHA512 sha512 = System.Security.Cryptography.SHA512.Create())
            {
                return BaseForBytes(sha512, txt, encoding);
            }
        }

        /// <summary>
        /// 基于SHA512的实现，安全性更高，长度128，推荐使用
        /// </summary>
        public static string SHA512(string txt, Encoding encoding = null)
        {
            byte[] newBuffer = SHA512ForBytes(txt, encoding);
            return BitConverter.ToString(newBuffer).Replace("-", "");
        }

        /// <summary>
        /// 确定是否一致
        /// </summary>
        internal static bool IsMatch(HashAlgorithm hashAlg, string hashedText, string unhashedText, Encoding encoding = null)
        {
            string hashedTextToCompare = BitConverter.ToString(BaseForBytes(hashAlg, unhashedText, encoding)).Replace("-", "");
            return string.Compare(hashedText, hashedTextToCompare, false, CultureInfo.InvariantCulture) == 0;
        }

        /// <summary>
        /// 确定是否一致
        /// </summary>
        public static bool MD5IsMatch(string hashedText, string unhashedText, Encoding encoding = null)
        {
            using (MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                return IsMatch(md5, hashedText, unhashedText, encoding);
            }
        }

        /// <summary>
        /// 确定是否一致
        /// </summary>
        public static bool MD160IsMatch(string hashedText, string unhashedText, Encoding encoding = null)
        {
            using (RIPEMD160 md160 = RIPEMD160.Create())
            {
                return IsMatch(md160, hashedText, unhashedText, encoding);
            }
        }

        /// <summary>
        /// 确定是否一致
        /// </summary>
        public static bool SHA1IsMatch(string hashedText, string unhashedText, Encoding encoding = null)
        {
            using (SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
            {
                return IsMatch(sha1, hashedText, unhashedText, encoding);
            }
        }

        /// <summary>
        /// 确定是否一致
        /// </summary>
        public static bool SHA256IsMatch(string hashedText, string unhashedText, Encoding encoding = null)
        {
            using (SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                return IsMatch(sha256, hashedText, unhashedText, encoding);
            }
        }

        /// <summary>
        /// 确定是否一致
        /// </summary>
        public static bool SHA384IsMatch(string hashedText, string unhashedText, Encoding encoding = null)
        {
            using (SHA384 sha384 = System.Security.Cryptography.SHA384.Create())
            {
                return IsMatch(sha384, hashedText, unhashedText, encoding);
            }
        }

        /// <summary>
        /// 确定是否一致
        /// </summary>
        public static bool SHA512IsMatch(string hashedText, string unhashedText, Encoding encoding = null)
        {
            using (SHA512 sha512 = System.Security.Cryptography.SHA512.Create())
            {
                return IsMatch(sha512, hashedText, unhashedText, encoding);
            }
        }
    }
}
