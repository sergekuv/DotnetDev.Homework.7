using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class WebApiContext : DbContext
    {
        public WebApiContext(DbContextOptions<WebApiContext> options)
           : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
