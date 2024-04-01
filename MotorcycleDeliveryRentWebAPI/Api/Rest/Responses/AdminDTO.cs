using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Responses
{
    public class AdminDTO
    {
        public string? Id { get; set; }
        public string? Email { get; set; }

        internal static async Task<List<AdminDTO>> Convert(Task<List<AdminModel>> task)
        {
            List<AdminModel> models = await task;
            List<AdminDTO> dtos = new List<AdminDTO>();

            foreach (var model in models)
            {
                AdminDTO dto = new AdminDTO
                {
                    Id = model.Id,
                    Email = model.Email
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        internal static async Task<AdminDTO> Convert(AdminModel model)
        {
            AdminDTO dto = new AdminDTO();
            dto.Id = model.Id;
            dto.Email = model.Email;
            return dto;
        }
    }
}
