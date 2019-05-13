using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetIndexStatistics
{
    public class IndexStatisticsViewModel
    {
        public int NumberOfCustomers { get; set; }
        public int NumberOfAccounts { get; set; }
        public decimal? TotalBalance { get; set; }
    }
}
