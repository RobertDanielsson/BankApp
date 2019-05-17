using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Customer>
    {
        private readonly IBankAppDbContext _context;

        public GetCustomerQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public Task<Customer> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            return _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId);
        }
    }
}
