using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportApi.Application.Dtos;
using ReportApi.Application.Interfaces;
using ReportApi.Domain.Entities;
using ReportApi.Infrastructure.Context;
using System.Text;
using System.Text.Json;


namespace ReportApi.Application.Services
{
    public class ReportService : IReportService 
    {
        private readonly ReportDbContext _context;
        private readonly ConnectionFactory _factory;
        private readonly IServiceScopeFactory _scopeFactory;


        public ReportService(ReportDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _scopeFactory = scopeFactory;
            _factory = new ConnectionFactory() { HostName = "localhost" }; // RabbitMQ Sunucu Adresi
        }
 
        public async Task<Report> RequestNewReportAsync()
        {
            var report = new Report();
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            await SendReportRequest(report.Id);
            return report;
        }

        public async Task<List<Report>> GetAllReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<ReportDto> GetReportDetailsAsync(Guid reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null || string.IsNullOrWhiteSpace(report.FilePath))
                return null;

            if (!System.IO.File.Exists(report.FilePath))
                return null;

            var reportJson = await System.IO.File.ReadAllTextAsync(report.FilePath);
            var reportData = JsonSerializer.Deserialize<ReportDto>(reportJson);

            return reportData;
        }


        public async Task SendReportRequest(Guid reportId)
        {
            var json = JsonSerializer.Serialize(reportId);
            var body = Encoding.UTF8.GetBytes(json);

            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            //Kuyruk yoksa oluşturur!
            await channel.QueueDeclareAsync(
                queue: "report-requests",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            //Kuyruğa mesajı gönder!
            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "report-requests",
                mandatory: false,
                body: body
            );
        }

 
    }

 
}
