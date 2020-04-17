using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Smartiks.Framework.Identity.Authentication.Web.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public object Get()
        {
            return new {
                User.Identity.Name
            };
        }
    }
}
