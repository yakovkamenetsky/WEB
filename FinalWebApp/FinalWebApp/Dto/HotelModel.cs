using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Dto
{
    public class HotelModel
    {
		public bool isAiSearch { get; set; }
		public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Available { get; set; }
        public float Price { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
		public int MyProperty { get; set; }
		public float userAge { get; set; }
		public float userGender { get; set; }
		public float userFamilyStatus { get; set; }
		public float userProfession { get; set; }
		public float userHobby { get; set; }
		public float userPurpose { get; set; }
	}
}
