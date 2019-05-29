using System;
using System.Linq;
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

        public async Task<string> ExecuteAsync(CreateVoucherCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (command == null) throw new ArgumentNullException(nameof(command));

                command.Validate(null);

                var account = command.ToDomain();

                await _accountRepository.AddAsync(account, cancellationToken);
                await _accountRepository.SaveChangesAsync(cancellationToken);

                return account.AccountId;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ExecuteAsync(ConsumeAccountCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (command == null) throw new ArgumentNullException(nameof(command));

                command.Validate(null);

                var accounts = _accountRepository.Get(ac => ac.Company == command.Company &&
                        ac.CPF == command.CPF &&
                        ac.AccountId == command.AccountId);

                if (!accounts.Any()) throw new InvalidOperationException("invalid account.");

                // get the selected account
                var single = accounts.Single();

                // consume from balance
                single.Consume(command.Value);

                // update selected account
                _accountRepository.Update(single);
                await _accountRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
