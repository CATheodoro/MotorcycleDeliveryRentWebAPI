using MotorcycleDeliveryRentWebAPI.Infra.Config.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Infra.Config
{
    public class RabbitMQSettings : IRabbitMQSerttings
    {
        public string HostName { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
