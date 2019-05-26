using BankApp.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BankApp.Domain.Entities;
using BankApp.Application.Exceptions;

namespace BankApp.Application.Accounts.Commands.CreateInterest
{
    public class CreateInterestCommandHandler : IRequestHandler<CreateInterestCommand, Unit>
    {
        private readonly IBankAppDbContext _context;

        public CreateInterestCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateInterestCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FindAsync(request.AccountId);
            var lastInterest = await _context.Transactions.AsNoTracking()
                .Where(t => t.AccountId == request.AccountId && t.Symbol == "interest credited").OrderByDescending(t => t.TransactionId)
                .Take(1).FirstOrDefaultAsync();
            decimal interest = 0;

            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Date = request.DateTimeProvider.GetCurrentTime(),
                Type = "Credit",
                Bank = "SBS",
                Symbol = "interest credited",
                Operation = ""
            };

            if (lastInterest == null)
            {
                var days = (request.DateTimeProvider.GetCurrentTime() - account.Created).Days;
                interest = Math.Round((((decimal)0.023 / 365) * account.Balance) * days, 2);
            }
            else
            {
                var days = (request.DateTimeProvider.GetCurrentTime() - lastInterest.Date).Days;
                interest = Math.Round((((decimal)0.023 / 365) * account.Balance) * days, 2);
            }

            if (interest > 0)
            {
                account.Balance += interest;
                transaction.Amount = interest;
                transaction.Balance = account.Balance;
                _context.Transactions.Add(transaction);

                if (await _context.SaveChangesAsync(cancellationToken) == 2)
                {
                    return Unit.Value;
                }
                else
                {
                    throw new ErrorSavingToDatabaseException();
                }
            }
            else
            {
                throw new ErrorSavingToDatabaseException("No interest applied, check previous interest dates");
            }
        }
    }
}
