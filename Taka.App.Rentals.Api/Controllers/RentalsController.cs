using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taka.App.Deliverer.Domain.Exceptions;
using Taka.App.Rentals.Domain.Enums;
using Taka.App.Rentals.Domain.Interfaces;
using Taka.App.Rentals.Domain.Requests;

namespace Taka.App.Rentals.Api.Controllers
{
    [Authorize(Policy = "MustHaveMicroserviceAccess")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        /// <summary>
        /// Create a new Rental
        /// </summary>
        /// <remarks>
        /// Create a new Rental. If the delivery person does not send the value to the PredictedEndDate field, according 
        /// to the business rule, the value will be defaulted to tomorrow's date.
        /// </remarks>
        /// <response code="200">Success create rental.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequest request)
        {
            try
            {
                var rental = await _rentalService.CreateRentalAsync(request);
                return CreatedAtAction(nameof(GetRentalById), new { id = rental.RentalId }, rental);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update or fineshed a new Rental
        /// </summary>
        /// <remarks>
        /// Update or fineshed a new Rental
        /// </remarks>
        /// <response code="200">Success create rental.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpPut]
        public async Task<IActionResult> CompleteRental([FromBody] CompleteRentalRequest request)
        {
            try
            {
                var rental = await _rentalService.CompleteRentalAsync(request);
                return Ok(rental);
            }
            catch (DomainException ex)
            {
                switch (ex.ErrorCode)
                {
                    case DomainErrorCode.RentalNotFound:
                        return NotFound(ex.Message);
                    case DomainErrorCode.RentalMotorcycleAlreadyExists:
                        return BadRequest(ex.Message);
                    default:
                        return StatusCode(500, "One or more errors occurred.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Query rental by Id
        /// </summary>
        /// <remarks>
        /// Query rental by Id ( RentalId )
        /// </remarks>
        /// <response code="200">Success query.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalById(Guid id)
        {
            try
            {
                var rentalResponse = await _rentalService.GetRentalByIdAsync(id);

                return Ok(rentalResponse);
            }
            catch (DomainException ex)
            {
                switch (ex.ErrorCode)
                {
                    case DomainErrorCode.RentalNotFound:
                        return NotFound(ex.Message);
                    case DomainErrorCode.RentalMotorcycleAlreadyExists:
                        return BadRequest(ex.Message);
                    default:
                        return StatusCode(500, "One or more errors occurred.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Query rental with details by Id
        /// </summary>
        /// <remarks>
        /// Query rental with details by Id ( RentalId )
        /// </remarks>
        /// <response code="200">Success query.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetRentalWithDetailsById(Guid id)
        {
            try
            {
                var detailsResponse = await _rentalService.GetRentalWithDetailsByIdAsync(id);

                return Ok(detailsResponse);
            }
            catch (DomainException ex)
            {
                switch (ex.ErrorCode)
                {
                    case DomainErrorCode.RentalNotFound:
                        return NotFound(ex.Message);
                    case DomainErrorCode.RentalMotorcycleAlreadyExists:
                        return BadRequest(ex.Message);
                    default:
                        return StatusCode(500, "One or more errors occurred.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Query rentals by MotorcycleId
        /// </summary>
        /// <remarks>
        /// Query rentals by MotorcycleId
        /// </remarks>
        /// <response code="200">Success query.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 
        [HttpGet("motorcycle/{motorcycleId}")]
        public async Task<IActionResult> CheckExistenceOfRentalsByMotorcycle(Guid motorcycleId)
        {
            try
            {
                var rentals = await _rentalService.CheckExistenceOfRentalsByMotorcycleAsync(motorcycleId);
                return Ok(rentals);
            }
            catch (DomainException ex)
            {
                switch (ex.ErrorCode)
                {
                    case DomainErrorCode.RentalNotFound:
                        return NotFound(ex.Message);
                    case DomainErrorCode.RentalMotorcycleAlreadyExists:
                        return BadRequest(ex.Message);
                    default:
                        return StatusCode(500, "One or more errors occurred.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Query rentals by DelivererId
        /// </summary>
        /// <remarks>
        /// Query rentals by DelivererId
        /// </remarks>
        /// <response code="200">Success query.</response>        
        /// <response code="404">Not found resulting.</response>        
        /// <response code="400">Fail. Request not responding.</response>
        /// <response code="500">Internal server error.</response>
        /// 

        [HttpGet("deliverer/{delivererId}")]
        public async Task<IActionResult> CheckExistenceOfRentalsByDeliverer(Guid delivererId)
        {
            try
            {
                var rentals = await _rentalService.CheckExistenceOfRentalsByDelivererAsync(delivererId);
                return Ok(rentals);
            }
            catch (DomainException ex)
            {
                switch (ex.ErrorCode)
                {
                    case DomainErrorCode.RentalNotFound:
                        return NotFound(ex.Message);
                    case DomainErrorCode.RentalMotorcycleAlreadyExists:
                        return BadRequest(ex.Message);
                    default:
                        return StatusCode(500, "One or more errors occurred.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}