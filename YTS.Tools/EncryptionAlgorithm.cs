using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YTS.Tools
{
    public static class EncryptionAlgorithm
    {
        /// <summary>
        /// MD5 32位加密
        /// </summary>
        public static string MD5_32(string source)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5 16位加密
        /// </summary>
        public static string MD5_16(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(source)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// 对称加密类的构造函数
        /// </summary>
        public class Symmetric
        {
            private RijndaelManaged mobjCryptoService;
            private string Key;

            public Symmetric()
            {
                mobjCryptoService = new RijndaelManaged();
                Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
            }

            /// <summary>
            /// 获得密钥
            /// </summary>
            /// <returns>密钥</returns>
            private byte[] GetLegalKey()
            {
                string sTemp = Key;
                mobjCryptoService.GenerateKey();
                byte[] bytTemp = mobjCryptoService.Key;
                int KeyLength = bytTemp.Length;
                if (sTemp.Length > KeyLength)
                    sTemp = sTemp.Substring(0, KeyLength);
                else if (sTemp.Length < KeyLength)
                    sTemp = sTemp.PadRight(KeyLength, ' ');
                return ASCIIEncoding.ASCII.GetBytes(sTemp);
            }

            /// <summary>
            /// 获得初始向量IV
            /// </summary>
            /// <returns>初试向量IV</returns>
            private byte[] GetLegalIV()
            {
                string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
                mobjCryptoService.GenerateIV();
                byte[] bytTemp = mobjCryptoService.IV;
                int IVLength = bytTemp.Length;
                if (sTemp.Length > IVLength)
                    sTemp = sTemp.Substring(0, IVLength);
                else if (sTemp.Length < IVLength)
                    sTemp = sTemp.PadRight(IVLength, ' ');
                return ASCIIEncoding.ASCII.GetBytes(sTemp);
            }

            /// <summary>
            /// 加密方法
            /// </summary>
            /// <param name="Source">待加密的串</param>
            /// <returns>经过加密的串</returns>
            public string Encrypto(string Source)
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
                MemoryStream ms = new MemoryStream();
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }

            /// <summary>
            /// 解密方法
            /// </summary>
            /// <param name="Source">待解密的串</param>
            /// <returns>经过解密的串</returns>
            public string Decrypto(string Source)
            {
                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
        }
    }
}
