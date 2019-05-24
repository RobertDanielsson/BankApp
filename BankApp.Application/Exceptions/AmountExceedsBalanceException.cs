using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Exceptions
{
    public class AmountExceedsBalanceException : Exception
    {
        public AmountExceedsBalanceException(int accountId)
            : base($"Amount exceeds available account balance. Account id: {accountId}")
        {
        }

    }
}
