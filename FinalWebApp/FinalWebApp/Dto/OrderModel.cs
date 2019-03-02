using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Dto
{
	public class OrderModel
	{
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }
		public int HotelId { get; set; }
		public bool isAi { get; set; }
		public float userAge { get; set; }
		public float userGender { get; set; }
		public float userFamilyStatus { get; set; }
		public float userProfession { get; set; }
		public float userHobby { get; set; }
		public float userPurpose { get; set; }
	}
}
