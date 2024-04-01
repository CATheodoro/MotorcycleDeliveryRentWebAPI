using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Responses
{
    public class DriverDTO
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Cnpj { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Cnh { get; set; }
        public CnhTypeEnum CnhType { get; set; }
        public DriverStatusEnum Status { get; set; }

        internal static async Task<List<DriverDTO>> Convert(Task<List<DriverModel>> task)
        {
            List<DriverModel> models = await task;
            List<DriverDTO> dtos = new List<DriverDTO>();

            foreach (var model in models)
            {
                DriverDTO dto = new DriverDTO
                {
                    Id = model.Id,
                    Email = model.Email,
                    Name = model.Name,
                    Cnpj = model.Cnpj,
                    BirthDate = model.BirthDate,
                    Cnh = model.Cnh,
                    CnhType = model.CnhType,
                    Status = model.Status
                };

                dtos.Add(dto);
            }
            return dtos;
        }

        internal static async Task<DriverDTO> Convert(DriverModel model)
        {
            DriverDTO dto = new DriverDTO();
            dto.Id = model.Id;
            dto.Email = model.Email;
            dto.Name = model.Name;
            dto.Cnpj = model.Cnpj;
            dto.BirthDate = model.BirthDate;
            dto.Cnh = model.Cnh;
            dto.CnhType = model.CnhType;
            dto.Status = model.Status;
            return dto;
        }
    }
}
