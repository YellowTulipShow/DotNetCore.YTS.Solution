using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using YTS.Tools;

namespace YTS.Shop.Tools
{
    /// <summary>
    /// 最高管理员
    /// </summary>
    public class ManageAuthentication
    {
        /// <summary>
        /// 加密管理员密码
        /// </summary>
        /// <param name="Password">密码原文</param>
        public static string EncryptionPassword(string Password)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new NullReferenceException("请输入有效的密码原文!");
            }
            string md5 = EncryptionAlgorithm.MD5_16(Password);
            var symmetric = new EncryptionAlgorithm.Symmetric();
            var md5s = symmetric.Encrypto(md5);
            return EncryptionAlgorithm.MD5_32(md5s);
        }
    }
}
