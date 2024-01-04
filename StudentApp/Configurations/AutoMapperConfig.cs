using AutoMapper;
using StudentApp.Data;
using StudentApp.Models;

namespace StudentApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<StudentDTO, Student>().ReverseMap();
        }
    }
}
