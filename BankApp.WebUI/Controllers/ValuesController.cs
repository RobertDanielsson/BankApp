using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BankApp.Application.Accounts.APIQueries.GetTransactions;
using BankApp.Application.Customers.Queries.GetCustomerDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.WebUI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("api/accounts/{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId, int limit = 20, int offset = 0)
        {
            return Ok(await _mediator.Send(new GetTransactionsQuery { AccountId = accountId, Limit = limit, Offset = offset }));
        }

        [Route("api/me")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Customer")]
        public async Task<IActionResult> GetCustomerDetails()
        {
            var customerId = User.Claims.FirstOrDefault(c => c.Type == "CustomerId").Value;

            if (customerId != null)
            {
                return Ok(await _mediator.Send(new GetCustomerDetailsQuery { CustomerId = int.Parse(customerId) }));
            }
            else
            {
                return BadRequest("No claim");
            }
        }
    }
}