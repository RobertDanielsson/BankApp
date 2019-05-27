using BankApp.Application.Exceptions;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Commands.AddNewAccount
{
    public class AddNewAccountCommandHandler : IRequestHandler<AddNewAccountCommand>
    {
        private readonly IBankAppDbContext _context;

        public AddNewAccountCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddNewAccountCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId);

            if (customer == null)
            {
                throw new CustomerNotFoundException(request.CustomerId);
            }
            else
            {
                var account = new Account
                {
                    Frequency = "Monthly",
                    Created = DateTime.Now,
                    Balance = 0
                };

                var disp = new Disposition
                {
                    Customer = customer,
                    Account = account,
                    Type = "Owner"
                };

                _context.Accounts.Add(account);
                _context.Dispositions.Add(disp);

                if (await _context.SaveChangesAsync(cancellationToken) == 2)
                {
                    return Unit.Value;
                }
                else
                {
                    throw new ErrorSavingToDatabaseException();
                }
            }
        }
    }
}
