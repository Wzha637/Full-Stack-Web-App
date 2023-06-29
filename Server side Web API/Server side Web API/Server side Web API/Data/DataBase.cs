using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using A1.Model;

namespace A1.Data
{
    public class DataBase : DbContext
    {
        public DataBase()
        {

        }

        public DataBase(DbContextOptions<DataBase> options) : base(options) { }
        public DbSet<Staff> AllStaff { get; set; }
        public DbSet<Products> AllProducts { get; set; }

        public DbSet<Comments> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MyDatabase.sqlite");
        }
    }
}
