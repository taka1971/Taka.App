using Taka.Common.Middlewares.Enums;

namespace Taka.App.Deliverer.Domain.Responses
{
    public record DelivererResponse(Guid Id, string Name, string Cnpj, string Cnh, CnhValidType cnhType, DateOnly BirthDate);
}
