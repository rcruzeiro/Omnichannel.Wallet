using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.DTOs;
using Omnichannel.Wallet.Platform.Application.Accounts.Queries.Filters;

namespace Omnichannel.Wallet.Platform.Application.Accounts
{
    public class AccountsAppService : IAccountsAppService
    {
        readonly IAccountsCommandHandler _accountsCommandHandler;
        readonly IAccountsQueryHandler _accountsQueryHandler;

        public AccountsAppService(IAccountsCommandHandler accountsCommandHandler,
                                  IAccountsQueryHandler accountsQueryHandler)
        {
            _accountsCommandHandler = accountsCommandHandler ?? throw new ArgumentNullException(nameof(accountsCommandHandler));
            _accountsQueryHandler = accountsQueryHandler ?? throw new ArgumentNullException(nameof(accountsQueryHandler));
        }

        public async Task<string> CreateVoucher(CreateVoucherCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _accountsCommandHandler.ExecuteAsync(command, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<string> CreateGiftcard(CreateGiftcardCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _accountsCommandHandler.ExecuteAsync(command, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Consume(ConsumeAccountCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _accountsCommandHandler.ExecuteAsync(command, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Register(RegisterGiftcardCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _accountsCommandHandler.ExecuteAsync(command, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Charge(ChargeGiftcardCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _accountsCommandHandler.ExecuteAsync(command, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<List<AccountDTO>> GetAccounts(GetAccountsFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _accountsQueryHandler.HandleAsync(filter, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<AccountDTO> GetAccounts(GetAccountFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _accountsQueryHandler.HandleAsync(filter, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
