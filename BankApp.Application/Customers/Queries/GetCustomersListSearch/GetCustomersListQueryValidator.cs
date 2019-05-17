using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomersListSearch
{
    public class GetCustomersListQueryValidator : AbstractValidator<GetCustomersListQuery>
    {
        public GetCustomersListQueryValidator()
        {
            RuleFor(x => x.Search).NotEmpty().WithMessage("Please enter a first name or city.");
        }
    }
}
