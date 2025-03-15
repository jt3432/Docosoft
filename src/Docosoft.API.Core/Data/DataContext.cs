using Docosoft.API.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Docosoft.API.Core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<DocosoftUser> DocosoftUsers { get; set; }
    }
}
