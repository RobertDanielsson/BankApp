﻿using AutoMapper;
using BankApp.Application.Customers.Commands.CreateCustomer;
using BankApp.Application.Exceptions;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Commands.EditCustomer
{
    public class EditCustomerCommandHandler : IRequestHandler<EditCustomerCommand, Unit>
    {
        private readonly IBankAppDbContext _context;
        private readonly IMapper _mapper;

        public EditCustomerCommandHandler(IBankAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer();
            customer = _mapper.Map(request, customer);
            _context.Update(customer);

            if (await _context.SaveChangesAsync(cancellationToken) == 1)
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
