using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Yiwan.Helpers.Security
{
    /// <summary>
    /// Asymmetric非对称加密实现类
    /// 目前还未整理
    /// </summary>
    public static class AsymmetricHelper
    {
        //private static string RSAPubicKey;
        //private static string RSAPrivateKey;

        ///// <summary>
        ///// Generates the RSA key.
        ///// </summary>
        ///// <param name="algorithm">The algorithm.</param>
        ///// <returns></returns>
        //public static void GenerateRSAKey(RSACryptoServiceProvider algorithm)
        //{
        //    if (algorithm == null) return;

        //    RSAPrivateKey = algorithm.ToXmlString(true);

        //    using (StreamWriter streamWriter = new StreamWriter("PublicPrivateKey.xml"))
        //    {
        //        streamWriter.Write(RSAPrivateKey);
        //    }

        //    RSAPubicKey = algorithm.ToXmlString(false);
        //    using (StreamWriter streamWriter = new StreamWriter("PublicOnlyKey.xml"))
        //    {
        //        streamWriter.Write(RSAPubicKey);
        //    }

        //}

        ///// <summary>
        ///// Encrypts the specified algorithm.
        ///// The Asymmetric Algorithm includes DSA，ECDiffieHellman， ECDsa and RSA.
        ///// Code Logic:
        ///// 1. Input encrypt algorithm and plain text.
        ///// 2. Read the public key from stream.
        ///// 3. Serialize plian text to byte array.
        ///// 4. Encrypt the plian text array by public key.
        ///// 5. Return ciphered string.
        ///// </summary>
        ///// <param name="algorithm">The algorithm.</param>
        ///// <param name="plainText">The plain text.</param>
        ///// <returns></returns>
        //public static string Encrypt(RSACryptoServiceProvider algorithm, string plainText)
        //{
        //    string publicKey;
        //    List<byte[]> cipherArray = new List<byte[]>();

        //    //// read the public key.
        //    using (StreamReader streamReader = new StreamReader("PublicOnlyKey.xml"))
        //    {
        //        publicKey = streamReader.ReadToEnd();
        //    }
        //    algorithm.FromXmlString(publicKey);

        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    byte[] plainBytes = null;


        //    //// Use BinaryFormatter to serialize plain text.
        //    using (MemoryStream memoryStream = new MemoryStream())
        //    {
        //        binaryFormatter.Serialize(memoryStream, plainText);
        //        plainBytes = memoryStream.ToArray();
        //    }

        //    int totLength = 0;
        //    int index = 0;

        //    //// Encrypt plain text by public key.
        //    if (plainBytes.Length > 80)
        //    {
        //        byte[] partPlainBytes;
        //        byte[] cipherBytes;
        //        List<byte[]> myArray = new List<byte[]>();
        //        while (plainBytes.Length - index > 0)
        //        {
        //            partPlainBytes = plainBytes.Length - index > 80 ? new byte[80] : new byte[plainBytes.Length - index];

        //            for (int i = 0; i < 80 && (i + index) < plainBytes.Length; i++)
        //                partPlainBytes[i] = plainBytes[i + index];
        //            myArray.Add(partPlainBytes);

        //            cipherBytes = algorithm.Encrypt(partPlainBytes, false);
        //            totLength += cipherBytes.Length;
        //            index += 80;

        //            cipherArray.Add(cipherBytes);
        //        }
        //    }

        //    //// Convert to byte array.
        //    byte[] cipheredPlaintText = new byte[totLength];
        //    index = 0;
        //    foreach (byte[] item in cipherArray)
        //    {
        //        for (int i = 0; i < item.Length; i++)
        //        {
        //            cipheredPlaintText[i + index] = item[i];
        //        }

        //        index += item.Length;
        //    }
        //    return Convert.ToBase64String(cipheredPlaintText);

        //}

        ///// <summary>
        ///// Decrypts the specified algorithm.
        ///// </summary>
        ///// <param name="algorithm">The algorithm.</param>
        ///// <param name="base64Text">The base64 text.</param>
        ///// <returns></returns>
        //public static string Decrypt(RSACryptoServiceProvider algorithm, string base64Text)
        //{
        //    byte[] cipherBytes = Convert.FromBase64String(base64Text);
        //    List<byte[]> plainArray = new List<byte[]>();
        //    string privateKey = string.Empty;

        //    //// Read the private key.
        //    using (StreamReader streamReader = new StreamReader("PublicPrivateKey.xml"))
        //    {
        //        privateKey = streamReader.ReadToEnd();
        //    }

        //    algorithm.FromXmlString(privateKey);

        //    int index = 0;
        //    int totLength = 0;
        //    byte[] partPlainText = null;
        //    byte[] plainBytes;
        //    int length = cipherBytes.Length / 2;
        //    //int j = 0;
        //    //// Decrypt the ciphered text through private key.
        //    while (cipherBytes.Length - index > 0)
        //    {
        //        partPlainText = cipherBytes.Length - index > 128 ? new byte[128] : new byte[cipherBytes.Length - index];

        //        for (int i = 0; i < 128 && (i + index) < cipherBytes.Length; i++)
        //            partPlainText[i] = cipherBytes[i + index];

        //        plainBytes = algorithm.Decrypt(partPlainText, false);

        //        totLength += plainBytes.Length;
        //        index += 128;
        //        plainArray.Add(plainBytes);

        //    }

        //    byte[] recoveredPlaintext = new byte[length];
        //    //List<byte[]> recoveredArray;
        //    index = 0;
        //    for (int i = 0; i < plainArray.Count; i++)
        //    {
        //        for (int j = 0; j < plainArray[i].Length; j++)
        //        {
        //            recoveredPlaintext[index + j] = plainArray[i][j];
        //        }
        //        index += plainArray[i].Length;
        //    }

        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        stream.Write(recoveredPlaintext, 0, recoveredPlaintext.Length);
        //        stream.Position = 0;
        //        string msgobj = (string)bf.Deserialize(stream);
        //        return msgobj;
        //    }

        //}
    }
}
