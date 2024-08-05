using Microsoft.EntityFrameworkCore;

namespace DotNet8WebApi.CookiesAuthSample.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Tbl_User> Tbl_Users { get; set; }
}
