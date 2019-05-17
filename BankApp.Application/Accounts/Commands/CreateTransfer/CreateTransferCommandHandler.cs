using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Accounts.Commands.CreateTransfer
{
    public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, string>
    {
        private readonly IBankAppDbContext _context;

        public CreateTransferCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            var senderAccount = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.SenderAccountId);
            var recieverAccount = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.RecieverAccountId);

            if(recieverAccount == null)
            {
                return "Transfer failed, account id not found";
            }
            else if(request.RecieverAccountId == request.SenderAccountId)
            {
                return "Transfer failed, reciever and sender account id are the same";
            }
            else if((senderAccount.Balance - request.Amount) < 0)
            {
                return "Transfer failed, amount exceeds balance";
            }
            else
            {
                var transactionSender = new Transaction
                {
                    AccountId = senderAccount.AccountId,
                    Date = DateTime.Now,
                    Type = "Debit",
                    Operation = "Transfer",
                    Amount = request.Amount * -1,
                    Balance = senderAccount.Balance - request.Amount
                };

                var transactionReciever = new Transaction
                {
                    AccountId = recieverAccount.AccountId,
                    Date = DateTime.Now,
                    Type = "Debit",
                    Operation = "Transfer",
                    Amount = request.Amount,
                    Balance = recieverAccount.Balance + request.Amount
                };

                _context.Transactions.AddRange(transactionSender, transactionReciever);

                senderAccount.Balance -= request.Amount;
                recieverAccount.Balance += request.Amount;
                await _context.SaveChangesAsync(cancellationToken);
                return "Transfer successful";
            }
        }
    }
}
