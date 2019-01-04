using System;
using System.Linq.Expressions;

namespace Smartiks.Framework.Data.Abstractions
{
    public class QueryCriteria<TQueryable>
    {
        public virtual Expression<Func<TQueryable, bool>> Expression { get; set; }
    }
}
