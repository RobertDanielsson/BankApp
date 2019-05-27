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

namespace BankApp.Application.Customers.Queries.GetCustomersListSearch
{
    public class GetCustomersListSearchQueryHandler : IRequestHandler<GetCustomersListSearchQuery, GetCustomersListSearchViewModel>
    {
        private readonly IBankAppDbContext _context;

        public GetCustomersListSearchQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<GetCustomersListSearchViewModel> Handle(GetCustomersListSearchQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.Page - 1) * request.PageSize;
            IOrderedQueryable<Customer> query;
            var city = request.City;
            var surName = request.Surname;
            var givenName = request.Givenname;

            if (city != null && givenName != null && surName != null)
            {
                query = _context.Customers
                    .Where(c => c.City.Contains(city) && c.Givenname.Contains(givenName) && c.Surname.Contains(surName))
                    .Distinct()
                    .OrderBy(c => c.Givenname)
                    .ThenBy(c => c.Surname);
            }
            else if (givenName != null && surName != null)
            {
                query = _context.Customers
                    .Where(c => c.Givenname.Contains(givenName) && c.Surname.Contains(surName))
                    .Distinct()
                    .OrderBy(c => c.Givenname)
                    .ThenBy(c => c.Surname);
            }
            else if (givenName != null && city != null)
            {
                query = _context.Customers
                    .Where(c => c.City.Contains(city) && c.Givenname.Contains(givenName))
                    .Distinct()
                    .OrderBy(c => c.Givenname)
                    .ThenBy(c => c.Surname);
            }
            else if (surName != null && city != null)
            {
                query = _context.Customers
                    .Where(c => c.City.Contains(city) && c.Surname.Contains(surName))
                    .Distinct()
                    .OrderBy(c => c.Givenname)
                    .ThenBy(c => c.Surname);
            }
            else if (city != null)
            {
                query = _context.Customers
                    .Where(c => c.City.Contains(city))
                    .Distinct()
                    .OrderBy(c => c.Givenname)
                    .ThenBy(c => c.Surname);
            }
            else if(surName != null)
            {
                query = _context.Customers
                    .Where(c => c.Surname.Contains(surName))
                    .Distinct()
                    .OrderBy(c => c.Givenname)
                    .ThenBy(c => c.Surname);
            }
            else if(givenName != null)
            {
                query = _context.Customers
                   .Where(c => c.Givenname.Contains(givenName))
                   .Distinct()
                   .OrderBy(c => c.Givenname)
                   .ThenBy(c => c.Surname);
            }
            else
            {
                query = _context.Customers
                    .Where(c => c.City.Contains(city) || c.Givenname.Contains(givenName) || c.Surname.Contains(surName))
                    .Distinct()
                    .OrderBy(c => c.Givenname)
                    .ThenBy(c => c.Surname);
            }

            var pageOfResultsTask = query.AsNoTracking().Skip(skip).Take(request.PageSize).ToListAsync();
            var countTask = query.AsNoTracking().CountAsync();

            var results = await pageOfResultsTask;
            var count = await countTask;
            var totalPages = (int)Math.Ceiling(Decimal.Divide(count, request.PageSize));
            var firstPage = 1;
            var lastPage = totalPages;
            var prevPage = Math.Max(request.Page - 1, firstPage);
            var nextPage = Math.Min(request.Page + 1, lastPage);

            var model = new GetCustomersListSearchViewModel
            {
                TotalPages = totalPages,
                FirstPage = firstPage,
                LastPage = lastPage,
                PrevPage = prevPage,
                NextPage = nextPage,
                Customers = results,
                Givenname = request.Givenname,
                City = request.City,
                CurrentPage = request.Page,
                Surname = request.Surname
            };

            return model;
        }
    }
}
