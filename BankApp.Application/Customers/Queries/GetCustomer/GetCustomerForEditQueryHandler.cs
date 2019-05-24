using AutoMapper;
using BankApp.Application.Customers.Commands.EditCustomer;
using BankApp.Application.Interfaces;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankApp.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerForEditQueryHandler : IRequestHandler<GetCustomerForEditQuery, EditCustomerCommand>
    {
        private readonly IBankAppDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomerForEditQueryHandler(IBankAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EditCustomerCommand> Handle(GetCustomerForEditQuery request, CancellationToken cancellationToken)
        {
            var model = new EditCustomerCommand();
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId);
            return _mapper.Map(customer, model);
        }
    }
}
