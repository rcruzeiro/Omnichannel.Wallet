using System;
using System.Collections.Generic;
using Core.Framework.Entities;
using Newtonsoft.Json.Linq;

namespace Omnichannel.Wallet.Platform.Domain.Accounts
{
    public abstract class Account : Entity, IAggregationRoot
    {
        public string Company { get; set; }

        public string AccountId { get; protected set; }

        protected int accountType;
        public virtual AccountType AccountType => (AccountType)accountType;

        public string CPF { get; set; }

        public decimal InitialValue { get; protected set; }

        public decimal Balance { get; protected set; }

        public DateTimeOffset? ExpiresOn { get; protected set; }

        public JObject ExtensionAttributes { get; protected set; }

        protected readonly List<Transaction> _transactions = new List<Transaction>();
        public IReadOnlyCollection<Transaction> Transactions => _transactions;

        protected Account()
        { }

        public virtual void Consume(decimal value, string location = null)
        {
            try
            {
                if (value > Balance)
                    throw new InvalidOperationException($"the given value ({string.Format("{0:C}", value)}) is greater than the account balance ({string.Format("{0:C}", Balance)}).");

                // updates the account balance
                Balance -= value;
                UpdatedAt = DateTimeOffset.Now;

                var transaction = new Transaction(this, location, OperationType.Debit, EventType.Consume, value);
                _transactions.Add(transaction);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected abstract void SetNewAccountId();

        public static Account Create(AccountType accountType,
            string company,
            decimal initialValue,
            string cpf = null,
            string location = null,
            DateTimeOffset? expiresOn = null,
            JObject extensionAttributes = null)
        {
            Account account;

            switch (accountType)
            {
                case AccountType.Voucher:
                    account = new VoucherAccount(company, cpf, initialValue, expiresOn, extensionAttributes);
                    break;
                case AccountType.Giftcard:
                    account = new GiftcardAccount(company, initialValue, expiresOn, extensionAttributes);
                    break;
                default:
                    throw new KeyNotFoundException("invalid account type.");
            }

            // create id for the new account
            account.SetNewAccountId();

            // create initial transaction
            var transaction = new Transaction(account, location, OperationType.Credit, EventType.Create, initialValue);
            account._transactions.Add(transaction);

            return account;
        }
    }

    public enum AccountType
    {
        Voucher = 1,
        Giftcard
    }
}
