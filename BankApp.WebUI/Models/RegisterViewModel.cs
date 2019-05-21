using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.WebUI.Models
{
    public class RegisterViewModel
    {
        //[Required(ErrorMessage = "Username is required")]
        //[MinLength(3, ErrorMessage = "Min length is 3 charcaters")]
        //[MaxLength(50, ErrorMessage = "Max length is 50 charcaters")]
        //public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(3, ErrorMessage = "Min length is 3 charcaters")]
        [MaxLength(50, ErrorMessage = "Max length is 50 charcaters")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Max length is 50 charcaters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength(50, ErrorMessage = "Max length is 50 charcaters")]
        public string LastName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
