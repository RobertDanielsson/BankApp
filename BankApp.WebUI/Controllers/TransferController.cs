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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
    [Authorize(Policy = "Cashier")]
    public class TransferController : Controller
    {
        private readonly IMediator _mediator;

        public TransferController(IMediator mediator)
        {
            _mediator = mediator;
        }


        public async Task<IActionResult> GetAdditionalTransactions(GetAccountTransactionsQuery query)
        {
            var result = await _mediator.Send(query);
            return Json(result);
        }

        public async Task<IActionResult> Transfer(int customerId)
        {
            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId }));
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(CreateTransferCommand query)
        {
            if (!ModelState.IsValid)
            {
                return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
            }
            else
            {
                try
                {
                    await _mediator.Send(query);
                    TempData["successMessage"] = "Transfer sent successfully";
                    return RedirectToAction("Transfer", new { customerId = query.CustomerId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
                }
            }
        }

        public async Task<IActionResult> Deposit(int customerId)
        {
            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId }));
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(CreateDepositCommand query)
        {
            if (!ModelState.IsValid)
            {
                return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
            }
            else
            {
                try
                {
                    await _mediator.Send(query);
                    TempData["successMessage"] = "Deposit successful";
                    return RedirectToAction("Deposit", new { customerId = query.CustomerId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
                }
            }
        }

        public async Task<IActionResult> Withdraw(int customerId)
        {
            return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = customerId }));
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(CreateWithdrawCommand query)
        {

            if (!ModelState.IsValid)
            {
                return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
            }
            else
            {
                try
                {
                    await _mediator.Send(query);
                    TempData["successMessage"] = "Withdrawal successful";
                    return RedirectToAction("Withdraw", new { customerId = query.CustomerId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(await _mediator.Send(new GetAccountTransferDataQuery { CustomerId = query.CustomerId }));
                }
            }
        }
    }
}
