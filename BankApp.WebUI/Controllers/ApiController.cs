using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Application.Accounts.APIQueries.GetTransactions;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
    public class ApiController : Controller
    {
        private readonly IMediator _mediator;

        public ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("api/accounts/{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId, int limit = 20, int offset = 0)
        {
            return Ok(await _mediator.Send(new GetTransactionsQuery { AccountId = accountId, Limit = limit, Offset = offset }));
        }
    }
}
