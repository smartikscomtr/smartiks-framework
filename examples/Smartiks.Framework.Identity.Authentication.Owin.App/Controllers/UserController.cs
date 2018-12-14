using System.Web.Http;

namespace Smartiks.Framework.Identity.Authentication.Owin.App.Controllers
{
    public class UserController : ApiController
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
