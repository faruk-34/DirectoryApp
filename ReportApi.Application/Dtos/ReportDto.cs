using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportApi.Application.Dtos
{
    public class ReportDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Beklemede";
        public string? FilePath { get; set; }
    }
}
