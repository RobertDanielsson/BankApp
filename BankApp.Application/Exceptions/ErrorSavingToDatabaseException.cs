using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Exceptions
{
    public class ErrorSavingToDatabaseException : Exception
    {
        public ErrorSavingToDatabaseException(string message = "Error saving to database, contact admin") : base(message)
        {

        }
    }
}
