using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalWebApp.Models;

namespace FinalWebApp.Models
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public DbSet<FinalWebApp.Models.City> City { get; set; }

        public DbSet<FinalWebApp.Models.Country> Country { get; set; }

        public DbSet<FinalWebApp.Models.Hotel> Hotel { get; set; }

        public DbSet<FinalWebApp.Models.Order> Order { get; set; }

        public DbSet<FinalWebApp.Models.User> User { get; set; }
    }
}
