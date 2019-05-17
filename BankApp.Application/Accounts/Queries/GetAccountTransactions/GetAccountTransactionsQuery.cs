using BankApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.Queries.GetAccountTransactions
{
    public class GetAccountTransactionsQuery : IRequest<List<Transaction>>
    {
        public int Page { get; set; } = 1;
        public int AccountId { get; set; }
    }
}
