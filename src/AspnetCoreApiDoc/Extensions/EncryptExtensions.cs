using System;
using System.Security.Cryptography;
using System.Text;

namespace AspnetCoreApiDoc.Extensions
{
    /// <summary>
    /// 加密扩展
    /// </summary>
    internal static class EncryptExtensions
    {
        private const string _key = @"BQEfde0ZxDZJagIwpXryGCKkbQ7XdZRt";

        #region ----------DES------------
        //资料来源：http://www.cnblogs.com/CreateMyself/p/6759996.html

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string DESEncrypt(this string input, string key = _key)
        {
            byte[] inputArray = Encoding.UTF8.GetBytes(input);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string DESDecrypt(this string input, string key = _key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            var tripleDES = TripleDES.Create();
            var byteKey = Encoding.UTF8.GetBytes(key);
            byte[] allKey = new byte[24];
            Buffer.BlockCopy(byteKey, 0, allKey, 0, 16);
            Buffer.BlockCopy(byteKey, 0, allKey, 16, 8);
            tripleDES.Key = allKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region ---------BASE64----------

        /// <summary>
        /// BASE64 加密
        /// </summary>
        /// <param name="source">待加密字段</param>
        /// <returns></returns>
        internal static string BASE64Encrypt(this string source)
        {
            var btArray = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(btArray, 0, btArray.Length);
        }

        /// <summary>
        /// BASE64 解密
        /// </summary>
        /// <param name="source">待解密字段</param>
        /// <returns></returns>
        internal static string BASE64Decrypt(this string source)
        {
            var btArray = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(btArray);
        }

        #endregion

        #region ----------MD5------------

        /// <summary>
        /// MD5计算
        /// </summary>
        /// <returns></returns>
        internal static string ToMD5(this string str)
        {
            using (var md5 = MD5.Create())
            {
//                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
//                return Encoding.UTF8.GetString(result);
                
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }

        #endregion
    }
}
