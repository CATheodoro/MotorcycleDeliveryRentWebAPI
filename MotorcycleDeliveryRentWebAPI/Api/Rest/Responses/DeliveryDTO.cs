using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Responses
{
    public class DeliveryDTO
    {
        public string? Id { get; set; }
        public string? AdminId { get; set; }
        public string? DriverId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime AcceptDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal Price { get; set; }
        public DeliveryStatusEnum Status { get; set; }



        internal static async Task<List<DeliveryDTO>> Convert(Task<List<DeliveryModel>> task)
        {
            List<DeliveryModel> models = await task;
            List<DeliveryDTO> dtos = new List<DeliveryDTO>();

            foreach (var model in models)
            {
                DeliveryDTO dto = new DeliveryDTO
                {
                    Id = model.Id,
                    AdminId = model.AdminId,
                    DriverId = model.DriverId,
                    CreationDate = model.CreationDate,
                    AcceptDate = model.AcceptDate,
                    DeliveryDate = model.DeliveryDate,
                    Price = model.Price,
                    Status = model.Status
                };

                dtos.Add(dto);
            }
            return dtos;
        }

        internal static async Task<DeliveryDTO> Convert(DeliveryModel model)
        {
            DeliveryDTO dto = new DeliveryDTO();
            dto.Id = model.Id;
            dto.AdminId = model.AdminId;
            dto.DriverId = model.DriverId;
            dto.CreationDate = model.CreationDate;
            dto.AcceptDate = model.AcceptDate;
            dto.DeliveryDate = model.DeliveryDate;
            dto.Price = model.Price;
            dto.Status = model.Status;
            return dto;
        }
    }
}
