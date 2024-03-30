namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface INotificationService
    {
        public void PublishNewDeliveryNotification(string deliveryId);
        public void PublishDeliveryAcceptance(string deliveryId, string driverId);
    }
}
