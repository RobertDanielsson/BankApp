using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Commands.AddNewAccount
{
    public class AddNewAccountCommand : IRequest
    {
        public int CustomerId { get; set; }
    }
}
