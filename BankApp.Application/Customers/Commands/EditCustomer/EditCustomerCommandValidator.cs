using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Commands.EditCustomer
{
    public class EditCustomerCommandValidator : AbstractValidator<EditCustomerCommand>
    {
        public EditCustomerCommandValidator()
        {
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender required").MaximumLength(6).WithMessage("Maximum characters = 6");
            RuleFor(x => x.Givenname).NotEmpty().WithMessage("First name required").MaximumLength(100).WithMessage("Maximum characters = 100");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname required").MaximumLength(100).WithMessage("Maximum characters = 100");
            RuleFor(x => x.Streetaddress).NotEmpty().WithMessage("Street adress required").MaximumLength(100).WithMessage("Maximum characters = 100");
            RuleFor(x => x.City).NotEmpty().WithMessage("City required").MaximumLength(100).WithMessage("Maximum characters = 100");
            RuleFor(x => x.Zipcode).NotEmpty().WithMessage("Zipcode required").MaximumLength(15).WithMessage("Maximum characters = 15");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country required").MaximumLength(100).WithMessage("Maximum characters = 100");
            RuleFor(x => x.CountryCode).NotEmpty().WithMessage("Country code required").MaximumLength(2).WithMessage("Maximum characters = 2");
            RuleFor(x => x.NationalId).MaximumLength(20).WithMessage("Maximum characters = 20");
            RuleFor(x => x.Telephonecountrycode).MaximumLength(10).WithMessage("Maximum characters = 10");
            RuleFor(x => x.Telephonenumber).MaximumLength(25).WithMessage("Maximum characters = 25");
            RuleFor(x => x.Emailaddress).MaximumLength(100).WithMessage("Maximum characters = 100");
        }
    }
}
