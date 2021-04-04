using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TransactionAPI.Models.Database
{
    public class User : IdentityUser
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>(); //Transactions database table reference
    }
}
