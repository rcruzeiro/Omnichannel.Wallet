using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Framework.Cqrs.Commands;
using Omnichannel.Wallet.Platform.Domain.Accounts;

namespace Omnichannel.Wallet.Platform.Application.Accounts.Commands.Actions
{
    public class CreateVoucherCommand : ICommand
    {
        [Required]
        public string Company { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public decimal Value { get; set; }

        public string Location { get; set; }

        public DateTimeOffset? ExpiresOn { get; set; }

        public CreateVoucherCommand(string company, string cpf, decimal value)
        {
            Company = company;
            CPF = cpf;
            Value = value;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Company))
                yield return new ValidationResult("invalid company.", new[] { nameof(Company) });

            if (string.IsNullOrEmpty(CPF))
                yield return new ValidationResult("invalid CPF.", new[] { nameof(CPF) });

            if (Value == default)
                yield return new ValidationResult("invalid value.", new[] { nameof(Value) });
        }
    }

    internal static class CreateVoucherCommandExtensions
    {
        internal static VoucherAccount ToDomain(this CreateVoucherCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            return (VoucherAccount)Account.Create(AccountType.Voucher,
                command.Company,
                command.Value,
                command.CPF,
                command.Location,
                command.ExpiresOn);
        }
    }
}
