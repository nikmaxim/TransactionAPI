using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionAPI.Models.JsonRequest
{
    public class RegisterRequestJson
    {
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
