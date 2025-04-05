using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportApi.Domain.Entities
{
   public class Report
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Beklemede";
        public string? FilePath { get; set; }
    }
}
