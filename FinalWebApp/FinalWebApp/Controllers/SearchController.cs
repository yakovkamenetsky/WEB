using System;
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
		private readonly MyContext _context;

		public SearchController(MyContext context)
		{
			_context = context;
		}
    
        public async Task<IActionResult> Index(string place, DateTime checkin, DateTime checkout)
        {
			var res = await _context.Hotel
				.Where(x => x.Name.Contains(place) || x.City.Name.Contains(place))
				.ToListAsync();
			

            return View("Results", res);
        }


		public async Task<IActionResult> HotelDetails(int id)
		{

			var res = _context.Hotel.Single(x => x.Id == id);

			return View("OfferView", res);
		}

	}
}