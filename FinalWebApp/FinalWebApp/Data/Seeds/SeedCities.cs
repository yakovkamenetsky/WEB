using FinalWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Data.Seeds
{
    public class SeedCities
    {
        public static List<City> cities = new List<City>
        {
            new City { Id=1, Name="Tel-Aviv", CountryId=1 },
            new City { Id=2, Name="Jerusalem", CountryId=1 },
            new City { Id=3, Name="Tiberias", CountryId=1 },
            new City { Id=4, Name="Eilat", CountryId=1 }
        };

        public static void InitialCities(IServiceProvider serviceProvider)
        {
            using (var context = new MyContext(serviceProvider.GetRequiredService<DbContextOptions<MyContext>>()))
            {
                context.Database.OpenConnection();
                try
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT " + typeof(City).Name + " ON");
                    context.SaveChanges();
                    context.City.AddRange(cities);
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
