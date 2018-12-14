using System.Web;
using System.Web.Mvc;

namespace Smartiks.Framework.Identity.Authentication.Owin.App
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
