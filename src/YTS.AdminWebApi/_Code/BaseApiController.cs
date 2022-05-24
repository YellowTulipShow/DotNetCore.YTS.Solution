using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YTS.Shop;
using YTS.Shop.Models;
using YTS.Tools;

namespace YTS.WebApi
{
    [ApiController]
    [Route(ApiConfig.APIRoute)]
    [EnableCors(ApiConfig.CorsName)]
    [Authorize]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// 获取JWT验证中写入的自定义荷载的值
        /// </summary>
        /// <param name="keyName">自定义的键名称</param>
        /// <returns>返回自定义的键代表的值内容</returns>
        protected string GetJwtPayloadValue(string keyName)
        {
            var auth = HttpContext.AuthenticateAsync();
            return auth.Result.Principal.Claims.FirstOrDefault(m => m.Type.Equals(keyName))?.Value;
        }

        private Managers _manager;

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="db">需要传入的数据库上下文</param>
        protected Managers GetManager(YTSEntityContext db)
        {
            if (_manager != null)
                return _manager;
            int ID = ConvertTool.ToInt(GetJwtPayloadValue(ApiConfig.ClainKey_ManagerID), 0);
            string Account = GetJwtPayloadValue(ApiConfig.ClainKey_ManagerName);
            var model = db.Managers.Where(m => m.ID == ID && m.Account == m.Account).FirstOrDefault();
            if (model == null)
                throw new NullReferenceException("身份验证已过期!");
            _manager = model;
            return model;
        }
    }
}
