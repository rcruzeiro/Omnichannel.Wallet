using System;
using Newtonsoft.Json.Linq;

namespace Omnichannel.Wallet.Platform.Domain.Accounts
{
    public class GiftcardAccount : Account
    {
        protected GiftcardAccount()
        { }

        internal GiftcardAccount(string company, decimal initialValue, DateTimeOffset? expiresOn = null, JObject extensionAttributes = null)
        {
            if (string.IsNullOrEmpty(company))
                throw new ArgumentNullException(nameof(company));

            if (initialValue == default)
                throw new InvalidOperationException("The account must be created with a value greater than zero.");

            accountType = (int)AccountType.Giftcard;
            Company = company;
            CPF = "00000000000";
            InitialValue = initialValue;
            Balance = initialValue;
            ExpiresOn = expiresOn;
            ExtensionAttributes = extensionAttributes;
            CreatedAt = DateTimeOffset.Now;
        }

        public void Charge(decimal value, string location = null)
        {
            try
            {
                // updates the account balance
                Balance += value;
                UpdatedAt = DateTimeOffset.Now;

                var transaction = new Transaction(this, location, OperationType.Credit, EventType.Charge, value);
                _transactions.Add(transaction);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Register(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) throw new ArgumentNullException(nameof(cpf));

            CPF = cpf;
            UpdatedAt = DateTimeOffset.Now;
        }

        protected override void SetNewAccountId()
        {
            try
            {
                string cpf = "0000";
                string guid = new Random().Next(1, 999999999).ToString("D9");
                string checker = new Random().Next(10, 99).ToString();

                AccountId = $"{DateTimeOffset.Now.ToString("yyyyMMdd")}{cpf}{guid}-{checker}";
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
