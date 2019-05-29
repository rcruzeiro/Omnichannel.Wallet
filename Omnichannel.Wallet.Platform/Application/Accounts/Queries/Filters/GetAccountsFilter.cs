using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Queries;
using Core.Framework.Repository;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Queries.Filters
{
    public class GetAccountsFilter : BaseSpecification<Account>, IFilter
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string CPF { get; set; }

        public GetAccountsFilter(string company, string cpf)
            : base(ac => ac.Company == company && ac.CPF == cpf && ac.Balance > 0)
        {
            Includes.Add(ac => ac.Transactions);
            Company = company;
            CPF = cpf;
        }

        public GetAccountsFilter(string company, string cpf, AccountType accountType)
            : base(ac => ac.Company == company && ac.CPF == cpf && ac.AccountType == accountType && ac.Balance > 0)
        {
            Includes.Add(ac => ac.Transactions);
            Company = company;
            CPF = cpf;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(CPF))
                yield return new ValidationResult("invalid CPF.", new[] { nameof(CPF) });
        }
    }
}
