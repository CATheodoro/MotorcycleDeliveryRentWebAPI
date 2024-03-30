using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;
using RabbitMQ.Client;
using System.Text;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<NotificationModel> _logger;

        private readonly IMongoCollection<NotificationModel> _context;

        public NotificationService(IMongoDBSettings settings, IMongoClient mongoClient, ILogger<NotificationModel> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<NotificationModel>("Notification");
            _logger = logger;

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            var rabbitMQSettings = configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMQSettings.HostName,
                Port = rabbitMQSettings.Port,
                UserName = rabbitMQSettings.UserName,
                Password = rabbitMQSettings.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void PublishNewDeliveryNotification(string deliveryId)
        {
            _channel.QueueDeclare(queue: "new_delivery_notifications",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            string message = $"New delivery created: {deliveryId}";
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "new_delivery_notifications",
                                  basicProperties: null,
                                  body: body);
            _logger.LogInformation($"New delivery created: {deliveryId}, {DateTime.UtcNow}");
            _context.InsertOne(new NotificationModel { Message = $"New delivery created: {deliveryId}", Timestamp = DateTime.UtcNow });
        }

        public void PublishDeliveryAcceptance(string deliveryId, string driverId)
        {
            _channel.QueueDeclare(queue: "delivery_acceptances",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            string message = $"Delivery {deliveryId} accepted by driver {driverId}";
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "delivery_acceptances",
                                  basicProperties: null,
                                  body: body);
            _logger.LogInformation($"Delivery {deliveryId} accepted by driver {driverId}, {DateTime.UtcNow}");
            _context.InsertOne(new NotificationModel { Message = $"Delivery {deliveryId} accepted by driver {driverId}", Timestamp = DateTime.UtcNow });
        }


    }
}