using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        public void Create(NotificationModel model);
    }
}
