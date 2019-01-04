using System.Collections.Generic;

namespace Smartiks.Framework.Data.Abstractions
{
    public class Query<TQueryable>
    {
        public virtual QueryCriteria<TQueryable> Criteria { get; set; }

        public virtual List<QueryOrder> Orders { get; set; }

        public virtual QuerySegment Segment { get; set; }
    }
}