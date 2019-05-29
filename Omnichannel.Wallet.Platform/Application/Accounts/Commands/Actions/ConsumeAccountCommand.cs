using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Commands;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions
{
    public class ConsumeAccountCommand : ICommand
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string AccountId { get; set; }

        [Required]
        public decimal Value { get; set; }

        public ConsumeAccountCommand(string company, string cpf, string accountId, decimal value)
        {
            Company = company;
            CPF = cpf;
            AccountId = accountId;
            Value = value;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(CPF))
                yield return new ValidationResult("invalid CPF.", new[] { nameof(CPF) });

            if (string.IsNullOrEmpty(AccountId))
                yield return new ValidationResult("invalid account.", new[] { nameof(AccountId) });

            if (Value == default)
                yield return new ValidationResult("invalid value.", new[] { nameof(Value) });
        }
    }
}
