using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReportApi.Domain.Entities;
 
namespace ReportApi.Infrastructure.Context
{
    public class ReportDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ReportDbContext(DbContextOptions<ReportDbContext> options, IConfiguration configuration)
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
 
        public DbSet<Report> Reports { get; set; }
    }
}
