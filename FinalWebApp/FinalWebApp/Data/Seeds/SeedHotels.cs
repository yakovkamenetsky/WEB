using FinalWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Data.Seeds
{
    public class SeedHotels
    {
        public static List<Hotel> hotels = new List<Hotel>
        {
            new Hotel { Id=1, Name="Herods", CityId=1, Address="HaYarkon Street 155, Tel Aviv-Yafo, Israel", Capacity=150, Price=700 },
            new Hotel { Id=2, Name="Leonardo Boutique", CityId=1, Address="HaBarzel Street 17, Tel Aviv-Yafo, Israel", Capacity=100, Price=790 },
            new Hotel { Id=3, Name="Leonardo Beach", CityId=1, Address="HaYarkon 156, Tel Aviv-Yafo, Israel", Capacity=100, Price=900 },

            new Hotel { Id=4, Name="Leonardo Plaza", CityId=2, Address="King George Street 47, Jerusalem, Israel", Capacity=200, Price=1080 },
            new Hotel { Id=5, Name="Leonardo", CityId=2, Address="Saint George Street 9, Jerusalem", Capacity=200, Price=800 },
            new Hotel { Id=6, Name="Leonardo Boutique", CityId=2, Address="Monbaz 3, Jerusalem, Israel", Capacity=150, Price=1200 },

            new Hotel { Id=7, Name="Leonardo Club", CityId=3, Address="Banim 1, Tiberias, Israel", Capacity=200, Price=880 },
            new Hotel { Id=8, Name="Leonardo", CityId=3, Address="gdud barak 1 tiberias", Capacity=150, Price=750 },
            new Hotel { Id=9, Name="Leonardo Plaza", CityId=3, Address="Banim 1, Tiberias, Israel", Capacity=100, Price=1400 },
        
            new Hotel { Id=10, Name="Leonardo Privilege", CityId=4, Address="North Beach, Eilat, Israel", Capacity=150, Price=1450 },
            new Hotel { Id=11, Name="Herods Palace", CityId=4, Address="HaYam Street 6, Eilat, Israel", Capacity=100, Price=1030 },
            new Hotel { Id=12, Name="Herods Vitalis", CityId=4, Address="Ha-Yam Street 8, Eilat, Israel", Capacity=150, Price=1500 }
        };

        public static void InitialHotels(IServiceProvider serviceProvider)
        {
            using (var context = new MyContext(serviceProvider.GetRequiredService<DbContextOptions<MyContext>>()))
            {
                context.Database.OpenConnection();
                try
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT " + typeof(Hotel).Name + " ON");
                    context.SaveChanges();
                    context.Hotel.AddRange(hotels);
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
