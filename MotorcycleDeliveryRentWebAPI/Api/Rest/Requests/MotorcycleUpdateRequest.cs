using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class MotorcycleUpdateRequest
    {
        public required int Year { get; set; }
        public required string Model { get; set; }

        internal static MotorcycleModel Convert(MotorcycleModel model, MotorcycleUpdateRequest motorcycleRequest)
        {
            model.Year = motorcycleRequest.Year;
            model.Model = motorcycleRequest.Model;
            return model;
        }
    }
}
