using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.Commands.CreateDeposit
{
    public class CreateDepositCommand : IRequest
    {
        public decimal Amount { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
    }
}
