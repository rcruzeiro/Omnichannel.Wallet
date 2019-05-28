using System;
using System.Threading;
using System.Threading.Tasks;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Commands
{
    public class AccountsCommandHandler : IAccountsCommandHandler
    {
        readonly IAccountRepository _accountRepository;

        public AccountsCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task ExecuteAsync(CreateVoucherCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (command == null) throw new ArgumentNullException(nameof(command));

                command.Validate(null);

                var account = command.ToDomain();

                await _accountRepository.AddAsync(account, cancellationToken);
                await _accountRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
