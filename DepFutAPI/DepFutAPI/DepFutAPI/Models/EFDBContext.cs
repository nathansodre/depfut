using DepFutAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace DepFutAPI.Models
{
    public class EFDBContext : DbContext
    {
        private readonly IMemoryCache _cache;

        public DbSet<Player> Players { get; set; }

        public EFDBContext(IMemoryCache cache)
        {
            _cache = cache;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
        }
    }
}
