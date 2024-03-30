using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class DriverUpdateRequest
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Cnpj { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Cnh { get; set; }
        public CnhTypeEnum CnhType { get; set; }

        internal static DriverModel Convert(DriverModel model, DriverUpdateRequest request)
        {
            if (request.Email != null)
                model.Email = request.Email;
            if (request.Name != null)
                model.Name = request.Name;
            if (request.Cnpj != null)
                model.Cnpj = request.Cnpj;
            model.BirthDate = request.BirthDate;
            if (request.Cnh != null)
                model.Cnh = request.Cnh;
            return model;
        }
    }
}
