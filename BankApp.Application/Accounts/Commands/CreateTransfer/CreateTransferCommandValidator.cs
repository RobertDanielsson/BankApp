using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.Commands.CreateTransfer
{
    public class CreateTransferCommandValidator : AbstractValidator<CreateTransferCommand>
    {
        public CreateTransferCommandValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Invalid amount, must be positive");
            RuleFor(x => x.Amount).ScalePrecision(2, 10, true).WithMessage("Invalid amount, max 2 decimals");
            RuleFor(x => x.SenderAccountId).NotEmpty().WithMessage("Select an account to send from");
            RuleFor(x => x.RecieverAccountId).NotEmpty().WithMessage("Reciever account id required");
        }
    }
}
