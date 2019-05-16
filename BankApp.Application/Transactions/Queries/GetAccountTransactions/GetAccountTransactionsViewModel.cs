using BankApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Transactions.Queries.GetAccountTransactions
{
    public class GetAccountTransactionsViewModel
    {
        public List<Transaction> Transactions { get; set; }
    }
}
