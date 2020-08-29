using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YTS.Tools;
using YTS.WebApi;

namespace YTS.AdminWebApi.Controllers
{
    /// <summary>
    /// 认证方式
    /// </summary>
    [ApiController]
    [Route("/SystemAuthentication")]
    [EnableCors(ApiConfig.CorsName)]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authService;
        public AuthenticationController(IAuthenticateService authService)
        {
            this._authService = authService;
        }

        /// <summary>
        /// 根据账户密码交换Token信息
        /// </summary>
        /// <param name="request">账户信息</param>
        /// <returns>返回结果</returns>
        [AllowAnonymous]
        [HttpPost, Route("RequestToken")]
        public ActionResult RequestToken(LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }
            if (_authService.IsAuthenticated(request, out string token))
            {
                return Ok(token);
            }
            return BadRequest("Invalid Request");
        }
    }
}
