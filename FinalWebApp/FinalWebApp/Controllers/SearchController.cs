﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalWebApp.Controllers
{
    public class SearchController : Controller
    {
        public async Task<IActionResult> Index(string place, DateTime checkin, DateTime checkout)
        {

			//var optionsBuilder = new DbContextOptions<MyContext>();

			//var context = new MyContext(optionsBuilder);
			//var res = await context.Hotel
			//	.Where(x => x.Name.Contains(place) || x.City.Name.Contains(place))
			//	.ToListAsync();

			var results = new List<Hotel>
			{
				new Hotel()
				{
					Address = "hayarkon 55",
					Name = "hilton",
					Capacity = 10,
					Price = 500,
					Id = 1
				},

				new Hotel()
				{
					Address = "London",
					Name = "hilton",
					Capacity = 20,
					Price = 5000,
					Id = 2
				},

				new Hotel()
				{
					Address = "Eilat",
					Name = "hilton",
					Capacity = 22,
					Price = 540,
					Id = 3
				}
			};

			//ViewBag["res"] = res;

            return View("Results", results);
        }
    }
}