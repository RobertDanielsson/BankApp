using BankApp.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //var model = new IndexStatisticsViewModel();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //model.NumberOfCustomers = await _context.Customers.CountAsync();
            //model.NumberOfAccounts = await _context.Accounts.CountAsync();
            //model.TotalBalance = await _context.Accounts.SumAsync(a => a.Amount);
            //var test = _context.Customers
            //    .Include(c => c.Dispositions)
            //    .ThenInclude(d => d.Account)
            //    .Select(b => new IndexStatisticsViewModel
            //    {
            //        NumberOfCustomers = c.Count()
            //    })

            var model = await _context.Customers.AsNoTracking().DefaultIfEmpty().Select(t => new IndexStatisticsViewModel
            {
                NumberOfCustomers = _context.Customers.OrderBy(c => c.CustomerId).Count(),
                NumberOfAccounts = _context.Accounts.OrderBy(c => c.AccountId).Count(),
                TotalBalance = _context.Accounts.OrderBy(c => c.AccountId).Sum(a => a.Balance)
            }).FirstOrDefaultAsync();

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");

            return model;
        }
    }
}
