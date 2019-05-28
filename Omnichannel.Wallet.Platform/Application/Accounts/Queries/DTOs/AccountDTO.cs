using System;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Queries.DTOs
{
    public class AccountDTO
    {
        public int ID { get; set; }

        public string AccountId { get; set; }

        public AccountType AccountType { get; set; }

        public string CPF { get; set; }

        public decimal InitialValue { get; set; }

        public decimal Balance { get; set; }

        public DateTimeOffset? ExpiresOn { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }

    internal static class AccountDTOExtensions
    {
        internal static AccountDTO ToDTO(this Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            return new AccountDTO
            {
                ID = account.ID,
                AccountId = account.AccountId,
                AccountType = account.AccountType,
                CPF = account.CPF,
                InitialValue = account.InitialValue,
                Balance = account.Balance,
                ExpiresOn = account.ExpiresOn,
                CreatedAt = account.CreatedAt
            };
        }
    }
}
