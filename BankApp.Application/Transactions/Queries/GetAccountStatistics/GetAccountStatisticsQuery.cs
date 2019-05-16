using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Transactions.Queries.GetAccountStatistics
{
    public class GetAccountStatisticsQuery : IRequest<GetAccountStatisticsViewModel>
    {
        public int AccountId { get; set; }
    }
}
