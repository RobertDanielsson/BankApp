using BankApp.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Accounts.Queries.GetAccountStatistics
{
    public class GetAccountStatisticsQueryHandler : IRequestHandler<GetAccountStatisticsQuery, GetAccountStatisticsViewModel>
    {
        private readonly IBankAppDbContext _context;

        public GetAccountStatisticsQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<GetAccountStatisticsViewModel> Handle(GetAccountStatisticsQuery request, CancellationToken cancellationToken)
        {
            var model = new GetAccountStatisticsViewModel
            {
                Account = await _context.Accounts.AsNoTracking().SingleOrDefaultAsync(a => a.AccountId == request.AccountId),
                Transactions = await _context.Transactions.AsNoTracking().Where(a => a.AccountId == request.AccountId).OrderByDescending(a => a.TransactionId).Take(20).ToListAsync()
            };

            return model;
        }
    }
}
