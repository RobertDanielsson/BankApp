using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Exceptions
{
    public class NegativeAmountException : Exception
    {
        public NegativeAmountException() : base($"Invalid amount, must be positive")
        {

        }
    }
}
