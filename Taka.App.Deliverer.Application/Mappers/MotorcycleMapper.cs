using Taka.App.Deliverer.Domain.Entities;
using Taka.App.Deliverer.Domain.Requests;
using Taka.App.Deliverer.Domain.Responses;

namespace Taka.App.Deliverer.Application.Mappers
{
    public static class DelivererMapper
    {
        public static DelivererResponse EntityToDto(PersonDelivery deliverer)
        {
            return new DelivererResponse(deliverer.Id, deliverer.Name, deliverer.CNPJ, deliverer.CNHNumber, deliverer.CNHType, deliverer.BirthDate) ;            
        }        

        public static PersonDelivery DtoToEntity(DelivererCreateRequest delivererRequest)
        {
            return new PersonDelivery
            {
                Name = delivererRequest.Name,
                CNPJ = delivererRequest.Cnpj,
                CNHNumber = delivererRequest.Cnh,
                CNHType = delivererRequest.cnhType,
                BirthDate = delivererRequest.BirthDate
            };
        }        
    }

}
