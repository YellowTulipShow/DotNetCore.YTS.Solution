using System;
using System.Linq;
using YTS.Shop;
using YTS.Shop.Models;
using YTS.Shop.Tools;

namespace YTS.WebApi
{
    public interface IUserService
    {
        /// <summary>
        /// 判断是否验证通过
        /// </summary>
        Managers IsValid(LoginRequestDTO req);
    }

    public class UserService : IUserService
    {
        protected YTSEntityContext db;
        public UserService(YTSEntityContext db)
        {
            this.db = db;
        }

        public Managers IsValid(LoginRequestDTO req)
        {
            if (string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Password))
                return null;
            var encryPwd = ManageAuthentication.EncryptionPassword(req.Password);
            return db.Managers
                .Where(m => m.Account == req.UserName && m.Password == encryPwd)
                .FirstOrDefault();
        }
    }
}
