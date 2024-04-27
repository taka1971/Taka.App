using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taka.App.Deliverer.Domain.Enums;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Deliverer.Domain.Interfaces;
using Taka.App.Deliverer.Domain.Requests;

namespace Taka.App.Deliverer.Api.Controllers
{
    [Authorize(Policy = "MustHaveMicroserviceAccess")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeliverersController : ControllerBase
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
        /// <response code="404">The query was successful, returning the data.</response>        
        /// 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deliverers = await _delivererService.GetAllAsync();
            return deliverers.Any() ? Ok(deliverers) : NotFound("There are still no records of registered deliverers.");
        }

        /// <summary>
        /// Query a specific deliverer
        /// </summary>
        /// <remarks>
        /// Query a specific deliverer, using their id for search.
        /// </remarks>
        /// <response code="200">Success query deliverer.</response>
        /// <response code="404">The query was successful, returning the data.</response>
        /// <response code="400">Fail validation.</response>
        /// <response code="500">Internal server error.</response>
        /// 

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var deliverer = await _delivererService.GetByIdAsync(id);

                return Ok(deliverer);
            }
            catch (DomainException ex)
            {
                return ex.ErrorCode == DomainErrorCode.DelivererNotFound
                                     ? NotFound(ex.Message) : StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        public async Task<IActionResult> CreateDeliverer([FromForm] DelivererCreateRequest request)
        {
            try
            {
                var createdDeliverer = await _delivererService.AddAsync(request);
                return Ok(createdDeliverer);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
            try
            {
                await _delivererService.UpdateAsync(motorcycleRequest);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
            try
            {
                await _delivererService.DeleteAsync(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
