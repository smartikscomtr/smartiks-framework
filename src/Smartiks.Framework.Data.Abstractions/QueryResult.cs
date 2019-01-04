using System.Collections.Generic;

namespace Smartiks.Framework.Data.Abstractions
{
    public class QueryResult<T>
    {
        public virtual IEnumerable<T> Items { get; set; }

        public virtual int? TotalCount { get; set; }
    }
}
