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
using BankApp.Application.Customers.Commands.AddExistingAccount;
using BankApp.Application.Customers.Commands.AddNewAccount;
using BankApp.Application.Customers.Commands.CreateCustomer;
using BankApp.Application.Customers.Commands.EditCustomer;
using BankApp.Application.Customers.Queries.GetCustomer;
using BankApp.Application.Customers.Queries.GetCustomerDetails;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
    [Authorize(Policy = "Cashier")]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(int customerId)
        {
            try
            {
                return View(await _mediator.Send(new GetCustomerDetailsQuery { CustomerId = customerId }));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("index", "home");
            }
        }

        public IActionResult CreateCustomer()
        {
            return View(new CreateCustomerCommand());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerCommand customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _mediator.Send(customer);
                    return RedirectToAction("index", new { customerId = int.Parse(result) });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(customer);
                }
            }

            return View(customer);
        }

        public async Task<IActionResult> EditCustomer(GetCustomerForEditQuery query)
        {
            return View(await _mediator.Send(query));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditCustomer(EditCustomerCommand query, [FromServices] IMapper mapper)
        {
            if (!ModelState.IsValid)
            {
                var customer = new Customer();
                return View(mapper.Map(query, customer));
            }
            else
            {
                try
                {
                    await _mediator.Send(query);
                    TempData["successMessage"] = "Customer updated";
                    return RedirectToAction("index", new { customerId = query.CustomerId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(mapper.Map(query, new Customer()));
                }
            }
        }

        public async Task<IActionResult> ManageAccounts(int customerId)
        {
            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId }));
        }

        [HttpPost]
        public async Task<IActionResult> AddExistingAccount(AddExistingAccountCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageAccounts", await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = command.CustomerId }));
            }
            else
            {
                try
                {
                    await _mediator.Send(command);
                    TempData["successMessage"] = "Existing account successfully added";
                    return RedirectToAction("Index", "Customer", new { customerId = command.CustomerId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    TempData["errorMessage"] = ex.Message;
                    return View("ManageAccounts", await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = command.CustomerId }));
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewAccount(AddNewAccountCommand command)
        {
            try
            {
                await _mediator.Send(command);
                TempData["successMessage"] = "New account successfully created and added";
                return RedirectToAction("Index", "Customer", new { customerId = command.CustomerId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["errorMessage"] = ex.Message;
                return View("ManageAccounts", await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = command.CustomerId }));
            }
        }

        public async Task<IActionResult> AccountDetails(int accountId, int customerId)
        {
            var model = await _mediator.Send(new GetAccountStatisticsQuery { AccountId = accountId });
            model.CustomerId = customerId;
            return View(model);
        }
    }
}
