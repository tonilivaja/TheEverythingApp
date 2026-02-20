using Microsoft.EntityFrameworkCore;
using TheEverythingApp.Application.Features.Auth;

namespace TheEverythingApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}
