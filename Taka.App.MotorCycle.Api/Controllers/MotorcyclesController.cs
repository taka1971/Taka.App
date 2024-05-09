using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taka.App.Motor.Domain.Interfaces;
using Taka.App.Motor.Domain.Request;
using Taka.Common;
using Taka.Common.Infrastructure;

namespace Taka.App.Motor.Api.Controllers
{
    [Authorize(Policy = "MustHaveMicroserviceAccess")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MotorcyclesController : BaseController
    {
        private readonly IMotorcycleService _motorcycleService;
        private readonly ResilienceEngine _resilienceEngine;

        public MotorcyclesController(IMotorcycleService motorcycleService, ResilienceEngine resilienceEngine)
        {
            _motorcycleService = motorcycleService;
            _resilienceEngine = resilienceEngine;
        }

        /// <summary>
        /// Query all Motorcycles
        /// </summary>
        /// <remarks>
        /// Query all Motorcycles
        /// </remarks>
        /// <response code="200">Success query all motorcycles.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var motorcycles = await _motorcycleService.GetAllAsync();
            return ApiResponse(motorcycles);
        }

        /// <summary>
        /// Query a specific motorcycles by id
        /// </summary>
        /// <remarks>
        /// Query a specific motorcycles by id
        /// </remarks>
        /// <response code="200">Success query a motorcycle.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var motorcycle = await _motorcycleService.GetByIdAsync(id);
            return ApiResponse(motorcycle);
        }

        /// <summary>
        /// Query a specific motorcycles by plate
        /// </summary>
        /// <remarks>
        /// Query a specific motorcycles by plate
        /// </remarks>
        /// <response code="200">Success query a motorcycle.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpGet("plate/{plate}")]
        public async Task<IActionResult> GetByPlate(string plate)
        {
            var motorcycle = await _motorcycleService.GetByPlateAsync(plate);
            return ApiResponse(motorcycle);
        }

        /// <summary>
        /// Create a motorcycle
        /// </summary>
        /// <remarks>
        /// Create a motorcycle
        /// </remarks>
        /// <response code="201">Success create a motorcycle.</response>                
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MotorcycleCreateRequest motorcycleRequest)
        {
            await _motorcycleService.AddAsync(motorcycleRequest);
            var message = "Motorcycle registration received and is being processed.";

            return ApiResponse(message);
        }

        /// <summary>
        /// Update a specific motorcycle
        /// </summary>
        /// <remarks>
        /// Update a specific motorcycle
        /// </remarks>
        /// <response code="202">Success update a motorcycle.</response>                
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MotorcycleUpdateRequest motorcycleRequest)
        {
            await _motorcycleService.UpdateAsync(motorcycleRequest);
            return NoContent();
        }

        /// <summary>
        /// Delete a specific motorcycle
        /// </summary>
        /// <remarks>
        /// Delete a specific motorcycle
        /// </remarks>
        /// <response code="202">Success delete a motorcycle.</response>                
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        ///
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _motorcycleService.DeleteAsync(id);
            return NoContent();
        }
    }
}
