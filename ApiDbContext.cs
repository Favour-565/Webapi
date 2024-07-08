using Microsoft.EntityFrameworkCore;
using Webapi.Models;

namespace Webapi
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<VisitorInfo> VisitorInfos { get; set; }
    }
}
