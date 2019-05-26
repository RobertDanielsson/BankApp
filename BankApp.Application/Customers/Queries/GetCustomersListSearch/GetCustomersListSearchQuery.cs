using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomersListSearch
{
    public class GetCustomersListSearchQuery : IRequest<GetCustomersListSearchViewModel>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string FirstName { get; set; }
        public string City { get; set; }
    }
}
