using BankApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomerDetails
{
    public class GetCustomerDetailsViewmodel
    {
        public Customer Customer { get; set; }
        public List<Account> Accounts { get; set; }
        public decimal TotalBalance { get; set; }

    }
}
