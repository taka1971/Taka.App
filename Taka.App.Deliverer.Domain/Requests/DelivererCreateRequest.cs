using Taka.Common.Middlewares.Enums;

namespace Taka.App.Deliverer.Domain.Requests
{
    public record DelivererCreateRequest(string Name, string Cnpj, string Cnh, CnhValidType CnhType, DateOnly BirthDate, string CnhImage);
    
    public record DelivererUpdateRequest(Guid Id, string CnhImage);
}
