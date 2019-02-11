using FinalWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Data.Seeds
{
    public class SeedOrders
    {
        public static List<Order> orders = new List<Order>
        {
            new Order { Id=1, Name="aaa", Email="a@gmail.com", UserId=1, HotelId=1,
                CheckInDate =System.DateTime.Parse("04/10/2019"),
                CheckOutDate =System.DateTime.Parse("10/10/2019") },
            new Order { Id=2, Name="bbb", Email="b@gmail.com", UserId=2, HotelId=1,
                CheckInDate =System.DateTime.Parse("08/10/2019"),
                CheckOutDate =System.DateTime.Parse("12/10/2019") },
            new Order { Id=3, Name="ccc", Email="c@gmail.com", UserId=3, HotelId=1,
                CheckInDate =System.DateTime.Parse("12/10/2019"),
                CheckOutDate =System.DateTime.Parse("13/10/2019") }
        };

        public static void InitialOrders(IServiceProvider serviceProvider)
        {
            using (var context = new MyContext(serviceProvider.GetRequiredService<DbContextOptions<MyContext>>()))
            {
                context.Database.OpenConnection();
                try
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [" + typeof(Order).Name + "] ON");
                    context.SaveChanges();
                    context.Order.AddRange(orders);
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
