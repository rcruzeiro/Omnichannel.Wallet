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

        public async Task<string> ExecuteAsync(CreateGiftcardCommand command, CancellationToken cancellationToken = default)
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

                // get any account type
                var accounts = await _accountRepository.GetAsync(ac =>
                        ac.Company == command.Company &&
                        ac.CPF == command.CPF &&
                        ac.AccountId == command.AccountId);

                if (!accounts.Any()) throw new InvalidOperationException("invalid account.");

                // get the selected account
                var single = accounts.Single();

                // validate account for change
                ValidateAccountForChange(single);

                // consume from balance
                single.Consume(command.Value, command.Location);

                // update selected account
                _accountRepository.Update(single);
                await _accountRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ExecuteAsync(RegisterGiftcardCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (command == null) throw new ArgumentNullException(nameof(command));

                command.Validate(null);

                // get only giftcards
                var accounts = await _accountRepository.GetGiftcards(ac =>
                        ac.Company == command.Company &&
                        ac.AccountId == command.AccountId);

                if (!accounts.Any()) throw new InvalidOperationException("invalid account.");

                // get the selected account
                var single = accounts.Single();

                // validate account for change
                ValidateAccountForChange(single);

                // throw exception if the giftcard already registered by a CPF
                if (single.CPF != "00000000000") throw new InvalidOperationException("CPF already registered.");

                // register the giftcard with new CPF
                single.Register(command.CPF);

                // update selected account
                _accountRepository.Update(single);
                await _accountRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ExecuteAsync(ChargeGiftcardCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (command == null) throw new ArgumentNullException(nameof(command));

                command.Validate(null);

                // get only giftcards
                var accounts = await _accountRepository.GetGiftcards(ac =>
                        ac.Company == command.Company &&
                        ac.CPF == command.CPF &&
                        ac.AccountId == command.AccountId);

                if (!accounts.Any()) throw new InvalidOperationException("invalid account.");

                // get the selected account
                var single = accounts.Single();

                // validate account for change
                ValidateAccountForChange(single);

                // charge giftcard
                single.Charge(command.Value, command.Location);

                // update selected account
                _accountRepository.Update(single);
                await _accountRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        private void ValidateAccountForChange(Account account)
        {
            if (account.ExpiresOn.HasValue)
                if (account.ExpiresOn.Value < DateTimeOffset.Now) throw new InvalidOperationException("account expired.");
        }
    }
}
