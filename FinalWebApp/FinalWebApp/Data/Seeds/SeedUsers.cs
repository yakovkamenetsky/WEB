using FinalWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Data.Seeds
{
    public class SeedUsers
    {
        public static List<User> users = new List<User>
        {
            new User { Id=1, Name="hemi", Email="a@gmail.com", Birthday=System.DateTime.Parse("05/02/1978"),
                       CityId =1, FamilyStatus=FamilyStatus.Married, Gender=Gender.Male,
                       Password="123456", Profession="manager", IsAdmin=true},
            new User { Id=2, Name="nati", Email="b@gmail.com", Birthday=System.DateTime.Parse("11/11/1985"),
                       CityId =2, FamilyStatus=FamilyStatus.MarriedPlus, Gender=Gender.Female,
                       Password="1234", Profession="professor", IsAdmin=false},
            new User { Id=3, Name="aaa", Email="d@gmail.com", Birthday=System.DateTime.Parse("04/10/1995"),
                       CityId =1, FamilyStatus=FamilyStatus.Other, Gender=Gender.Male,
                       Password="2345", Profession="nothing", IsAdmin=false},
            new User { Id=4, Name="bbb", Email="a@gmail.com", Birthday=System.DateTime.Parse("27/09/2000"),
                       CityId =4, FamilyStatus=FamilyStatus.Singel, Gender=Gender.Other,
                       Password="3456", Profession="", IsAdmin=false }
        };

        public static void InitialUsers(IServiceProvider serviceProvider)
        {
            using (var context = new MyContext(serviceProvider.GetRequiredService<DbContextOptions<MyContext>>()))
            {
                context.Database.OpenConnection();
                try
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [" + typeof(User).Name + "] ON");
                    context.SaveChanges();
                    context.User.AddRange(users);
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
