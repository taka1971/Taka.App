using Taka.App.Motor.Domain.Entitites;
using Taka.App.Motor.Domain.Request;
using Taka.App.Motor.Domain.Responses;

namespace Taka.App.Motor.Application.Mappers
{
    public static class MotorcycleMapper
    {
        public static MotorcycleResponse EntityToDto(Motorcycle motorcycle)
        {
            return new MotorcycleResponse(motorcycle.MotorcycleId, motorcycle.Year, motorcycle.Model, motorcycle.Plate);            
        }        

        public static Motorcycle DtoToEntity(MotorcycleCreateRequest motorcycle)
        {
            return new Motorcycle
            {
                Year = motorcycle.Year,
                Model = motorcycle.Model,
                Plate = motorcycle.Plate                
            };
        }

    }

}
