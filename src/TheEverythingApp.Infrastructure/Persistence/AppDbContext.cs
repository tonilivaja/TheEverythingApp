using Microsoft.EntityFrameworkCore;

namespace TheEverythingApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // To do
    }
}
