using BankApp.Application.Customers.Commands.CreateCustomer;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
    {
        private readonly IBankAppDbContext _context;

        public CreateCustomerCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            Random rnd = new Random();

            //for (int i = 0; i < 50; i++)
            //{
            //    var newUser = new Customer { f = "Ellinore", LastName = "Danielsson" };
            //    var newAccount = new Account { AccountNumber = rnd.Next(1, 500).ToString(), Balance = 50, Customer = newUser };
            //    _context.Customers.Add(newUser);
            //    _context.Accounts.Add(newAccount);
            //}
            //await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
