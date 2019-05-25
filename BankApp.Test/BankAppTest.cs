using BankApp.Application.Accounts.Commands.CreateDeposit;
using BankApp.Application.Accounts.Commands.CreateTransfer;
using BankApp.Application.Accounts.Commands.CreateWithdraw;
using BankApp.Application.Exceptions;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankApp.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Test
{
    [TestClass]
    public class BankAppTest
    {

        [TestMethod]
        [ExpectedException(typeof(AmountExceedsBalanceException))]
        public async Task Test_Withdraw_exceeded()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankAppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Withdraw_exceed")
                .Options;


            //Act
            using (var context = new BankAppDbContext(options))
            {
                var account = new Account
                {
                    Balance = 10,
                };

                context.Accounts.Add(account);
                await context.SaveChangesAsync();

                var x = new CreateWithdrawCommandHandler(context);
                await x.Handle(new CreateWithdrawCommand { Amount = 20, AccountId = account.AccountId }, new CancellationToken());
            }

            //Assert that method throws AmountExceedsBalanceException
        }

        [TestMethod]
        [ExpectedException(typeof(AmountExceedsBalanceException))]
        public async Task Test_Transfer_exceeded()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankAppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Transfer_exceeded")
                .Options;


            //Act
            using (var context = new BankAppDbContext(options))
            {
                var senderAccount = new Account
                {
                    Balance = 10,
                };
                var recieverAccount = new Account
                {
                    Balance = 0
                };

                context.AddRange(senderAccount, recieverAccount);
                await context.SaveChangesAsync();

                var x = new CreateTransferCommandHandler(context);
                await x.Handle(new CreateTransferCommand { Amount = 20, SenderAccountId = senderAccount.AccountId, RecieverAccountId = recieverAccount.AccountId }, new CancellationToken());
            }

            //Assert that method throws AmountExceedsBalanceException
        }

        [TestMethod]
        [ExpectedException(typeof(NegativeAmountException))]
        public async Task Test_Negative_Deposit()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankAppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Negative_Deposit")
                .Options;

            //Act
            using (var context = new BankAppDbContext(options))
            {
                var account = new Account
                {
                    Balance = 10,
                };

                context.Accounts.Add(account);
                await context.SaveChangesAsync();

                var x = new CreateDepositCommandHandler(context);
                await x.Handle(new CreateDepositCommand { Amount = -20, AccountId = account.AccountId }, new CancellationToken());
            }

            //Assert that method throws NegativeAmountException
        }

        [TestMethod]
        [ExpectedException(typeof(NegativeAmountException))]
        public async Task Test_Negative_Witdrawal()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankAppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Negative_Witdrawal")
                .Options;

            //Act
            using (var context = new BankAppDbContext(options))
            {
                var account = new Account
                {
                    Balance = 10,
                };

                context.Accounts.Add(account);
                await context.SaveChangesAsync();

                var x = new CreateWithdrawCommandHandler(context);
                await x.Handle(new CreateWithdrawCommand { Amount = -20, AccountId = account.AccountId }, new CancellationToken());
            }

            //Assert that method throws NegativeAmountException
        }

        [TestMethod]
        public async Task Test_Transaction_Created()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankAppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Withdraw_exceed")
                .Options;

            int actual = 0;

            //Act
            using (var context = new BankAppDbContext(options))
            {
                var senderAccount = new Account
                {
                    Balance = 100,
                };
                var recieverAccount = new Account
                {
                    Balance = 0
                };

                context.AddRange(senderAccount, recieverAccount);
                await context.SaveChangesAsync();

                var withdrawCommand = new CreateWithdrawCommandHandler(context);
                await withdrawCommand.Handle(new CreateWithdrawCommand { Amount = 10, AccountId = senderAccount.AccountId }, new CancellationToken());
                var depositCommand = new CreateDepositCommandHandler(context);
                await depositCommand.Handle(new CreateDepositCommand { Amount = 10, AccountId = senderAccount.AccountId }, new CancellationToken());
                var transferCommand = new CreateTransferCommandHandler(context);
                await transferCommand.Handle(new CreateTransferCommand { Amount = 10, SenderAccountId = senderAccount.AccountId, RecieverAccountId = recieverAccount.AccountId }, new CancellationToken());

                actual = await context.Transactions.CountAsync();
            }

            //Assert that method creates 4 transfers
            int expected = 4;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Test_Interest_Applied()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankAppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Interest_Applied")
                .Options;
        }
    }
}
