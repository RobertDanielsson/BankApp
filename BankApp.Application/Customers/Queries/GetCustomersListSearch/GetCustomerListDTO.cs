﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetCustomersListSearch
{
    public class GetCustomerListDTO
    {
        public string CustomerId { get; set; }
        public string Birthdate { get; set; }
        public string Name { get; set; }
        public string Streetaddress { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
    }
}
