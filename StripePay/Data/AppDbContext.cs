using Microsoft.EntityFrameworkCore;
using StripePay.Data.Models;

namespace StripePay.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Operation> Operations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }
}
