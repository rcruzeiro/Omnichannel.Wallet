using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Queries;
using Core.Framework.Repository;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Queries.Filters
{
    public class GetAccountFilter : BaseSpecification<Account>, IFilter
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string CPF { get; set; }

        public GetAccountFilter(string company, string cpf, string accountId)
            : base(ac => ac.Company == company && ac.CPF == cpf && ac.AccountId == accountId)
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
