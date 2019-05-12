using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Domain.Entities
{
    public class Customer
    {
        public Customer()
        {
            Accounts = new List<Account>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public ICollection<Account> Accounts { get; }
    }
}
