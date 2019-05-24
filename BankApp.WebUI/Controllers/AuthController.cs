using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BankApp.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
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
    }
}