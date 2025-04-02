using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DirectoryApi.Domain.Entities;
using Directory = DirectoryApi.Domain.Entities.Directory;

namespace DirectoryApi.Infrastructure.Context
{
    public class DirectoryDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DirectoryDbContext(DbContextOptions<DirectoryDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
            }
        }

        public DbSet<Directory> Directories { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
    }
}
