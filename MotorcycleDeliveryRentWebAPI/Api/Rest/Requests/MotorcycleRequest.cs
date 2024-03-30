using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class MotorcycleRequest
    {
        public required int Year { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }

        internal static MotorcycleModel Convert(MotorcycleRequest motorcycleRequest)
        {
            MotorcycleModel model = new MotorcycleModel();
            model.Year = motorcycleRequest.Year;
            model.Model = motorcycleRequest.Model;
            model.Plate = motorcycleRequest.Plate;
            model.Status = MotorcycleStatusEnum.Available;
            return model;
        }
    }
}
