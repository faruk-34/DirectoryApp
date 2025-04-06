using DirectoryApi.Application.Dtos;
using DirectoryApi.Domain.Entities;
using Directory = DirectoryApi.Domain.Entities.Directory;


namespace DirectoryApi.Application.Interfaces
{
    public interface IContactInfoService
    {
        Task<Directory> Insert(ContactInfo request);
        Task<Directory> Delete(int id);
        Task<List<ContactInfoDto>> GetLocationReportDataAsync();
    }
}

