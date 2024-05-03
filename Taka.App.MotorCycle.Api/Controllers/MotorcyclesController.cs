using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Taka.App.Motor.Domain.Enums;
using Taka.App.Motor.Domain.Exceptions;
using Taka.App.Motor.Domain.Interfaces;
using Taka.App.Motor.Domain.Request;
using Taka.App.Motor.Domain.Responses;
using Taka.Common.Infrastructure;

namespace Taka.App.Motor.Api.Controllers
{
    [Authorize(Policy = "MustHaveMicroserviceAccess")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MotorcyclesController : ControllerBase
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
            try
            {
                var motorcycles = await _motorcycleService.GetAllAsync();
                return motorcycles.Any() ? Ok(motorcycles) : NotFound("Not found any motorcycles.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var motorcycle = await _motorcycleService.GetByIdAsync(id);

                return Ok(motorcycle);
            }
            catch (DomainException ex)
            {
                return ex.ErrorCode == DomainErrorCode.MotorcycleNotFound
                                     ? NotFound(ex.Message) : StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                var motorcycle = await _motorcycleService.GetByPlateAsync(plate);
                
                return Ok(motorcycle);
            }
            catch (DomainException ex)
            {
                return ex.ErrorCode == DomainErrorCode.MotorcycleNotFound
                                     ? NotFound(ex.Message) : StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
            {
                await _motorcycleService.AddAsync(motorcycleRequest);

                return Ok();
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
            try
            {
                await _motorcycleService.UpdateAsync(motorcycleRequest);                

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
            try
            {
                await _motorcycleService.DeleteAsync(id);

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
