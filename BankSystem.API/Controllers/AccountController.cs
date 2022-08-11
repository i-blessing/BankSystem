using BankSystem.Business.Dto;
using BankSystem.Business.Models;
using BankSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace BankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        private UserDto _userDto;

        public AccountController(IAccountService accountService, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]        
        public async Task<IActionResult> GetAccountsForUserAsync()
        {
            await GetUserDto();

            var result = await _accountService.GetAccountsForUserAsync(_userDto.UserId);

            return Ok(result);
        }

        /// <summary>
        /// Deposit money to an account
        /// </summary>
        /// <param name="dto"><see cref="DepositDto"/></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /deposit
        ///     {
        ///        "accountId": 1,
        ///        "amount": 100
        ///     }
        ///
        /// </remarks>
        /// <response code="200">If the deposit was successful</response>
        /// <response code="400">If the request validation fails, the account id is invalid, or the deposit amount exceeds the limit</response>
        /// <response code="401">If the bearer token is invalid</response>
        [HttpPost]
        [Route("deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DepositMoneyAsync([FromBody] DepositDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await GetUserDto();

            var result = await _accountService.CreateTransaction(new AccountTransactionDto
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = Business.Enums.TransactionType.Deposit
            });

            return Ok();
        }

        [HttpPost]
        [Route("withdrawal")]
        public async Task<IActionResult> WithdrawMoneyAsync([FromBody] DepositDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.CreateTransaction(new AccountTransactionDto
            {
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Type = Business.Enums.TransactionType.WithDrawal
            });

            return Ok();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAccountAsync()
        {
            await GetUserDto();
            await _accountService.CreateAccountAsync(new CreateAccountDto { UserId = _userDto.UserId });

            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteAccountAsync([FromQuery] long accountId)
        {
            await GetUserDto();
            await _accountService.DeleteAccountAsync(new DeleteAccountDto { AccountId = accountId });

            return Ok();
        }       

        private async Task GetUserDto()
        {
            var bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            _userDto = await _userService.GetUserForTokenAsync(bearerToken);
        }
    }
}
