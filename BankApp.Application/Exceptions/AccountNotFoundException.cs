using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(int accountId) : base($"Account with account id {accountId} not found")
        {

        }
    }
}
