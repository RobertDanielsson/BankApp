using BankApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Transactions.Queries.GetAccountStatistics
{
    public class GetAccountStatisticsViewModel
    {
        public Account Account { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
