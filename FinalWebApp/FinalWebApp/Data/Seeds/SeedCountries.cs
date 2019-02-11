using FinalWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Data.Seeds
{
    public class SeedCountries
    {
        public static List<Country> countries = new List<Country>
        {
            new Country { Id=1, Name="Israel" }
        };

        public static void InitialCountries(IServiceProvider serviceProvider)
        {
            using (var context = new MyContext(serviceProvider.GetRequiredService<DbContextOptions<MyContext>>()))
            {
                context.Database.OpenConnection();
                try
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT " + typeof(Country).Name + " ON");
                    context.SaveChanges();
                    context.Country.AddRange(countries);
                    context.SaveChanges();
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
        }
    }
}
