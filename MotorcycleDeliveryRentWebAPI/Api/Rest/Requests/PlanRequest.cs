using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class PlanRequest
    {
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required int TotalDay { get; set; }
        public required int PenaltyPercentage { get; set; }

        internal static PlanModel Convert(PlanRequest request)
        {
            PlanModel model = new PlanModel();
            model.Price = request.Price;
            model.Description = request.Description;
            model.TotalDay = request.TotalDay;
            model.PenaltyPercentage = request.PenaltyPercentage;
            return model;
        }

        internal static PlanModel ConvertUpdate(PlanModel model, PlanRequest request)
        {
            model.Price = request.Price;
            model.Description = request.Description;
            model.TotalDay = request.TotalDay;
            model.PenaltyPercentage = request.PenaltyPercentage;
            return model;
        }
    }
}
