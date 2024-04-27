using Refit;

namespace Taka.App.Deliverer.Application.Services.Apis
{
    public interface IRentalsApiService
    {
        [Get("/rentals/deliverer/{delivererId}")]
        Task<bool>  ExistRentals(Guid delivererId);
    }
}
