using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.WebUI.Models
{
    public class UpdateUserViewModel
    {
        [MinLength(3, ErrorMessage = "Min length is 3 characters")]
        [DataType(DataType.Password)]
        [MaxLength(50, ErrorMessage = "Max length is 50 characters")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }

        public string RoleId { get; set; }
        public List<SelectListItem> RoleList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = "Cashier",
                Value = "Cashier"
            },
            new SelectListItem
            {
                Text = "Admin",
                Value = "Admin"
            }
        };
    }
}
