using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WeOwn.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Modul.Models.MovieShow> MovieShow { get; set; }

    }
}
