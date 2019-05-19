using BankApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.APIQueries.GetTransactions
{
    public class GetTransactionsQuery : IRequest<List<Transaction>>
    {
        public int AccountId { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}
