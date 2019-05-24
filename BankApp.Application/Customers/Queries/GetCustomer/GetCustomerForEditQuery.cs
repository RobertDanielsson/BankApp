using BankApp.Application.Customers.Commands.EditCustomer;
using BankApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerForEditQuery : IRequest<EditCustomerCommand>
    {
        public int CustomerId { get; set; }
    }
}
