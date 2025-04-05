using RabbitMQ.Client;
using ReportApi.Application.Interfaces;
using System.Text;
using System.Text.Json;


namespace ReportApi.Application.Services
{
    public class ReportProducerService : IReportConsumerService
    {
        private readonly ConnectionFactory _factory;

        public ReportProducerService()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" }; // RabbitMQ Sunucu Adresi
        }

        public async Task SendReportRequest(Guid reportId)
        {
            var json = JsonSerializer.Serialize(reportId);
            var body = Encoding.UTF8.GetBytes(json);

            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync(); // async versiyon!

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
