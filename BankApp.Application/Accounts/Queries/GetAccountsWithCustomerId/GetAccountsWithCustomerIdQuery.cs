using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Accounts.Queries.GetAccountsWithCustomerId
{
    public class GetAccountsWithCustomerIdQuery : IRequest<List<Account>>
    {
        public int CustomerId { get; set; }
    }

    public class GetAccountsWithCustomerIdQueryHandler : IRequestHandler<GetAccountsWithCustomerIdQuery, List<Account>>
    {
        private readonly IBankAppDbContext _context;

        public GetAccountsWithCustomerIdQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> Handle(GetAccountsWithCustomerIdQuery request, CancellationToken cancellationToken)
        {
            return await (from acc in _context.Accounts
                    join disp in _context.Dispositions on acc.AccountId equals disp.AccountId
                    where disp.CustomerId == request.CustomerId
                    select acc).ToListAsync();
        }
    }
}
