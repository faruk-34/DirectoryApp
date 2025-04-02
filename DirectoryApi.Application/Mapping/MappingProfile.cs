 
using AutoMapper;
using DirectoryApi.Application.Dtos;
 
namespace DirectoryApi.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<DirectoryDto, DirectoryInfo>();
        }
    }
}

