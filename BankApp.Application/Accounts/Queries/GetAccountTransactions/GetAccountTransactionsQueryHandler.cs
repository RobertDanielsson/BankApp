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

namespace BankApp.Application.Accounts.Queries.GetAccountTransactions
{
    public class GetAccountTransactionsQueryHandler : IRequestHandler<GetAccountTransactionsQuery, List<Transaction>>
    {
        private readonly IBankAppDbContext _context;

        public GetAccountTransactionsQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> Handle(GetAccountTransactionsQuery request, CancellationToken cancellationToken)
        {
            var model = await _context.Transactions.AsNoTracking().Where(t => t.AccountId == request.AccountId).OrderByDescending(a => a.TransactionId).Skip(20 * request.Page).Take(20).ToListAsync();
            return model;
        }
    }
}
