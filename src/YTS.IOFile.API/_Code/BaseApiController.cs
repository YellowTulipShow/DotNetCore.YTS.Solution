using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace YTS.IOFile.API
{
    /// <summary>
    /// »ù´¡API
    /// </summary>
    [ApiController]
    [Route(ApiConfig.APIRoute)]
    [EnableCors(ApiConfig.CorsName)]
    //[Authorize]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
