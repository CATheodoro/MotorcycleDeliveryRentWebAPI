using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class RentRequest
    {
        public string? DriverId { get; set; }
        public string? MotorcycleId { get; set; }
        public string? PlanId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public decimal? TotalPrice { get; set; }

        internal static RentModel Convert(string driverId, string motorcycleId, string planId, DateOnly startDate, DateOnly endDate)
        {
            RentModel model = new RentModel();
            model.DriverId = driverId;
            model.MotorcycleId = motorcycleId;
            model.PlanId = planId;
            model.StartDate = startDate;
            model.EndDate = endDate;
            return model;
        }

        internal static RentModel ConvertUpdate(RentModel model, DateOnly returnDate, decimal totalPrice)
        {
            model.ReturnDate = returnDate;
            model.TotalPrice = totalPrice;
            return model;
        }
    }
}
