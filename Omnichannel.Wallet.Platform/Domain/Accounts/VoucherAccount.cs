using System;
using Newtonsoft.Json.Linq;

namespace Omnichannel.Wallet.Platform.Domain.Accounts
{
    public class VoucherAccount : Account
    {
        protected VoucherAccount()
        { }

        internal VoucherAccount(string company, string cpf, decimal initialValue, DateTimeOffset? expiresOn = null, JObject extensionAttributes = null)
        {
            if (string.IsNullOrEmpty(company))
                throw new ArgumentNullException(nameof(company));

            if (string.IsNullOrEmpty(cpf))
                throw new ArgumentNullException(nameof(cpf));

            if (initialValue == default)
                throw new InvalidOperationException("The account must be created with a value greater than zero.");

            accountType = (int)AccountType.Voucher;
            Company = company;
            CPF = cpf;
            InitialValue = initialValue;
            Balance = initialValue;
            ExpiresOn = expiresOn;
            ExtensionAttributes = extensionAttributes;
            CreatedAt = DateTimeOffset.Now;
        }

        protected override void SetNewAccountId()
        {
            try
            {
                string cpf = CPF.Substring(0, 4);
                string guid = new Random().Next(1, 999999999).ToString("D9");
                string checker = new Random().Next(10, 99).ToString();

                AccountId = $"{DateTimeOffset.Now.ToString("yyyyMMdd")}{cpf}{guid}-{checker}";
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
