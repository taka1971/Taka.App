using Taka.Common.Middlewares.Enums;

namespace Taka.App.Rentals.Domain.Responses
{
    public record DelivererResponse(Guid Id, string Name, string Cnpj, string Cnh, CnhValidType cnhType, DateTime BirthDate); 
}
