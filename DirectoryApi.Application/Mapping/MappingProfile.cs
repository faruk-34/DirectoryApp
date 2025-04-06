using Directory = DirectoryApi.Domain.Entities.Directory;
using AutoMapper;
using DirectoryApi.Application.Dtos;
using DirectoryApi.Domain.Entities;

namespace DirectoryApi.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
             CreateMap<Directory, DirectoryDto>().ReverseMap();
            CreateMap<ContactInfo, ContactInfoDto>().ReverseMap();
        }
    }
}

