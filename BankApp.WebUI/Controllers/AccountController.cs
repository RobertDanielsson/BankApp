using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Application.Accounts.Commands.CreateDeposit;
using BankApp.Application.Accounts.Commands.CreateTransfer;
using BankApp.Application.Accounts.Commands.CreateWithdraw;
using BankApp.Application.Accounts.Queries.GetAccountStatistics;
using BankApp.Application.Accounts.Queries.GetAccountTransactions;
using BankApp.Application.Accounts.Queries.GetAccountTransferData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Details(int accountId, int customerId)
        {
            var model = await _mediator.Send(new GetAccountStatisticsQuery { AccountId = accountId });
            model.CustomerId = customerId;
            return View(model);
        }

        public async Task<IActionResult> GetAdditionalTransactions(GetAccountTransactionsQuery query)
        {
            var result = await _mediator.Send(query);
            Console.WriteLine(query.AccountId + " " + query.Page);
            return Json(result);
        }

        public async Task<IActionResult> Transfer(int customerId)
        {
            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId }));
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(CreateTransferCommand query)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(query);

                if (result == "Transfer successful")
                {
                    TempData["successMessage"] = "Transfer sent successfully";
                    return RedirectToAction("Transfer", new { customerId = query.CustomerId });
                }
                else
                {
                    ModelState.AddModelError("", result);
                }
            }

            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
        }

        public async Task<IActionResult> Deposit(int customerId)
        {
            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId }));
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(CreateDepositCommand query)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(query);

                if (result == "Deposit successful")
                {
                    TempData["successMessage"] = "Deposit successful";
                    return RedirectToAction("Withdraw", new { customerId = query.CustomerId });
                }
                else
                {
                    ModelState.AddModelError("", result);
                }
            }

            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
        }

        public async Task<IActionResult> Withdraw(int customerId)
        {
            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId }));
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(CreateWithdrawCommand query)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(query);

                if (result == "Withdraw successful")
                {
                    TempData["successMessage"] = "Withdraw successful";
                    return RedirectToAction("Withdraw", new { customerId = query.CustomerId });
                }
                else
                {
                    ModelState.AddModelError("", result);
                }
            }

            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
        }
    }
}
