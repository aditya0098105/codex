using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace testingassignment2.Models
{
    public class AppDbContext : DbContext
    {
        // Constructor (ye EF Core ko options pass karega, jaise connection string)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Ye property tumhare Car model ke liye ek table banayegi (Cars)
        public DbSet<Car> Cars { get; set; }
    }
}
