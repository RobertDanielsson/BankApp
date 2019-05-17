using BankApp.Application.DTO;
using BankApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Application.Accounts.Queries.GetAccountTransferData
{
    public class GetAccountTransferDataViewModel
    {
        public List<SelectListItem> Accounts { get; set; }
        public CustomerDTO Customer { get; set; }

    }
}
