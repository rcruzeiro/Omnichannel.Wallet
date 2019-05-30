using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Blockchain;
using Core.Framework.Cqrs.Commands;
using Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions;
using Omnichannel.Wallet.Platform.Domain.Accounts;
using Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Commands
{
    public class AccountsCommandHandler : IAccountsCommandHandler
    {
        readonly IAccountRepository _accountRepository;
        readonly IWalletBlockchainService _walletBlockchainService;

        public AccountsCommandHandler(IAccountRepository accountRepository, IWalletBlockchainService walletBlockchainService)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _walletBlockchainService = walletBlockchainService ?? throw new ArgumentNullException(nameof(walletBlockchainService));
        }

        public async Task<string> ExecuteAsync(CreateVoucherCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // validate command
                ValidateCommand(command);

                var account = command.ToDomain();

                await _accountRepository.AddAsync(account, cancellationToken);
                await _accountRepository.SaveChangesAsync(cancellationToken);

                // add the account transaction into the blockchain
                await AddBlockToBlockchain(account);

                return account.AccountId;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<string> ExecuteAsync(CreateGiftcardCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // validate command
                ValidateCommand(command);

                var account = command.ToDomain();

                await _accountRepository.AddAsync(account, cancellationToken);
                await _accountRepository.SaveChangesAsync(cancellationToken);

                // add the account transaction into the blockchain
                await AddBlockToBlockchain(account);

                return account.AccountId;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ExecuteAsync(ConsumeAccountCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // validate command
                ValidateCommand(command);

                // get any account type
                var accounts = await _accountRepository.GetAsync(ac =>
                        ac.Company == command.Company &&
                        ac.CPF == command.CPF &&
                        ac.AccountId == command.AccountId);

                if (!accounts.Any()) throw new InvalidOperationException("invalid account.");

                // get the selected account
                var account = accounts.Single();

                // validate account for change
                ValidateAccountForChange(account);

                // consume from balance
                account.Consume(command.Value, command.Location);

                // update selected account
                _accountRepository.Update(account);
                await _accountRepository.SaveChangesAsync(cancellationToken);

                // add the account transaction into the blockchain
                await AddBlockToBlockchain(account);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ExecuteAsync(RegisterGiftcardCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // validate command
                ValidateCommand(command);

                // get only giftcards
                var accounts = await _accountRepository.GetGiftcards(ac =>
                        ac.Company == command.Company &&
                        ac.AccountId == command.AccountId);

                if (!accounts.Any()) throw new InvalidOperationException("invalid account.");

                // get the selected account
                var account = accounts.Single();

                // validate account for change
                ValidateAccountForChange(account);

                // throw exception if the giftcard already registered by a CPF
                if (account.CPF != "00000000000") throw new InvalidOperationException("CPF already registered.");

                // register the giftcard with new CPF
                account.Register(command.CPF);

                // update selected account
                _accountRepository.Update(account);
                await _accountRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ExecuteAsync(ChargeGiftcardCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // validate command
                ValidateCommand(command);

                // get only giftcards
                var accounts = await _accountRepository.GetGiftcards(ac =>
                        ac.Company == command.Company &&
                        ac.CPF == command.CPF &&
                        ac.AccountId == command.AccountId);

                if (!accounts.Any()) throw new InvalidOperationException("invalid account.");

                // get the selected account
                var account = accounts.Single();

                // validate account for change
                ValidateAccountForChange(account);

                // charge giftcard
                account.Charge(command.Value, command.Location);

                // update selected account
                _accountRepository.Update(account);
                await _accountRepository.SaveChangesAsync(cancellationToken);

                // add the account transaction into the blockchain
                await AddBlockToBlockchain(account);
            }
            catch (Exception ex)
            { throw ex; }
        }

        private void ValidateCommand<T>(T command)
            where T : class, ICommand
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.Validate(null);
        }

        private void ValidateAccountForChange(Account account)
        {
            if (account.ExpiresOn.HasValue)
                if (account.ExpiresOn.Value < DateTimeOffset.Now) throw new InvalidOperationException("account expired.");
        }

        private async Task AddBlockToBlockchain(Account account)
        {
            // get the account latest transaction
            var transaction = account.Transactions.Last();

            // create an object for document db
            var result = new
            {
                transaction.ID,
                transaction.Location,
                transaction.OperationType,
                transaction.EventType,
                transaction.Value,
                transaction.CreatedAt,
                transaction.Account.Company,
                transaction.Account.AccountId,
                transaction.Account.CPF,
                transaction.Account.AccountType,
                transaction.Account.Balance
            };

            Block block = new Block(DateTimeOffset.Now, result);
            await _walletBlockchainService.AddBlock(block);
        }
    }
}
