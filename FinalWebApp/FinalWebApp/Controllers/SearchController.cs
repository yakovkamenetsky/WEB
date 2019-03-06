using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FinalWebApp.Dto;
using FinalWebApp.ML;
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
		private readonly Prediction _predictionEngine;

		public SearchController(MyContext context, Prediction predictionEngine)
		{
			_context = context;
			_predictionEngine = predictionEngine;
		}

		[HttpPost]
		public async Task<IActionResult> Predict(FeaturesModel featuresModel)
		{
			var hotelPrice = _predictionEngine.GetPrediction(featuresModel);
			var hotelModel = _context.Hotel
				.OrderBy(x => Math.Abs(x.Price - hotelPrice))
				.Take(2)
				.Select(x => new HotelModel()
				{
					isAiSearch = true,
					Address = x.Address,
					City = x.City.Name,
					Id = x.Id,
					Name = x.Name,
					Price = x.Price,
					Available = x.Capacity - x.Orders.Where(o => (o.CheckInDate <= featuresModel.checkin && o.CheckOutDate >= featuresModel.checkin)
															|| (o.CheckInDate >= featuresModel.checkin && o.CheckInDate <= featuresModel.checkout)
															|| (o.CheckInDate <= featuresModel.checkin && o.CheckOutDate >= featuresModel.checkout)
															).Count()
			});
			var resultModel = new ResultModel()
			{
				hotels = await hotelModel.ToListAsync(),
				checkin = featuresModel.checkin,
				checkout = featuresModel.checkout,
				isAiSearch = true, 
				Features = featuresModel
			};

			return View("Results", resultModel);
		}


		public async Task<IActionResult> Index(ResultModel resultModel)
        {
            var senitized = resultModel.place.Trim().ToUpper();

            var res = _context.Hotel
                .Where(x => x.Name.ToUpper().Contains(resultModel.place) 
                        || x.City.Name.ToUpper().Contains(resultModel.place) 
				        || x.City.Country.Name.ToUpper().Contains(resultModel.place))
                .Select(x => new HotelModel()
                {
                    Address = x.Address,
                    City = x.City.Name,
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Available = x.Capacity - x.Orders.Where(o => (o.CheckInDate <= resultModel.checkin && o.CheckOutDate >= resultModel.checkin)
                                                                || (o.CheckInDate >= resultModel.checkin && o.CheckInDate <= resultModel.checkout)
                                                                || (o.CheckInDate <= resultModel.checkin && o.CheckOutDate >= resultModel.checkout)
                                                            ).Count()
                });

            if (resultModel.toPrice != 0)
            {
                res = res.Where(x => x.Price <= resultModel.toPrice);
            }

            if (!string.IsNullOrEmpty(resultModel.CityName))
            {
                res = res.Where(x => x.City == resultModel.CityName);
            }

            res = res.OrderBy(x => x.Price);

            resultModel.hotels = await res.ToListAsync();

            if (res.Count() > 0)
                await AddAggregations(res, resultModel);

            return View("Results", resultModel);
        }

        private async Task AddAggregations(IQueryable<HotelModel> query, ResultModel resultModel)
        {
            if (string.IsNullOrEmpty(resultModel.CityName))
            {
                resultModel.cityAggregation = await query.GroupBy(x => x.City).Select(x => new CityAggregationModel()
                {
                    CityName = x.Key,
                    Count = x.Count()
                }).ToListAsync();
            }

            var agg = new List<PriceAggregationModel>();

            for (int i = 1; i*500 < query.Max(x => x.Price); i++)
            {
                agg.Add(new PriceAggregationModel()
                {
                    minPrice = (i - 1) * 500,
                    MaxPrice = i * 500
                });
            }

            agg.Add(new PriceAggregationModel()
            {
                minPrice = agg.Max(x => x.MaxPrice),
                MaxPrice = query.Max(x => x.Price)
            });

            resultModel.PriceAggregation = agg;
        }

        [HttpPost]
		public async Task<IActionResult> HotelDetails(HotelModel hotelModel)
		{
			var res = _context.Hotel.Include(x => x.City).ThenInclude(y => y.Country).Single(x => x.Id == hotelModel.Id);
			ViewBag.mapAddress = "https://maps.google.com/maps?q=" + UrlEncoder.Default.Encode(res.Address) + "&t=&z=13&ie=UTF8&iwloc=&output=embed";

            string coordinates = null;

            try
            {
                coordinates = await GetHotelCoords(res.Address);
            }
            catch(Exception e)
            {
                Console.WriteLine("Cant recieve coordinates");
            }
                
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