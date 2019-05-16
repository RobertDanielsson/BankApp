using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Application.Customers.Commands.CreateDeposit;
using BankApp.Application.Customers.Commands.CreateTransfer;
using BankApp.Application.Customers.Commands.CreateWithdraw;
using BankApp.Application.Customers.Queries.GetAccountTransferData;
using BankApp.Application.Customers.Queries.GetCustomerDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
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
            var model = await _mediator.Send(new GetCustomerDetailsQuery { CustomerId = customerId });
            return View(model);
        }

        public async Task<IActionResult> Transfer(int customerId)
        {
            var value = TempData["model"];

            var model = await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId });
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(CreateTransferCommand model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(model);
                if (result == "Transfer successful")
                {
                    TempData["successMessage"] = "Transfer sent successfully";
                    return RedirectToAction("Transfer", new { customerId = model.CustomerId });
                }
                else
                {
                    TempData["errorMessage"] = result;
                    return RedirectToAction("Transfer", new { customerId = model.CustomerId });
                }
            }
            else
            {
                TempData["errorMessage"] = "Transfer failed, check your inputs";
                return RedirectToAction("Transfer", new { customerId = model.CustomerId });
            }

        }

        public async Task<IActionResult> Deposit(int customerId)
        {
            var model = await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId });
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(CreateDepositCommand model)
        {
            var result = await _mediator.Send(model);
            if (result == "Deposit successful")
            {
                TempData["successMessage"] = "Deposit successfull";
                return RedirectToAction("Deposit", new { customerId = model.CustomerId });
            }
            else
            {
                TempData["errorMessage"] = result;
                return RedirectToAction("Deposit", new { customerId = model.CustomerId });
            }
        }

        public async Task<IActionResult> Withdraw(int customerId)
        {
            var model = await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId });
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(CreateWithdrawCommand model)
        {
            var result = await _mediator.Send(model);

            if(result == "Withdraw successful")
            {
                TempData["successMessage"] = "Withdraw successfull";
                return RedirectToAction("Withdraw", new { customerId = model.CustomerId });
            }
            else
            {
                TempData["errorMessage"] = result;
                return RedirectToAction("Withdraw", new { customerId = model.CustomerId });
            }
        }

    }
}
