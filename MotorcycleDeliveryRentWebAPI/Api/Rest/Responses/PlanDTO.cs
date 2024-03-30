using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Responses
{
    public class PlanDTO
    {
        public string? Id { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int TotalDay { get; set; }
        public int PenaltyPercentage { get; set; }

        internal static List<PlanDTO> Convert(List<PlanModel> models)
        {
            List<PlanDTO> dtos = new List<PlanDTO>();

            foreach (var model in models)
            {
                PlanDTO dto = new PlanDTO
                {
                    Id = model.Id,
                    Price = model.Price,
                    Description = model.Description,
                    TotalDay = model.TotalDay,
                    PenaltyPercentage = model.PenaltyPercentage
                };

                dtos.Add(dto);
            }
            return dtos;
        }

        internal static PlanDTO Convert(PlanModel model)
        {
            PlanDTO dto = new PlanDTO();
            dto.Id = model.Id;
            dto.Price = model.Price;
            dto.Description = model.Description;
            dto.TotalDay = model.TotalDay;
            dto.PenaltyPercentage = model.PenaltyPercentage;
            return dto;
        }
    }
}
