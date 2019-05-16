using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Commands.CreateDeposit
{
    public class CreateDepositCommand : IRequest<string>
    {
        public decimal Amount { get; set; }
        public int CustomerId { get; set; }
        public int RecieverAccountId { get; set; }
    }
}
