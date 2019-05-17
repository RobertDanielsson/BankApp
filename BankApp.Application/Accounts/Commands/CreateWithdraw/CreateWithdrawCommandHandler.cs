using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Accounts.Commands.CreateWithdraw
{
    class CreateWithdrawCommandHandler : IRequestHandler<CreateWithdrawCommand, string>
    {
        private readonly IBankAppDbContext _context;

        public CreateWithdrawCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateWithdrawCommand request, CancellationToken cancellationToken)
        {
            var recieverAccount = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.RecieverAccountId);

            if (recieverAccount == null)
            {
                return "Withdraw failed, reciever account id not found";
            }
            else if (request.Amount <= 0)
            {
                return "Withdraw failed, amount has to be positive";
            }
            else if ((recieverAccount.Balance - request.Amount) < 0)
            {
                return "Withdraw failed, amount exceeded available balance";
            }
            else
            {
                var transaction = new Transaction
                {
                    AccountId = recieverAccount.AccountId,
                    Date = DateTime.Now,
                    Type = "Debit",
                    Operation = "Withdrawal in Cash",
                    Amount = request.Amount * -1,
                    Balance = recieverAccount.Balance - request.Amount
                };

                _context.Transactions.Add(transaction);
                recieverAccount.Balance -= request.Amount;
                await _context.SaveChangesAsync(cancellationToken);
                return "Withdraw successful";
            }
        }
    }
}
