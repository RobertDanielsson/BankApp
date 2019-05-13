using BankApp.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Application.Customers.Queries.GetCustomerDetails
{
    class GetCustomerDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, GetCustomerDetailsViewmodel>
    {
        private readonly IBankAppDbContext _context;

        public GetCustomerDetailsQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<GetCustomerDetailsViewmodel> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
        {
            var model = new GetCustomerDetailsViewmodel();

            model.Customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId);
            model.Accounts = await (from acc in _context.Accounts
                                    join disp in _context.Dispositions on acc.AccountId equals disp.AccountId
                                    where disp.CustomerId == request.CustomerId
                                    select acc).ToListAsync();

            model.TotalBalance = model.Accounts.Sum(a => a.Balance);

            return model;
        }
    }
}
