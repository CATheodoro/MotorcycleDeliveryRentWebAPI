using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;
using RabbitMQ.Client;
using System.Text;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
        {

            _repository = notificationRepository;
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

        public void PublishNewDeliveryNotification(string deliveryId, string adminId, string driverId)
        {
            _channel.QueueDeclare(queue: "new_delivery_notifications",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            string message = $"New delivery created {deliveryId} by Admin: {adminId}, Driver: {driverId} was notified, {DateTime.UtcNow}";
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "new_delivery_notifications",
                                  basicProperties: null,
                                  body: body);
            _logger.LogInformation(message);

            NotificationModel model = new NotificationModel { Message = message, Timestamp = DateTime.UtcNow };
            _repository.Create(model);
        }

        public void PublishDeliveryAcceptance(string deliveryId, string adminId, string driverId)
        {
            _channel.QueueDeclare(queue: "delivery_acceptances",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            string message = $"Delivery {deliveryId}, create by admin: {adminId}, accepted by driver {driverId}, {DateTime.UtcNow}";
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "delivery_acceptances",
                                  basicProperties: null,
                                  body: body);
            _logger.LogInformation(message);

            NotificationModel model = new NotificationModel { Message = message, Timestamp = DateTime.UtcNow };
            _repository.Create(model);
        }


    }
}