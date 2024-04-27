namespace Taka.App.Motor.Domain.Request
{
    public record MotorcycleCreateRequest(int Year, string Model, string Plate)
    {
        public string Plate { get; private set; } = Plate.ToUpper();
    }
        
    public record MotorcycleUpdateRequest(Guid Id, string Plate);    
    
}
