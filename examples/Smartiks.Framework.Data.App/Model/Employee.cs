using Smartiks.Framework.Data.Abstractions;

namespace Smartiks.Framework.Data.App.Model
{
    public class Employee : IId<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
