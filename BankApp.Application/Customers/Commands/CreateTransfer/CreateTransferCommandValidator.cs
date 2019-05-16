using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Commands.CreateTransfer
{
    public class CreateTransferCommandValidator : AbstractValidator<CreateTransferCommand>
    {
        public CreateTransferCommandValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount has to be greater than 0.");
            RuleFor(x => x.SenderAccountId).NotEmpty().WithMessage("Select an account to send from");
            RuleFor(x => x.RecieverAccountId).NotEmpty().WithMessage("Enter a recievers account id");
        }
    }
}
