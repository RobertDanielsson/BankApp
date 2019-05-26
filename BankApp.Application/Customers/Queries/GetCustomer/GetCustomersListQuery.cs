using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomersListSearch
{
    public class GetCustomersListQuery : IRequest<GetCustomersListSearchViewModel>
    {
        public string FirstName { get; set; }
        public string City { get; set; }
        public int Page { get; set; } = 1;
    }
}
