using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FinalWebApp.Dto;
using FinalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var senitized = place.Trim().ToUpper();

            var res = await _context.Hotel
                .Where(x => x.Name.ToUpper().Contains(place) || x.City.Name.ToUpper().Contains(place))
                .Select(x => new HotelModel()
                {
                    Address = x.Address,
                    City = x.City.Name,
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Available = x.Capacity - x.Orders.Where(o => (o.CheckInDate <= checkin && o.CheckOutDate >= checkin)
                                                                || (o.CheckInDate >= checkin && o.CheckInDate <= checkout)
                                                                || (o.CheckInDate <= checkin && o.CheckOutDate >= checkout)
                                                            ).Count()
                }).ToListAsync();

            var model = new ResultModel()
            {
                hotels = res,
                checkin = checkin,
                checkout = checkout
            };
            
            return View("Results", model);
        }

        [HttpPost]
		public async Task<IActionResult> HotelDetails(HotelModel hotelModel)
		{
			var res = _context.Hotel.Include(x => x.City).ThenInclude(y => y.Country).Single(x => x.Id == hotelModel.Id);
			ViewBag.mapAddress = "https://maps.google.com/maps?q=" + UrlEncoder.Default.Encode(res.Address) + "&t=&z=13&ie=UTF8&iwloc=&output=embed";

			var coordinates = await GetHotelCoords(res.Address);
			ViewBag.coords = coordinates;

            hotelModel.Address = res.Address;
            hotelModel.Name = res.Name;
            hotelModel.Price = res.Price;

			return View("OfferView", hotelModel);
		}

		private async Task<string> GetHotelCoords(string address)
		{
			var client = new HttpClient();
			var url = "https://api.opencagedata.com/geocode/v1/json?q=" + UrlEncoder.Default.Encode(address) + "&key=55f3c34bb9a3424d96a72154deca11ea&no_annotations=1&language=en";
			var getResult =  await client.GetAsync(url);
			var serialize = await getResult.Content.ReadAsStringAsync();
			dynamic parsed = JObject.Parse(serialize);

			var lat = parsed.results[0].geometry.lat;
			var lng = parsed.results[0].geometry.lng;

			return (lat + "," + lng);
		}
	}
}