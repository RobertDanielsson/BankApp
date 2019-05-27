using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Commands.AddExistingAccount
{
    public class AddExistingAccountCommand : IRequest
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
    }
}
