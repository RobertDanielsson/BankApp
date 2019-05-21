﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankApp.Application.Accounts.Commands.CreateDeposit;
using BankApp.Application.Accounts.Commands.CreateTransfer;
using BankApp.Application.Accounts.Commands.CreateWithdraw;
using BankApp.Application.Accounts.Queries.GetAccountStatistics;
using BankApp.Application.Accounts.Queries.GetAccountTransferData;
using BankApp.Application.Customers.Commands.CreateCustomer;
using BankApp.Application.Customers.Commands.EditCustomer;
using BankApp.Application.Customers.Queries.GetCustomer;
using BankApp.Application.Customers.Queries.GetCustomerDetails;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CustomerController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(int customerId)
        {
            return View(await _mediator.Send(new GetCustomerDetailsQuery { CustomerId = customerId }));
        }

        public IActionResult CreateCustomer()
        {
            return View(new CreateCustomerCommand());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerCommand customer)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(customer);

                if (int.TryParse(result, out int customerId)) // Om Dbn sparar nya kontot och returnerar kundens id
                {
                    return RedirectToAction("index", new { customerId = customerId });
                }
                else
                {
                    ModelState.AddModelError("", result);
                }
            }

            return View(customer);
        }

        public async Task<IActionResult> EditCustomer(int customerId)
        {
            return View(await _mediator.Send(new GetCustomerQuery { CustomerId = customerId }));
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(EditCustomerCommand query)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(query);

                if (int.TryParse(result, out int customerId)) // Om Dbn uppdaterar kontot och returnerar kundens id
                {
                    return RedirectToAction("index", new { customerId = customerId });
                }
                else
                {
                    ModelState.AddModelError("", result);
                }
            }

            var customer = new Customer();
            return View(_mapper.Map(query, customer));
        }

        public async Task<IActionResult> ManageAccounts(int customerId)
        {
            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId }));
        }

        public async Task<IActionResult> AccountDetails(int accountId, int customerId)
        {
            var model = await _mediator.Send(new GetAccountStatisticsQuery { AccountId = accountId });
            model.CustomerId = customerId;
            return View(model);
        }
    }
}
