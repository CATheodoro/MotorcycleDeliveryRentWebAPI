using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Responses
{
    public class MotorcycleDTO
    {
        public string? Id { get; set; }
        public int? Year { get; set; }
        public string? Model { get; set; }
        public string? Plate { get; set; }
        public MotorcycleStatusEnum Status { get; set; }

        internal static async Task<List<MotorcycleDTO>> Convert(Task<List<MotorcycleModel>> task)
        {
            List<MotorcycleModel> models = await task;
            List<MotorcycleDTO> dtos = new List<MotorcycleDTO>();

            foreach (var model in models)
            {
                MotorcycleDTO dto = new MotorcycleDTO
                {
                    Id = model.Id,
                    Year = model.Year,
                    Model = model.Model,
                    Plate = model.Plate,
                    Status = model.Status
                };

                dtos.Add(dto);
            }
            return dtos;
        }

        internal static async Task<MotorcycleDTO> Convert(MotorcycleModel model)
        {
            MotorcycleDTO dto = new MotorcycleDTO();
            dto.Id = model.Id;
            dto.Year = model.Year;
            dto.Model = model.Model;
            dto.Plate = model.Plate;
            dto.Status = model.Status;
            return dto;
        }
    }
}
