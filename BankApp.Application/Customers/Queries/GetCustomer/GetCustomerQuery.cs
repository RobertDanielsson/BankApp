using BankApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerQuery : IRequest<Customer>
    {
        public int CustomerId { get; set; }
    }
}
