using Microsoft.AspNetCore.Mvc;
using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Interfaces;
using Taka.App.Authentication.Domain.Responses;
using Taka.App.Authentication.Application;
using Taka.App.Authentication.Domain.Exceptions;
using Serilog;
using Taka.App.Authentication.Domain.Entities;
using Taka.App.Authentication.Domain.Enums;

namespace Taka.App.Authentication.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;


        public AuthController(IUserService userService, ITokenService tokenService, IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }


        /// <summary>
        /// Register one new user.
        /// </summary>
        /// <remarks>
        /// Use this feature to create a new user. All new users will be deliverer.
        /// </remarks>
        /// <response code="200">Success register user.</response>
        /// <response code="400">Fail validation.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            try
            {
                var user = await _userService.CreateUserAsync(request);
                Log.Information("Success. User {Email} registered.", user.Email);
                return Ok(user.ToDto());
            }
            catch (UserFailValidationException ex)
            {
                Log.Warning(ex, "Error. Fail validation {Email}. {Message}", request.Email, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error when trying to create user {Email}. {Message}", request.Email, ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// Update one new user.
        /// </summary>
        /// <remarks>
        /// Use this feature to change a user.
        /// </remarks>
        /// <response code="200">Success update user.</response>
        /// <response code="400">Fail validation.</response>
        /// <response code="500">Internal server error.</response>

        [HttpPut("alter")]
        public async Task<IActionResult> AlterAdmin([FromBody] UserUpdateRequest request)
        {
            try
            {
                var user = await _userService.UpdateUserAdminAsync(request.Email, request.Password);
                Log.Information("Success. User {Email} altered.", user.Email);
                return Ok(user.ToDto());
            }
            catch (UserFailValidationException ex)
            {
                Log.Warning(ex, "Error. Fail validation {Email}. {Message}", request.Email, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error when trying to update user {Email}. {Message}", request.Email, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <remarks>
        /// Use this feature to validate user access. If successful, a token will be returned.
        /// This token will be used to access the other microservices.
        /// </remarks>
        /// <response code="200">Success login.</response>
        /// <response code="401">User unauthorized. Check if the user exists and if their data has been entered correctly. </response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] UserLoginRequest request)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(request.Email, request.Password);

                var token = _tokenService.GenerateJwtToken(user);

                Log.Information("Success. Generate Token from User {Email}.", request.Email);
                return Ok(token);
            }
            catch (UserFailValidationException ex)
            {
                Log.Warning(ex, "Unauthorized {Email}. User or password invalid.", request.Email);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Unable to perform user {Email} authentication", request.Email);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Refresh token.
        /// </summary>
        /// <remarks>
        /// Use this feature to validate user access. If successful, a token will be returned.
        /// This token will be used to access the other microservices.
        /// </remarks>
        /// <response code="200">Success login.</response>
        /// <response code="401">User unauthorized. Check if the user exists and if their data has been entered correctly. </response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshTokenUser([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var token = await _refreshTokenService.RefreshToken(request);

                Log.Information("Success. Generate Token from User {Email}.", request.UserEmail);
                return Ok(token);
            }
            catch (UserFailValidationException ex)
            {
                Log.Warning(ex, "Unauthorized {Token}. User or password invalid.", request.Token);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Unable to perform user {Token} authentication", request.Token);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
