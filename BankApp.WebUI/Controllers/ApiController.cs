using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BankApp.Application.Accounts.APIQueries.GetTransactions;
using BankApp.Application.Customers.Queries.GetCustomerDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BankApp.WebUI.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public ApiController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost("token")]
        public IActionResult GetToken(string customerId = "100001")
        {
            //Security key
            string securityKey = _configuration["securityKey"];
            //Symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            //Signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //Add claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Customer"),
                new Claim("CustomerId", customerId),
                new Claim("accountId", "5")
            };

            //Create token
            var token = new JwtSecurityToken(
                issuer: "robband.se",
                audience: "readers",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims);
            //Return token
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [Route("accounts/{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId, int limit = 20, int offset = 0)
        {
            return Ok(await _mediator.Send(new GetTransactionsQuery { AccountId = accountId, Limit = limit, Offset = offset }));
        }

        [Route("me")]
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
                return BadRequest("No CustomerId claim found");
            }
        }
    }
}