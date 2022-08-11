using BankSystem.Business.Dto;
using BankSystem.Business.Models;
using BankSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        ///     Get user token
        /// </summary>
        /// <param name="request"><see cref="LoginRequest"/></param>
        /// <returns><see cref="LoginResponse"/></returns>
        /// Sample request:
        ///
        ///     POST /
        ///     {
        ///        "username": "test",
        ///        "password": "password"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">If the login was successful</response>
        /// <response code="400">If the request validation fails</response>
        /// <response code="401">If the credentials are invalid</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.LoginAsync(request);

            return Ok(response);
        }
    }
}
