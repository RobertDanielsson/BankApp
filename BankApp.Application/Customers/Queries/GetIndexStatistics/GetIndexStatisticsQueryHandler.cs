using BankApp.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Queries.GetIndexStatistics
{
    public class GetIndexStatisticsQueryHandler : IRequestHandler<GetIndexStatisticsQuery, IndexStatisticsViewModel>
    {
        private readonly IBankAppDbContext _context;

        public GetIndexStatisticsQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<IndexStatisticsViewModel> Handle(GetIndexStatisticsQuery request, CancellationToken cancellationToken)
        {
            var model = new IndexStatisticsViewModel();

            model.NumberOfCustomers = await _context.Customers.CountAsync();
            model.NumberOfAccounts = await _context.Accounts.CountAsync();
            model.TotalBalance = await _context.Accounts.SumAsync(a => a.Amount);

            return model;
        }
    }
}
