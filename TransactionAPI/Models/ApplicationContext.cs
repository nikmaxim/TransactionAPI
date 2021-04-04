using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransactionAPI.Models.Database;

namespace TransactionAPI.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionTemp> TransactionsTemp { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
    }
}
