using BankApp.Application.Exceptions;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Commands.AddExistingAccount
{
    public class AddExistingAccountCommandHandler : IRequestHandler<AddExistingAccountCommand, Unit>
    {
        private readonly IBankAppDbContext _context;

        public AddExistingAccountCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddExistingAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.AccountId);

            if(account == null)
            {
                throw new AccountNotFoundException(request.AccountId);
            }
            else
            {
                var customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId);

                var disp = new Disposition
                {
                    Customer = customer,
                    Account = account,
                    Type = "Disponent"
                };

                _context.Dispositions.Add(disp);

                if(await _context.SaveChangesAsync(cancellationToken) == 1)
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
