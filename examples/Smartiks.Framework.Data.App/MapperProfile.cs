using AutoMapper;
using Smartiks.Framework.Data.App.Model;

namespace Smartiks.Framework.Data.App
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Employee, Employee>()
                .ReverseMap();
        }
    }
}