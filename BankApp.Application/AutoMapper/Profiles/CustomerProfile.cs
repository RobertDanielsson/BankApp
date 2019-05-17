using AutoMapper;
using BankApp.Application.Customers.Commands.EditCustomer;
using BankApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.AutoMapper.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<EditCustomerCommand, Customer>();
        }
    }
}
