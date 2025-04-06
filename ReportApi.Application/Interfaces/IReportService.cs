using ReportApi.Application.Dtos;
using ReportApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportApi.Application.Interfaces
{
    public interface IReportService
    {
         Task<Report> RequestNewReportAsync();
        Task<List<Report>> GetAllReportsAsync();
        Task<ReportDto> GetReportDetailsAsync(Guid reportId);
        Task SendReportRequest(Guid reportId);
    }
}
