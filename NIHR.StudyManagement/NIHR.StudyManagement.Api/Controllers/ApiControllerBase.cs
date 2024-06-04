using Microsoft.AspNetCore.Mvc;
using NIHR.StudyManagement.Api.EnumsAndConstants;

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

        protected string ApiConsumerSystemName
        {
            get
            {
                foreach (var userClaim in this.User.Claims)
                {
                    if (userClaim.Type == UserTokenClaimNames.ApiSystemName)
                    {
                        return userClaim.Value;
                    }
                }

                return "";
            }
        }
    }
}
