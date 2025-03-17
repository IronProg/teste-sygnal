using Microsoft.EntityFrameworkCore;
using TesteSygnal.Models;

namespace TesteSygnal.Context;

public class TSDbContext : DbContext
{
    public TSDbContext(DbContextOptions<TSDbContext> dbOptions) : base(dbOptions) { }

    public DbSet<Order> Orders { get; set; }
}