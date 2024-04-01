using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Responses
{
    public class RentDTO
    {
        public string? Id { get; set; }
        public string? DriverId { get; set; }
        public string? MotorcycleId { get; set; }
        public string? PlanId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }

        internal static async Task<List<RentDTO>> Convert(Task<List<RentModel>> task)
        {
            List<RentModel> models = await task;
            List<RentDTO> dtos = new List<RentDTO>();

            foreach (var model in models)
            {
                RentDTO dto = new RentDTO
                {
                    Id = model.Id,
                    DriverId = model.DriverId,
                    MotorcycleId = model.MotorcycleId,
                    PlanId = model.PlanId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ReturnDate = model.ReturnDate,
                    TotalPrice = model.TotalPrice
                };

                dtos.Add(dto);
            }
            return dtos;
        }

        internal static async Task<RentDTO> Convert(RentModel model)
        {
            RentDTO dto = new RentDTO();
            dto.Id = model.Id;
            dto.DriverId = model.DriverId;
            dto.MotorcycleId = model.MotorcycleId;
            dto.PlanId = model.PlanId;
            dto.StartDate = model.StartDate;
            dto.EndDate = model.EndDate;
            dto.ReturnDate = model.ReturnDate;
            dto.TotalPrice = model.TotalPrice;
            return dto;
        }
    }
}
