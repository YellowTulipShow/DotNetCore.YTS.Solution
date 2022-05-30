using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace YTS.WEBAPI
{
    /// <summary>
    /// ����API
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors(Cors.ServiceExtend.CorsName)]
    //[Authorize]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
