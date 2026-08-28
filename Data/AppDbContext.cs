using Microsoft.EntityFrameworkCore;
using ASP_Library.Entities;

namespace ASP_Library.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
}
