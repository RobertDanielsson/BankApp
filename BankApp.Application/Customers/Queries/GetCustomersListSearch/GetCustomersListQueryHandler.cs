﻿using BankApp.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Queries.GetCustomersListSearch
{
    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, CustomersListViewModel>
    {
        private readonly IBankAppDbContext _context;

        public GetCustomersListQueryHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomersListViewModel> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
        {
            var model = new CustomersListViewModel();
            //var result = _context.Customers.AsNoTracking().Where(c => c.FirstName.Contains(request.SearchInput)).OrderBy(c => c.Id);
            //model.Customers = await PagingList.CreateAsync(result, 10, request.Page);
            //model.Customers.RouteValue = new RouteValueDictionary
            //{
            //    {"searchInput", request.SearchInput }
            //};

            return model;
        }
    }
}