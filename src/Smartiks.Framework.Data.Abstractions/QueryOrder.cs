using System.Linq.Expressions;

namespace Smartiks.Framework.Data.Abstractions
{
    public class QueryOrder
    {
        public virtual Expression Expression { get; set; }

        public virtual bool IsDescending { get; set; }
    }
}
