using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taka.App.Deliverer.Domain.Enums;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Deliverer.Domain.Interfaces;
using Taka.App.Deliverer.Domain.Requests;
using Taka.Common;

namespace Taka.App.Deliverer.Api.Controllers
{
    [Authorize(Policy = "MustHaveMicroserviceAccess")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeliverersController : BaseController
    {
        private readonly IDelivererService _delivererService;
        public DeliverersController(IDelivererService delivererService)
        {
            _delivererService = delivererService;
        }

        /// <summary>
        /// Query all deliverers.
        /// </summary>
        /// <remarks>
        /// Use when you want to return all deliverers        
        /// </remarks>
        /// <response code="200">Success.</response>
        /// <response code="404">The query was executed, but found no data.</response>        
        /// 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deliverers = await _delivererService.GetAllAsync();

            return ApiResponse(deliverers);
        }

        /// <summary>
        /// Query a specific deliverer
        /// </summary>
        /// <remarks>
        /// Query a specific deliverer, using their id for search.
        /// </remarks>
        /// <response code="200">Success query deliverer.</response>
        /// <response code="404">The query was executed, but found no data.</response>
        /// <response code="400">Fail validation.</response>
        /// <response code="500">Internal server error.</response>
        /// 

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var deliverer = await _delivererService.GetByIdAsync(id);

            return ApiResponse(deliverer);

        }

        /// <summary>
        /// Create a deliverer
        /// </summary>
        /// <remarks>
        /// Register a new deliverer. Validations will be carried out, where the CNPJ and CNH must
        /// not be already registered for another deliverer.
        /// </remarks>
        /// <response code="200">Success register deliverer.</response>        
        /// <response code="400">Fail validation.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpPost]
        public async Task<IActionResult> CreateDeliverer([FromBody] DelivererCreateRequest request)
        {
            var createdDeliverer = await _delivererService.AddAsync(request);
            return ApiResponse(createdDeliverer);
        }

        /// <summary>
        /// Update a specific deliverer
        /// </summary>
        /// <remarks>
        /// Update a specific deliverer.
        /// </remarks>
        /// <response code="204">Success update deliverer.</response>        
        /// <response code="400">Fail validation.</response>
        /// <response code="500">Internal server error.</response>
        /// 

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DelivererUpdateRequest motorcycleRequest)
        {
            await _delivererService.UpdateAsync(motorcycleRequest);
            return NoContent();
        }

        /// <summary>
        /// Delete a specific deliverer
        /// </summary>
        /// <remarks>
        /// Update a specific deliverer.
        /// </remarks>
        /// <response code="204">Success update deliverer.</response>        
        /// <response code="400">Fail validation.</response>
        /// <response code="500">Internal server error.</response>
        /// 

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _delivererService.DeleteAsync(id);
            return NoContent();
        }
    }
}
