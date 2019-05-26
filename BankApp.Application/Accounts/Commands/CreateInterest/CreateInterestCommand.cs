using BankApp.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.Commands.CreateInterest
{
    public class CreateInterestCommand : IRequest
    {
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public IDateTime DateTimeProvider { get; set; }
    }
}
