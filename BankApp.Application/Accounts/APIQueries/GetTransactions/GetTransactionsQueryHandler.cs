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

namespace BankApp.Application.Accounts.APIQueries.GetTransactions
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, List<Transaction>>
    {
        private readonly IBankAppDbContext _context;

        public GetTransactionsQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Transactions.Where(t => t.AccountId == request.AccountId).OrderByDescending(t => t.TransactionId).Skip(request.Offset).Take(request.Limit <= 20 ? request.Limit : 20).ToListAsync();
        }
    }
}
