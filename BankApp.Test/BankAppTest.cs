using BankApp.Application.Accounts.Commands.CreateDeposit;
using BankApp.Application.Accounts.Commands.CreateInterest;
using BankApp.Application.Accounts.Commands.CreateTransfer;
using BankApp.Application.Accounts.Commands.CreateWithdraw;
using BankApp.Application.Exceptions;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using BankApp.Infrastructure;
using BankApp.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
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
        public async Task Test_Interest_No_Prior_Applied()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankAppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Interest_No_Prior_Applied")
                .Options;

            var systemClockCreateAccount = Substitute.For<IDateTime>();
            systemClockCreateAccount.GetCurrentTime().Returns(new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Local));
            var systemClockCheckInterest = Substitute.For<IDateTime>();
            systemClockCheckInterest.GetCurrentTime().Returns(new DateTime(2010, 2, 1, 0, 0, 0, DateTimeKind.Local));

            decimal actual = 0;

            //Act
            using (var context = new BankAppDbContext(options))
            {
                var account = new Account
                {
                    Balance = 100,
                    Created = systemClockCreateAccount.GetCurrentTime()
                };

                context.Accounts.Add(account);
                await context.SaveChangesAsync();

                var x = new CreateInterestCommandHandler(context);
                await x.Handle(new CreateInterestCommand { AccountId = account.AccountId, DateTimeProvider = systemClockCheckInterest }, new CancellationToken());
                actual = account.Balance;
            }

            // Assert equation: 2.3% interest applied daily since last interest credit OR
            // - since account creation. This test will apply since account creation.
            // ((2.3% / 365) * account.Balance) * number of days since last credit

            decimal expected = 100.20m;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Test_Interest_With_Prior_Applied()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankAppDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Interest_With_Prior_Applied")
                .Options;

            var accountCreatedClock = Substitute.For<IDateTime>();
            accountCreatedClock.GetCurrentTime().Returns(new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Local));

            var systemClockCheckInterestInitial = Substitute.For<IDateTime>();
            systemClockCheckInterestInitial.GetCurrentTime().Returns(new DateTime(2010, 2, 1, 0, 0, 0, DateTimeKind.Local));

            var systemClockCheckInterest = Substitute.For<IDateTime>();
            systemClockCheckInterest.GetCurrentTime().Returns(new DateTime(2010, 4, 1, 0, 0, 0, DateTimeKind.Local));

            decimal actual = 0;

            //Act
            using (var context = new BankAppDbContext(options))
            {
                var account = new Account
                {
                    Balance = 100,
                    Created = accountCreatedClock.GetCurrentTime()
                };

                context.Accounts.Add(account);
                await context.SaveChangesAsync();

                var x = new CreateInterestCommandHandler(context);

                await x.Handle(new CreateInterestCommand { AccountId = account.AccountId, DateTimeProvider = systemClockCheckInterestInitial }, new CancellationToken());
                await x.Handle(new CreateInterestCommand { AccountId = account.AccountId, DateTimeProvider = systemClockCheckInterest }, new CancellationToken());
                actual = account.Balance;
            }

            // Assert equation: 2.3% interest applied daily since last interest credit. 
            // This check creates an applies an initial interest credit, followed by a second one.
            // First one will create an intial once since account creation.
            // Second one will apply days since first interest was applied.
            // ((2.3% / 365) * account.Balance) * number of days since last credit
            decimal expected = 100.57m;
            Assert.AreEqual(expected, actual);
        }
    }
}
