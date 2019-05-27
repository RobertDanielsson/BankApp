using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(int customerId) : base($"Customer with id {customerId} not found")
        {

        }
    }
}
