using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CAF.Security
{
    public static class Password
    {
        /// <summary>
        /// 加密字符串长度应该大于8
        /// </summary>
        public static string strEncrKey = "P@ssw0rd";

        /// <summary>
        /// 获取新密码
        /// </summary>
        /// <param name="pwdlen">密码长度</param>
        /// <returns></returns>
        public static string GetNewPassword(int pwdlen)
        {
            var randomchars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var tmpstr = "";
            int iRandNum;
            var rnd = new Random();
            for (var i = 0; i < pwdlen; i++)
            {
                iRandNum = rnd.Next(randomchars.Length);
                tmpstr += randomchars[iRandNum];
            }
            return tmpstr;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string DesEncrypt(string strText)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                var inputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (System.Exception error)
            {
                throw error;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string DesDecrypt(string strText)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            var inputByteArray = new Byte[strText.Length];
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                var des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = new System.Text.UTF8Encoding();
                return encoding.GetString(ms.ToArray());
            }
            catch (System.Exception error)
            {
                throw error;
            }
        }

        /// <summary>
        /// 生成MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}