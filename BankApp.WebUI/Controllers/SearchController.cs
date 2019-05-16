using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Application.Customers.Queries.GetCustomersListSearch;
using BankApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.WebUI.Controllers
{
    public class SearchController : Controller
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(GetCustomersListQuery test)
        {

            if (int.TryParse(test.SearchInput, out int customerId))
            {
                return RedirectToAction("Index", "Customer", new { customerId = customerId });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var model = await _mediator.Send(new GetCustomersListQuery { SearchInput = test.SearchInput, Page = test.Page });
                    return View(model);
                }
                else
                {
                    return NoContent();
                }
            }
        }
    }
}
