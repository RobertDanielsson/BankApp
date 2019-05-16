using BankApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Interfaces
{
    public interface IBankAppDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Card> Cards { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Disposition> Dispositions { get; set; }
        DbSet<Loan> Loans { get; set; }
        DbSet<PermenentOrder> PermenentOrder { get; set; }
        DbSet<Transaction> Transactions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
