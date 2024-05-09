using Microsoft.AspNetCore.Mvc;
using Taka.App.Authentication.Domain.Dtos;
using Taka.App.Authentication.Domain.Interfaces;
using Taka.App.Authentication.Application;
using Taka.Common;

namespace Taka.App.Authentication.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
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
            var user = await _userService.CreateUserAsync(request);
            var message = $"Success. User {user.Email} registered.";

            return ApiResponse(message, user.ToDto());
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
            var user = await _userService.UpdateUserAdminAsync(request.Email, request.Password);
            var message = $"Success. User {user.Email} altered.";

            return ApiResponse(message, user.ToDto());
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
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            var token = _tokenService.GenerateJwtToken(user);
            var message = $"Success. Generate Token from User {request.Email}.";

            return ApiResponse(message, token);
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
            var token = await _tokenService.RefreshToken(request);

            var message = $"Success. Generate Token from User {request.UserEmail}.";
            return ApiResponse(message, token);
        }
    }
}
