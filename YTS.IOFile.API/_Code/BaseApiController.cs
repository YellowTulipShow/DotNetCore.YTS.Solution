using System;
using System.Linq;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace YTS.IOFile.API
{
    [ApiController]
    [Route(ApiConfig.APIRoute)]
    [EnableCors(ApiConfig.CorsName)]
    //[Authorize]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
