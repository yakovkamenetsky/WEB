using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public  string Email { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public User User { get; set; }
		public int UserId { get; set; }
        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
	}
}
