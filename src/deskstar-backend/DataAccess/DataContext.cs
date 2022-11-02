namespace Deskstar.DataAccess;
using Microsoft.EntityFrameworkCore;
public class DataContext : DbContext
{
    protected readonly IConfiguration? Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server with connection string from appsettings.json
        options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
    }

    //TODO: public DbSet<User> Users { get; set; }
}