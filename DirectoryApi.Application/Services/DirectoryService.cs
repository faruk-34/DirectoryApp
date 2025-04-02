using AutoMapper;
using DirectoryApi.Application.Dtos;
using DirectoryApi.Application.Interfaces;
using DirectoryApi.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Directory = DirectoryApi.Domain.Entities.Directory;

namespace DirectoryApi.Application.Services
{
    public class DirectoryService : IDirectoryService
    {
        private readonly DirectoryDbContext _context;
        private readonly IMapper _mapper;
        public DirectoryService(DirectoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Directory>> Get()
        {

            List<Directory> directorys = await _context.Directories.Include(p => p.ContactInfos).ToListAsync();

            return directorys;
        }


        [HttpPost]
        public async Task<DirectoryDto> Insert(DirectoryDto request)
        {
            Directory directory = _mapper.Map<Directory>(request);
            await _context.Directories.AddAsync(directory);
            await _context.SaveChangesAsync();
            return _mapper.Map<DirectoryDto>(directory);
        }

        [HttpPut]
        public async Task<DirectoryDto> Update(DirectoryDto request)
        {
             Directory directory = await _context.Directories.FindAsync(request.Id);
            directory = _mapper.Map(request, directory);

            return _mapper.Map<DirectoryDto>(directory);
        }

        public async Task<bool> Delete(int id)
        {
            Directory directory = await _context.Directories.FindAsync(id);
            _context.Directories.Remove(directory);
            await _context.SaveChangesAsync();
            return true;

        }
    }

}
