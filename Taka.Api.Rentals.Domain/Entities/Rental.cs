namespace Taka.App.Rentals.Domain.Entities
{
    public class Rental
    {
        public Guid RentalId { get; set; } // Identificador único para o aluguel
        public Guid RentalPlanId { get; set; } // Chave estrangeira para o plano de aluguel
        public RentalPlan RentalPlan { get; set; } = new RentalPlan();
        public Guid DelivererId { get; set; } // Identificador do entregador
        public Guid MotorcycleId { get; set; } // Identificador da moto alugada
        public DateTime StartDate { get; set; } // Data de início da locação
        public DateTime? EndDate { get; set; } // Data de término da locação
        public DateTime PredictedEndDate { get; set; } // Data prevista para o término        
    }
}
