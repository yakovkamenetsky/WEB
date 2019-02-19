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
    }
}
