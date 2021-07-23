using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Provides default route, all new controllers must inherit it.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}