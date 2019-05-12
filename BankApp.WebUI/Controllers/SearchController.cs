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

        public async Task<IActionResult> Index(string searchInput, int page=1)
        {
            var model = await _mediator.Send(new GetCustomersListQuery { SearchInput = searchInput, Page = page });
            model.SearchInput = searchInput;
            return View(model);
        }
    }
}
