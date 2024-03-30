namespace MotorcycleDeliveryRentWebAPI.Infra.Config
{
    public interface IMongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
