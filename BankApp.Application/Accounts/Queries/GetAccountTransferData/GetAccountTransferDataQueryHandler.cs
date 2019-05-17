using BankApp.Application.DTO;
using BankApp.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Accounts.Queries.GetAccountTransferData
{
    public class GetAccountTransferDataQueryHandler : IRequestHandler<GetAccountTransferDataQuery, GetAccountTransferDataViewModel>
    {
        private readonly IBankAppDbContext _context;

        public GetAccountTransferDataQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<GetAccountTransferDataViewModel> Handle(GetAccountTransferDataQuery request, CancellationToken cancellationToken)
        {
            var model = await _context.Customers.AsNoTracking().Select(x => new GetAccountTransferDataViewModel
            {
                Customer = new CustomerDTO
                {
                    Name = x.Givenname + " " + x.Surname,
                    CustomerId = x.CustomerId.ToString(),
                },
                Accounts = x.Dispositions.Select(c => new SelectListItem
                {
                    Value = c.Account.AccountId.ToString(),
                    Text = "Account id: " + c.Account.AccountId + " - " + c.Account.Balance.ToString("C", CultureInfo.CurrentCulture),

                }).ToList(),

            }).SingleOrDefaultAsync(c => c.Customer.CustomerId == request.CustomerId.ToString());

            return model;
        }
    }
}
