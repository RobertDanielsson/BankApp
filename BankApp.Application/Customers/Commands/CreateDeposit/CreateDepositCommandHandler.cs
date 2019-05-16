using BankApp.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Commands.CreateDeposit
{
    public class CreateDepositCommandHandler : IRequestHandler<CreateDepositCommand, string>
    {
        private readonly IBankAppDbContext _context;

        public CreateDepositCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
        {
            var recieverAccount = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.RecieverAccountId);

            if(recieverAccount == null)
            {
                return "Deposit failed, reciever account id not found";
            }
            else if (request.Amount <= 0)
            {
                return "Deposit failed, amount has to be positive";
            }
            else
            {
                recieverAccount.Balance += request.Amount;
                await _context.SaveChangesAsync(cancellationToken);
                return "Deposit successful";
            }
        }
    }
}
