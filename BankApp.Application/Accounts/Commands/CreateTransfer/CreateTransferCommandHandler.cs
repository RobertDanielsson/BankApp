﻿using BankApp.Application.Exceptions;
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
    public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, Unit>
    {
        private readonly IBankAppDbContext _context;

        public CreateTransferCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            var senderAccount = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.SenderAccountId);
            var recieverAccount = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.RecieverAccountId);

            if(recieverAccount == null)
            {
                throw new AccountNotFoundException(request.RecieverAccountId);
            }
            else if(request.Amount <= 0)
            {
                throw new NegativeAmountException();
            }
            else if(senderAccount == null)
            {
                throw new AccountNotFoundException(request.SenderAccountId);
            }
            else if(request.RecieverAccountId == request.SenderAccountId)
            {
                throw new AccountNotFoundException(request.RecieverAccountId);
            }
            else if((senderAccount.Balance - request.Amount) < 0)
            {
                throw new AmountExceedsBalanceException(request.SenderAccountId);
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
                    Balance = senderAccount.Balance - request.Amount,
                    Bank = "SBS",
                    Account = request.RecieverAccountId.ToString()
                };

                var transactionReciever = new Transaction
                {
                    AccountId = recieverAccount.AccountId,
                    Date = DateTime.Now,
                    Type = "Credit",
                    Operation = "Transfer",
                    Amount = request.Amount,
                    Balance = recieverAccount.Balance + request.Amount,
                    Bank = "SBS",
                    Account = request.SenderAccountId.ToString()
                };

                _context.Transactions.AddRange(transactionSender, transactionReciever);

                senderAccount.Balance -= request.Amount;
                recieverAccount.Balance += request.Amount;

                if(await _context.SaveChangesAsync(cancellationToken) == 4)
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
