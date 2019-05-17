using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Accounts.Commands.CreateDeposit
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
            else
            {
                var transaction = new Transaction
                {
                    AccountId = recieverAccount.AccountId,
                    Date = DateTime.Now,
                    Type = "Debit",
                    Operation = "Deposit in Cash",
                    Amount = request.Amount,
                    Balance = recieverAccount.Balance + request.Amount
                };

                _context.Transactions.Add(transaction);

                recieverAccount.Balance += request.Amount;
                await _context.SaveChangesAsync(cancellationToken);
                return "Deposit successful";
            }
        }
    }
}
