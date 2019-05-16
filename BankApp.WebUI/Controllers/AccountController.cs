using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Application.Transactions.Queries.GetAccountStatistics;
using BankApp.Application.Transactions.Queries.GetAccountTransactions;
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
        public async Task<IActionResult> Details(int accountId)
        {
            var model = await _mediator.Send(new GetAccountStatisticsQuery { AccountId = accountId });
            return View(model);
        }

        public async Task<IActionResult> GetAdditionalTransactions(GetAccountTransactionsQuery query)
        {
            var result = await _mediator.Send(query);
            Console.WriteLine(query.AccountId + " " + query.Page);
            return Json(result);
        }
    }
}
