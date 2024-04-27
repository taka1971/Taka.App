using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Rentals.Domain.Interfaces;
using Taka.App.Rentals.Domain.Requests;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Taka.App.Rentals.Api.Controllers
{
    [Authorize(Policy = "MustHaveMicroserviceAccess")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RentalPlansController : ControllerBase
    {
        private readonly IRentalPlanService _rentalPlanService;
        public RentalPlansController(IRentalPlanService rentalPlanService)
        {
            _rentalPlanService = rentalPlanService;
        }

        /// <summary>
        /// Query All Rental Plains
        /// </summary>
        /// <remarks>
        /// Query All Rental Plains
        /// </remarks>
        /// <response code="200">Success query.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var rentalResponse = await _rentalPlanService.GetAllAsync();

                return Ok(rentalResponse);
            }
            catch (DomainException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Query Rental Plain by Id
        /// </summary>
        /// <remarks>
        /// Query Rental Plain by Id (RentalPlanId)
        /// </remarks>
        /// <response code="200">Success query.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        ///
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalPlanById(Guid id)
        {
            try
            {
                var rentalResponse = await _rentalPlanService.GetRentalPlanByIdAsync(id);
                
                return Ok(rentalResponse);
            }
            catch (DomainException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new Rental Plain
        /// </summary>
        /// <remarks>
        /// Create a new Rental Plain
        /// </remarks>
        /// <response code="200">Success query.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        ///
        [HttpPost]
        public async Task<IActionResult> CreateRentalPlan([FromBody] CreateRentalPlanRequest request)
        {
            try
            {
                var response = await _rentalPlanService.CreateRentalPlanAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }        
    }
}
