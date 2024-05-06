using Taka.App.Deliverer.Application.Mappers;
using Taka.App.Deliverer.Application.Services.Apis;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Deliverer.Domain.Interfaces;
using Taka.App.Deliverer.Domain.Requests;
using Taka.App.Deliverer.Domain.Responses;

namespace Taka.App.Deliverer.Application.Services
{
    public class DelivererService : IDelivererService
    {
        private readonly IDelivererRepository _delivererRepository;
        private readonly IStorageService _storageService;
        private readonly IRentalsApiService _rentalsApiService;

        public DelivererService(IDelivererRepository delivererRepository, IStorageService storageService, IRentalsApiService rentalsApiService)
        {
            _delivererRepository = delivererRepository;
            _storageService = storageService;
            _rentalsApiService = rentalsApiService;
        }

        public async Task<DelivererResponse> AddAsync(DelivererCreateRequest request)
        {
            var deliverer = DelivererMapper.DtoToEntity(request);

            if (!string.IsNullOrEmpty(request.CnhImage))
            {
                byte[] imageBytes = Convert.FromBase64String(request.CnhImage);

                deliverer.CNHImageUrl = await UploadFileAsync(imageBytes, request.Cnh);
            }

            await _delivererRepository.AddAsync(deliverer);
            return DelivererMapper.EntityToDto(deliverer);
        }

        public async Task DeleteAsync(Guid id)
        {
            var deliverer = await _delivererRepository.GetByIdAsync(id);
            if (deliverer != null)
            {
                var existRentals = await _rentalsApiService.ExistRentals(id);
                if (existRentals)
                    throw new AppException("It is not possible to delete this delivery person. There is one or more rentals made by him.");

                await _delivererRepository.DeleteAsync(deliverer);
            }
        }

        public async Task<IEnumerable<DelivererResponse>> GetAllAsync()
        {
            var deliverers = await _delivererRepository.GetAllAsync();

            var deliverersResponses = deliverers.Select(m => new DelivererResponse
            (m.Id, m.Name, m.CNPJ, m.CNHNumber, m.CNHType, m.BirthDate)
            ).ToList();

            return deliverersResponses;
        }

        public async Task<DelivererResponse> GetByIdAsync(Guid id)
        {
            var deliverer = await _delivererRepository.GetByIdAsync(id);

            var deliveryResponse = new DelivererResponse
            (deliverer.Id, deliverer.Name, deliverer.CNPJ, deliverer.CNHNumber, deliverer.CNHType, deliverer.BirthDate);

            return deliveryResponse;
        }

        public async Task UpdateAsync(DelivererUpdateRequest delivererRequest)
        {
            var delivererResult = await _delivererRepository.GetByIdAsync(delivererRequest.Id);

            if (delivererResult != null)
            {
                byte[] imageBytes = Convert.FromBase64String(delivererRequest.CnhImage);
                delivererResult.CNHImageUrl = await UploadFileAsync(imageBytes, delivererResult.CNHNumber);
                await _delivererRepository.UpdateAsync(delivererResult);
            }
        }

        private async Task<string> UploadFileAsync(byte[] imageData, string cnhNumber)
        {
            var fileName = $"cnh_{cnhNumber}.png";
            return await _storageService.UploadFileAsync(imageData, fileName);
        }
    }
}
