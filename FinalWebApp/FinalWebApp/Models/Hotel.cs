using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public ICollection<Order> Orders { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }
	}

}
