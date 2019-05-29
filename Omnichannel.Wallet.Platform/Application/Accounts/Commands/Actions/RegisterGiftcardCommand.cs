using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Commands;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions
{
    public class RegisterGiftcardCommand : ICommand
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string AccountId { get; set; }

        public RegisterGiftcardCommand(string company, string cpf, string accountId)
        {
            Company = company;
            CPF = cpf;
            AccountId = accountId;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(CPF))
                yield return new ValidationResult("invalid CPF.", new[] { nameof(CPF) });

            if (string.IsNullOrEmpty(AccountId))
                yield return new ValidationResult("invalid account.", new[] { nameof(AccountId) });
        }
    }
}
