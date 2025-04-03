using DirectoryApi.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Directory = DirectoryApi.Domain.Entities.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryApi.Application.Interfaces
{
    public interface IDirectoryService
    {
        Task<List<Directory>> Get();
        Task<DirectoryDto> Insert(DirectoryDto request);
        Task<DirectoryDto> Update(DirectoryDto request);
        Task<bool> Delete(int id);
    }
}
