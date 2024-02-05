using Microsoft.AspNetCore.Mvc;

namespace NIHR.StudyManagement.Api.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            return Ok();
        }
    }
}
