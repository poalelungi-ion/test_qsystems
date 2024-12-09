using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace In_MethodAPI
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<RequestLog> RequestLogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
    }

}
