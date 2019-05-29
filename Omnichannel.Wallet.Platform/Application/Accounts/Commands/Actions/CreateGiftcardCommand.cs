using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Commands;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions
{
    public class CreateGiftcardCommand : ICommand
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public decimal Value { get; set; }

        public string Location { get; set; }

        public DateTimeOffset? ExpiresOn { get; set; }

        public CreateGiftcardCommand(string company, decimal value)
        {
            Company = company;
            Value = value;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (Value == default)
                yield return new ValidationResult("invalid value.", new[] { nameof(Value) });
        }
    }

    internal static class CreateGiftcardCommandExtensions
    {
        internal static GiftcardAccount ToDomain(this CreateGiftcardCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            return (GiftcardAccount)Account.Create(AccountType.Giftcard,
                command.Company,
                command.Value,
                location: command.Location,
                expiresOn: command.ExpiresOn);
        }
    }
}
