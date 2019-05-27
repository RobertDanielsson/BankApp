using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.Commands.CreateDeposit
{
    public class CreateDepositCommandValidator : AbstractValidator<CreateDepositCommand>
    {
        public CreateDepositCommandValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0.00m).WithMessage("Amount must be positive, decimals separated by comma");
            RuleFor(x => x.Amount).ScalePrecision(2, 10, true).WithMessage("Invalid amount, max 2 decimals");
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account id required");
        }
    }
}