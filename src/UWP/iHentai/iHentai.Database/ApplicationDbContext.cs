using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace iHentai.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<InstanceModel> Instances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ihentai.db");
        }
    }
}
