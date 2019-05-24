using BankApp.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Queries.GetCustomersListSearchNew
{
    public class GetCustomersListSearchNewQueryHandler : IRequestHandler<GetCustomersListSearchNewQuery, GetCustomersListSearchNewViewModel>
    {
        private readonly IBankAppDbContext _context;

        public GetCustomersListSearchNewQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<GetCustomersListSearchNewViewModel> Handle(GetCustomersListSearchNewQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.Page - 1) * request.PageSize;
            var pageOfResultsTask = _context.Customers.OrderBy(c => c.CustomerId).AsNoTracking().Where(c => c.Givenname.StartsWith(request.FirstName)).Skip(skip).Take(request.PageSize).ToListAsync();
            var countTask = _context.Customers.OrderBy(c => c.CustomerId).AsNoTracking().Where(c => c.Givenname.StartsWith(request.FirstName)).CountAsync();

            var results = await pageOfResultsTask;
            var count = await countTask;
            var totalPages = (int)Math.Ceiling(Decimal.Divide(count, request.PageSize));
            var firstPage = 1;
            var lastPage = totalPages;
            var prevPage = Math.Max(request.Page - 1, firstPage);
            var nextPage = Math.Min(request.Page + 1, lastPage);

            var model = new GetCustomersListSearchNewViewModel
            {
                TotalPages = totalPages,
                FirstPage = firstPage,
                LastPage = lastPage,
                PrevPage = prevPage,
                NextPage = nextPage,
                Customers = results,
                FirstName = request.FirstName,
                City = request.City
            };

            return model;
        }
    }
}
