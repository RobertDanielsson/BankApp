using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Commands.CreateDeposit
{
    public class CreateDepositCommandValidator : AbstractValidator<CreateDepositCommand>
    {
        public CreateDepositCommandValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount has to be greater than 0.");
            RuleFor(x => x.RecieverAccountId).NotEmpty().WithMessage("Enter a recievers account id");
        }
    }
}
