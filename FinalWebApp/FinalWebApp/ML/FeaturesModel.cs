using FinalWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.ML
{
	public class FeaturesModel
	{
		public float Age { get; set; }
		public float Gender { get; set; }
		public float Profession { get; set; }
		public float FamilyStatus { get; set; }
		public float hobby { get; set; }
		public float purpose { get; set; }
		public DateTime checkin { get; set; }
		public DateTime checkout { get; set; }
	}
}
