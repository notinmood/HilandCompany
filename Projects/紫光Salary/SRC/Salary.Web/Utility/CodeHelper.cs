using System;
using System.IO;
using System.Security.Cryptography;
using System.Web;

namespace Salary.Web.Utility
{
    /// <summary>
    /// 编码工具类

    /// </summary>
    public static class CodeHelper
    {
        #region EncodeQueryString
        private static byte[] SecureKey = { 0x24, 0xE5, 0xFF, 0x29, 0xC6, 0x84, 0x43, 0x0A };


        /// <summary>
        /// 加密一个QueryString
        /// </summary>
        /// <param name="s">需要加密的QueryString</param>
        /// <returns>加密后的串</returns>
        public static string EncodeQueryString(string s)
        {
            return
                HttpUtility.UrlEncode(
                    BytesToStr(
                        Encode(
                            System.Text.Encoding.GetEncoding("gb2312").GetBytes(s)
                        )
                    )
                    , System.Text.Encoding.UTF8
                );
        }

        /// <summary>
        /// 解密一个QueryString
        /// </summary>
        /// <param name="queryString">需要解密的QueryString</param>
        /// <returns>解密后的串</returns>
        public static string DecodeQueryString(string queryString)
        {
            return
                System.Text.Encoding.GetEncoding("gb2312").GetString(
                    Decode(
                        StrToHex(
                        HttpUtility.UrlDecode(
                                queryString,
                                System.Text.Encoding.UTF8
                        )
                        )
                    )
                );
        }

        private static byte[] Decode(byte[] data)
        {
            byte[] rtn;

            System.IO.MemoryStream ms = new MemoryStream();

            byte[] key = SecureKey;

            DES des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            des.Key = key;
            CryptoStream decStream = new CryptoStream(ms, des.CreateDecryptor(key, key), CryptoStreamMode.Write);

            decStream.Write(data, 0, data.Length);
            decStream.FlushFinalBlock();
            rtn = ms.ToArray();

            decStream.Close();
            ms.Close();

            return rtn;

        }

        private static byte[] Encode(byte[] data)
        {
            byte[] rtn;

            System.IO.MemoryStream ms = new MemoryStream();

            byte[] key = SecureKey;

            DES des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            des.Key = key;

            CryptoStream decStream = new CryptoStream(ms, des.CreateEncryptor(key, key), CryptoStreamMode.Write);

            decStream.Write(data, 0, data.Length);
            decStream.FlushFinalBlock();
            rtn = ms.ToArray();

            decStream.Close();
            ms.Close();

            return rtn;
        }

        private static byte[] StrToHex(string key)
        {
            int length = key.Length;
            var count = length / 2;
            byte[] keytemp = new byte[count];

            for (int i = 0; i < keytemp.Length; i++)
            {
                keytemp[i] = byte.Parse(key.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            if ((length & 1) != 0)
            {
                byte[] keytemp1 = new byte[count + 1];
                Array.Copy(keytemp, keytemp1, count);
                keytemp1[count] = byte.Parse(key[length - 1].ToString(), System.Globalization.NumberStyles.HexNumber);
                keytemp = keytemp1;
            }
            return keytemp;
        }

        private static string BytesToStr(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
        #endregion
    }
}
