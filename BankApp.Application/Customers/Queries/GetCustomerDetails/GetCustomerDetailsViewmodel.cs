using BankApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomerDetails
{
    public class GetCustomerDetailsViewmodel
    {
        public Customer Customer { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
        public decimal TotalBalance { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public List<PermenentOrder> PermantentOrders { get; set; } = new List<PermenentOrder>();
        public List<Loan> Loans { get; set; } = new List<Loan>();

    }
}
