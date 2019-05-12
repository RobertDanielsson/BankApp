using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BankApp.Application.Customers.Queries.GetIndexStatistics;
using BankApp.Application.Customers.Commands.CreateCustomers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = await _mediator.Send(new GetIndexStatisticsQuery());
            return View(viewModel);
        }

        public async Task<IActionResult> CreateUsers()
        {
            await _mediator.Send(new CreateCustomersCommand());
            return RedirectToAction("index");
        }
    }
}
