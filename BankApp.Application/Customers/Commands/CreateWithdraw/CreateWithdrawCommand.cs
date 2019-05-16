using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Commands.CreateWithdraw
{
    public class CreateWithdrawCommand : IRequest<string>
    {
        public decimal Amount { get; set; }
        public int CustomerId { get; set; }
        public int RecieverAccountId { get; set; }
    }
}
