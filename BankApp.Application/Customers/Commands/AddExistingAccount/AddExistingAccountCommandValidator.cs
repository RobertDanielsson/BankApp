using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Commands.AddExistingAccount
{
    public class AddExistingAccountCommandValidator : AbstractValidator<AddExistingAccountCommand>
    {
        public AddExistingAccountCommandValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("Enter a valid account id").GreaterThanOrEqualTo(1).WithMessage("Invalid account id, id must be positive");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Error, no customer id sent. Contact admin").GreaterThanOrEqualTo(1).WithMessage("Invalid customer id, id must be positive");
        }
    }
}
