using BankApp.Application.DTO;
using BankApp.Domain.Entities;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomersListSearch
{
    public class CustomersListViewModel
    {
        public PagingList<CustomerDTO> Customers { get; set; } 
        public string Search { get; set; }
    }
}
