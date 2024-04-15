namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface INotificationService
    {
        public void PublishNewDeliveryNotification(string deliveryId, string adminId, string driverId);
        public void PublishDeliveryAcceptance(string deliveryId, string adminId, string driverId);
    }
}
