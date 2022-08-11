using BankSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Data
{
    public class BankSystemDbContext : DbContext
    {
        public BankSystemDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}