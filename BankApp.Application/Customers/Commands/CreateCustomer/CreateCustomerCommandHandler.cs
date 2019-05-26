using BankApp.Application.Exceptions;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, string>
    {
        private readonly IBankAppDbContext _context;

        public CreateCustomerCommandHandler(IBankAppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Gender = request.Gender,
                Givenname = request.Givenname,
                Surname = request.Surname,
                Streetaddress = request.Streetaddress,
                City = request.City,
                Zipcode = request.Zipcode,
                Country = request.Country,
                CountryCode = request.CountryCode,
                Birthday = request.Birthday,
                NationalId = request.NationalId,
                Telephonecountrycode = request.Telephonecountrycode,
                Telephonenumber = request.Telephonenumber,
                Emailaddress = request.Emailaddress
            };

            var account = new Account
            {
                Frequency = "Monthly",
                Created = DateTime.Now,
                Balance = 0
            };

            var disp = new Disposition
            {
                Customer = customer,
                Account = account,
                Type = "Owner"
            };

            _context.Accounts.Add(account);
            _context.Customers.Add(customer);
            _context.Dispositions.Add(disp);

            if (await _context.SaveChangesAsync(cancellationToken) == 3)
            {
                return customer.CustomerId.ToString();
            }
            else
            {
                throw new ErrorSavingToDatabaseException();
            }
        }
    }
}
