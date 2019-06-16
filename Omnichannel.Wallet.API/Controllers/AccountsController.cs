using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omnichannel.Wallet.API.Messages.Accounts;
using Omnichannel.Wallet.Platform.Application.Accounts;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.Filters;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.API.Controllers
{
    [Authorize("Bearer")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        readonly IAccountsAppService _accountsAppService;

        public AccountsController(IAccountsAppService accountsAppService)
        {
            _accountsAppService = accountsAppService ?? throw new ArgumentNullException(nameof(accountsAppService));
        }
        /// <summary>
        /// Gets the CPF associated accounts.
        /// </summary>
        /// <returns>The CPF accounts.</returns>
        /// <param name="request">Route parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="200">Return accounts, if any, with no errors.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [ProducesResponseType(typeof(GetCPFAccountsResponse), 200)]
        [ProducesResponseType(typeof(GetCPFAccountsResponse), 500)]
        [HttpGet]
        public async Task<IActionResult> GetCPFAccounts([FromRoute]GetCPFAccountsRequest request, CancellationToken cancellationToken = default)
        {
            GetCPFAccountsResponse response = new GetCPFAccountsResponse();

            try
            {
                var filter = new GetAccountsFilter(request.Company, request.CPF);
                var result = await _accountsAppService.GetAccounts(filter, cancellationToken);

                response.StatusCode = 200;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Gets the CPF associated voucher accounts.
        /// </summary>
        /// <returns>The CPF voucher accounts.</returns>
        /// <param name="request">Route parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="200">Return voucher accounts, if any, with no errors.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [ProducesResponseType(typeof(GetCPFAccountsResponse), 200)]
        [ProducesResponseType(typeof(GetCPFAccountsResponse), 500)]
        [HttpGet("voucher")]
        public async Task<IActionResult> GetCPFVoucherAccounts([FromRoute]GetCPFAccountsRequest request, CancellationToken cancellationToken = default)
        {
            GetCPFAccountsResponse response = new GetCPFAccountsResponse();

            try
            {
                var filter = new GetAccountsFilter(request.Company, request.CPF, AccountType.Voucher);
                var result = await _accountsAppService.GetAccounts(filter, cancellationToken);

                response.StatusCode = 200;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Gets the CPF associated giftcard accounts.
        /// </summary>
        /// <returns>The CPF giftcard accounts.</returns>
        /// <param name="request">Route parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="200">Return giftcard accounts, if any, with no errors.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [ProducesResponseType(typeof(GetCPFAccountsResponse), 200)]
        [ProducesResponseType(typeof(GetCPFAccountsResponse), 500)]
        [HttpGet("giftcard")]
        public async Task<IActionResult> GetCPFGiftcardAccounts([FromRoute]GetCPFAccountsRequest request, CancellationToken cancellationToken = default)
        {
            GetCPFAccountsResponse response = new GetCPFAccountsResponse();

            try
            {
                var filter = new GetAccountsFilter(request.Company, request.CPF, AccountType.Giftcard);
                var result = await _accountsAppService.GetAccounts(filter, cancellationToken);

                response.StatusCode = 200;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Gets an specifict account by his unique identifier.
        /// </summary>
        /// <returns>The requested account.</returns>
        /// <param name="request">Request parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="200">Return the account, if exists, with no errors.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [ProducesResponseType(typeof(GetCPFAccountByAccountIdResponse), 200)]
        [ProducesResponseType(typeof(GetCPFAccountByAccountIdResponse), 500)]
        [HttpGet("{AccountId}")]
        public async Task<IActionResult> GetCPFAccountByAccountId([FromRoute]GetCPFAccountByAccountIdRequest request, CancellationToken cancellationToken = default)
        {
            GetCPFAccountByAccountIdResponse response = new GetCPFAccountByAccountIdResponse();

            try
            {
                var filter = new GetAccountFilter(request.Company, request.CPF, request.AccountId);
                var result = await _accountsAppService.GetAccounts(filter, cancellationToken);

                response.StatusCode = 200;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Creates a new voucher account.
        /// </summary>
        /// <returns>The voucher unique id.</returns>
        /// <param name="request">Request with new voucher basic data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="200">New voucher created with success.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateVoucherResponse), 200)]
        [ProducesResponseType(typeof(CreateVoucherResponse), 500)]
        [HttpPost("voucher")]
        public async Task<IActionResult> CreateVoucher([FromBody]CreateVoucherRequest request, CancellationToken cancellationToken = default)
        {
            CreateVoucherResponse response = new CreateVoucherResponse();

            try
            {
                var result = await _accountsAppService.CreateVoucher(request.Command, cancellationToken);

                response.StatusCode = 200;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Creates a new giftcard account.
        /// </summary>
        /// <returns>The giftcard unique id.</returns>
        /// <param name="request">Request with new giftcard basic data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="200">New giftcard created with success.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateGiftcardResponse), 200)]
        [ProducesResponseType(typeof(CreateGiftcardResponse), 500)]
        [HttpPost("giftcard")]
        public async Task<IActionResult> CreateGiftcard([FromBody]CreateGiftcardRequest request, CancellationToken cancellationToken = default)
        {
            CreateGiftcardResponse response = new CreateGiftcardResponse();

            try
            {
                var result = await _accountsAppService.CreateGiftcard(request.Command, cancellationToken);

                response.StatusCode = 200;
                response.Data = result;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Consumes the account.
        /// </summary>
        /// <param name="request">Request with consume data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="201">Account balance changed with success.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [AllowAnonymous]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ConsumeAccountResponse), 500)]
        [HttpPut("consume")]
        public async Task<IActionResult> ConsumeAccount([FromBody]ConsumeAccountRequest request, CancellationToken cancellationToken = default)
        {
            ConsumeAccountResponse response = new ConsumeAccountResponse();

            try
            {
                await _accountsAppService.Consume(request.Command, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Register the giftcard with a new CPF.
        /// </summary>
        /// <param name="request">Request with register data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="201">Giftcard is now registered.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(RegisterGiftcardResponse), 500)]
        [HttpPut("register")]
        public async Task<IActionResult> RegisterGiftcard([FromBody]RegisterGiftcardRequest request, CancellationToken cancellationToken = default)
        {
            RegisterGiftcardResponse response = new RegisterGiftcardResponse();

            try
            {
                var command = new RegisterGiftcardCommand(request.Company, request.CPF, request.AccountId);
                await _accountsAppService.Register(command, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Charges the giftcard.
        /// </summary>
        /// <param name="request">Request with charge data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <response code="201">Account balance changed with success.</response>
        /// <response code="500">Internal Server Error. See response messages for details.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ChargeGiftcardResponse), 500)]
        [HttpPut("charge")]
        public async Task<IActionResult> ChargeGiftcard([FromBody]ChargeGiftcardRequest request, CancellationToken cancellationToken = default)
        {
            ChargeGiftcardResponse response = new ChargeGiftcardResponse();

            try
            {
                var command = new ChargeGiftcardCommand(request.Company, request.CPF, request.AccountId, request.Value)
                { Location = request.Location };
                await _accountsAppService.Charge(command, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Messages.Add(ResponseMessage.Create(ex, ""));

                return StatusCode(500, response);
            }
        }
    }
}
