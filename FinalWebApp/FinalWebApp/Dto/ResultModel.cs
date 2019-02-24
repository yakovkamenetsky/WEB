using FinalWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Dto
{
    public class ResultModel
    {
        public DateTime checkin { get; set; }
        public DateTime checkout { get; set; }
        public List<HotelModel> hotels { get; set; }
        public string place { get; set; }
        public int fromPrice { get; set; }
        public int toPrice { get; set; }
        public string CityName { get; set; }
        public List<CityAggregationModel> cityAggregation { get; set; }
        public List<PriceAggregationModel> PriceAggregation { get; set; }
    }

    public class CityAggregationModel
    {
        public string CityName { get; set; }
        public int Count { get; set; }
    }

    public class PriceAggregationModel
    {
        public int minPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}
