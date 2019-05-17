using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.Commands.CreateTransfer
{
    public class CreateTransferCommand : IRequest<string>
    {
        public decimal Amount { get; set; }
        public int CustomerId { get; set; }
        public int SenderAccountId { get; set; }
        public int RecieverAccountId { get; set; }
    }
}
