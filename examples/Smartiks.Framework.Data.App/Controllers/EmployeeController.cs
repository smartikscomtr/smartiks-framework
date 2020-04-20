using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.Data.Abstractions;
using Smartiks.Framework.Data.App.Data;
using Smartiks.Framework.Data.App.Model;
using Smartiks.Framework.Data.EntityFramework;
using Smartiks.Framework.Data.EntityFramework.Web;

namespace Smartiks.Framework.Data.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ContextApiController<DataContext, Employee, Employee, Employee, Employee, Employee, int>
    {
        public EmployeeController(ContextRepository<DataContext, Employee, Query<Employee>, int> repository) : base(repository)
        {
        }
    }
}