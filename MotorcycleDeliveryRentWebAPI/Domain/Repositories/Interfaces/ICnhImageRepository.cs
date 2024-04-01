using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces
{
    public interface ICnhImageRepository
    {
        public Task CreateAsync(CnhImageModel model);
    }
}
