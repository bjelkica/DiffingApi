using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DiffingApi.Models
{
    public class DiffContext : DbContext
    {
        public DbSet<DiffData> Diffs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DiffsDB;Trusted_Connection=True;ConnectRetryCount=0");
        }
    }
}
