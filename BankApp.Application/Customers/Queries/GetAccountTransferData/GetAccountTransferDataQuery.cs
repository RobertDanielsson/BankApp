using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Customers.Queries.GetAccountTransferData
{
    public class GetAccountTransferDataQuery : IRequest<GetAccountTransferDataViewModel>
    {
        public int CustomerId { get; set; }
    }
}
