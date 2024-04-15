namespace MotorcycleDeliveryRentWebAPI.Infra.Config.Interfaces
{
    public interface IMongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
