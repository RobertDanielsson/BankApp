using BankApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomersListSearch
{
    public class GetCustomersListSearchViewModel
    {
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int FirstPage { get; set; } = 1;
        public int LastPage { get; set; }
        public int PrevPage { get; set; }
        public int NextPage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
