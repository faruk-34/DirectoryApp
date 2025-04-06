using Microsoft.AspNetCore.Mvc;
using ReportApi.Application.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly HttpClient _httpClient;

        public ReportController(IReportService reportService, HttpClient httpClient)
        {
            _reportService = reportService;
            _httpClient = httpClient;
        }


        [HttpPost("request-report")]
        public async Task<IActionResult> RequestReport()
        {
            var report = await _reportService.RequestNewReportAsync();
            return Ok(new { reportId = report.Id, status = report.Status });
        }

        [HttpGet("list-reports")]
        public async Task<IActionResult> ListReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }


        [HttpGet("report/{id}")]
        public async Task<IActionResult> GetReportDetails(Guid id)
        {
            var reportData = await _reportService.GetReportDetailsAsync(id);

            if (reportData == null)
                return NotFound("Rapor bulunamadı, dosya mevcut değil veya henüz tamamlanmamış olabilir.");

            return Ok(reportData);
        }

 
    }
}
