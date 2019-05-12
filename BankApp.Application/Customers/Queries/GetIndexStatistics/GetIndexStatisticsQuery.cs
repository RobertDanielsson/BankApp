using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace BankApp.Application.Customers.Queries.GetIndexStatistics
{
    public class GetIndexStatisticsQuery : IRequest<IndexStatisticsViewModel>
    {
    }
}
