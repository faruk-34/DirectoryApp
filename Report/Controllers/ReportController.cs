using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportApi.Application.Interfaces;
using ReportApi.Infrastructure.Context;
using System.Text.Json;


namespace Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportDbContext _context;
        private readonly IReportConsumerService _reportConsumerService;

        public ReportController(ReportDbContext context, IReportConsumerService reportConsumerService)
        {
            _context = context;
            _reportConsumerService = reportConsumerService;
        }
 
        [HttpGet] // Rapor da  istenen bilgiler 
        public async Task<IActionResult> Get()
        {
            var konumlar = _context.ContactInfos.GroupBy(p => p.Location).Select(s => s.Key).ToList();
            var result = from konum in konumlar
                         select new
                         {
                             Konum = konum,
                             KisiSayisi = _context.ContactInfos.Where(p => p.Location == konum && p.Phone != "").Count(),

                         };
            return Ok(result.OrderBy(p => p.KisiSayisi).ToList());
        }


 
        [HttpPost("request-report")]
        public async Task<IActionResult> RequestReport()
        {
            var report = new ReportApi.Domain.Entities.Report();
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            _reportConsumerService.SendReportRequest(report.Id);

            return Ok(new { reportId = report.Id, status = report.Status });
        }

        [HttpGet("list-reports")]
        public async Task<IActionResult> ListReports()
        {
            var reports = await _context.Reports.ToListAsync();
            return Ok(reports);
        }

        [HttpGet("report/{id}")]
        public async Task<IActionResult> GetReportDetails(Guid id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null || report.FilePath == null)
                return NotFound("Rapor bulunamadı veya henüz tamamlanmadı.");

            var reportData = await System.IO.File.ReadAllTextAsync(report.FilePath);
            return Ok(JsonSerializer.Deserialize<object>(reportData));
        }
    }
}
