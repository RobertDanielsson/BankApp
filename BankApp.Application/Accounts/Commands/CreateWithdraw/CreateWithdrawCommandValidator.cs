using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.Commands.CreateWithdraw
{
    public class CreateWithdrawCommandValidator : AbstractValidator<CreateWithdrawCommand>
    {
        public CreateWithdrawCommandValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount has to be positive");
            RuleFor(x => x.Amount).ScalePrecision(2, 10, true).WithMessage("Invalid amount");
            RuleFor(x => x.RecieverAccountId).NotEmpty().WithMessage("Reciever account id required");
        }
    }
}
