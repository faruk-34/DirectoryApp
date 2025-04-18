﻿using DirectoryApi.Application.Dtos;
using DirectoryApi.Application.Interfaces;
using DirectoryApi.Domain.Entities;
using DirectoryApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Directory = DirectoryApi.Domain.Entities.Directory;


namespace DirectoryApi.Application.Services
{
    public class ContactInfoService : IContactInfoService
    {
        private readonly DirectoryDbContext _context;
        public ContactInfoService(DirectoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContactInfoDto>> GetLocationReportDataAsync()
        {
            var result = await _context.ContactInfos
          .Where(c => !string.IsNullOrEmpty(c.Location) && !string.IsNullOrEmpty(c.Phone))
          .GroupBy(c => c.Location)
          .Select(group => new ContactInfoDto
          {
              Location = group.Key,
              PersonCount = group.Count()
          })
          .OrderBy(dto => dto.PersonCount)
          .ToListAsync();

            return result;
        }
        public async Task<Directory> Insert(ContactInfo request)
        {
            await _context.ContactInfos.AddAsync(request);
            await _context.SaveChangesAsync();

            Directory directory = await _context.Directorys.Include(p => p.ContactInfos).FirstAsync(p => p.Id == request.DirectoryId);

            return directory;
        }

        public async Task<Directory> Delete(int id)
        {
            ContactInfo contact = await _context.ContactInfos.FindAsync(id);
            _context.ContactInfos.Remove(contact);
            await _context.SaveChangesAsync();

            Directory directory = await _context.Directorys.Include(p => p.ContactInfos).FirstAsync(p => p.Id == contact.DirectoryId);

            return directory;

        }
    }
}
