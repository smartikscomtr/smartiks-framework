using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.Data.Abstractions;
using Smartiks.Framework.Data.App.Data;
using Smartiks.Framework.Data.App.Model;
using Smartiks.Framework.Data.EntityFramework;
using Smartiks.Framework.Data.EntityFramework.Web;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Smartiks.Framework.Data.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ContextApiController<DataContext, Employee, Employee, Employee, Employee, Employee, int>
    {
        public EmployeeController(ContextRepository<DataContext, Employee, Query<Employee>, int> repository) : base(repository)
        {

        }

        public async override Task<Employee> Get(int id)
        {
            Expression<Func<Employee, string>> selectName = p => p.Name;

            var query = new Query<Employee>
            {
                Segment = new QuerySegment
                {
                    Count = 2,
                    StartIndex = 2
                },
                Orders = new System.Collections.Generic.List<QueryOrder>()
                {
                    new QueryOrder
                    {
                        IsDescending = true,
                        Expression = selectName
                    }
                }
            };

            var a = await base.Get(query);

            return await base.Get(id);
        }
    }
}