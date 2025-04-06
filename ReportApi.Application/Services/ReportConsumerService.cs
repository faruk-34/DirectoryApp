using DirectoryApi.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportApi.Infrastructure.Context;
using System.Text;
using System.Text.Json;

namespace ReportApi.Application.Services
{
    public class ReportConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConnectionFactory _factory;
        private readonly HttpClient _httpClient;

        public ReportConsumerService(IServiceScopeFactory scopeFactory, HttpClient httpClient   )
        {
            _scopeFactory = scopeFactory;
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _httpClient = httpClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync(); // async versiyon!

            await channel.QueueDeclareAsync(queue: "reportQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                using var scope = _scopeFactory.CreateScope();
                var _context = scope.ServiceProvider.GetRequiredService<ReportDbContext>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var request = JsonSerializer.Deserialize<ReportRequest>(message);

                if (request != null)
                {
                    var report = await _context.Reports.FindAsync(request.ReportId);
                    if (report != null)
                    {
                        report.Status = "Hazırlanıyor";
                        await _context.SaveChangesAsync(); 

                        var apiUrl = $"http://localhost:7003/api/ContactInfo/GetLocationReport";

                        var response = await _httpClient.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                          
                            string filePath = $"reports/{report.Id}.json";
                            System.IO.Directory.CreateDirectory("reports");
                            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(content));

                            report.Status = "Tamamlandı";
                            report.FilePath = filePath;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await channel.BasicConsumeAsync(queue: "reportQueue", autoAck: false, consumer: consumer);

            await Task.Delay(-1, stoppingToken);
        }
    }

    public class ReportRequest
    {
        public Guid ReportId { get; set; }
    }
}

