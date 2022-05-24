using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace YTS.WebApi
{
    public class LoginRequestDTO
    {
        /// <summary>
        /// 登录用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 登录用户密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
