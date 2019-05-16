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

            //var q = await _context.Customers
            //        .Include(x => x.Dispositions)
            //        .ThenInclude(x => x.Cards)
            //        .Include(c => c.Dispositions)
            //        .ThenInclude(c => c.Account)
            //        .ThenInclude(c => c.Loans)
            //        .Include(c => c.Dispositions)
            //        .ThenInclude(c => c.Account)
            //        .ThenInclude(c => c.PermenentOrder)
            //        .SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId);

            var test = await _context.Customers.AsNoTracking().Select(x => new GetCustomerDetailsViewmodel
            {
                Customer = x,
                Accounts = x.Dispositions.Select(c => c.Account).ToList(),
                Cards = x.Dispositions.SelectMany(c => c.Cards).ToList(),
                Loans = x.Dispositions.SelectMany(c => c.Account.Loans).ToList(),
                PermantentOrders = x.Dispositions.SelectMany(c => c.Account.PermenentOrder).ToList()

            }).Where(x => x.Customer.CustomerId == request.CustomerId).SingleAsync();

            test.TotalBalance = test.Accounts.Sum(s => s.Balance);

            //var model = new GetCustomerDetailsViewmodel();
            //model.Customer = q;

            //foreach (var item in q.Dispositions)
            //{
            //    model.Accounts.Add(item.Account);

            //    foreach (var card in item.Cards)
            //    {
            //        model.Cards.Add(card);
            //    }
            //    foreach (var permenentOrder in item.Account.PermenentOrder)
            //    {
            //        model.PermantentOrders.Add(permenentOrder);
            //    }
            //    foreach (var loan in item.Account.Loans)
            //    {
            //        model.Loans.Add(loan);
            //    }
            //    model.TotalBalance += item.Account.Balance;
            //}

            //var model = new GetCustomerDetailsViewmodel();
            //model.Customer = _context.Customers.SingleOrDefault(c => c.CustomerId == request.CustomerId);
            //if (model.Customer != null)
            //{
            //    model.Accounts = model.Customer.Dispositions.Select(x => x.Account).ToList();
            //    model.Cards = model.Customer.Dispositions.SelectMany(x => x.Cards).ToList();
            //    model.PermantentOrders = model.Accounts.SelectMany(x => x.PermenentOrder).ToList();
            //}

            return test;
        }
    }
}
