using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BankApp.Application.Identity;

namespace BankApp.Persistence
{
    public class BankAppDbContext : IdentityDbContext<ApplicationUser>, IBankAppDbContext
    {
        public BankAppDbContext(DbContextOptions<BankAppDbContext> options) : base(options)
        {

        }

        public BankAppDbContext()
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BankApp;Trusted_Connection=True;");
        }
    }
}
