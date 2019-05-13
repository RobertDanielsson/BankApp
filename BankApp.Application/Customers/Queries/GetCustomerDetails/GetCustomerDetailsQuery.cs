using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomerDetails
{
    public class GetCustomerDetailsQuery : IRequest<GetCustomerDetailsViewmodel>
    {
        public int CustomerId { get; set; }
    }
}
