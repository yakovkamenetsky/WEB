using FinalWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.ML
{
	public class FeaturesModel
	{
		public int Age { get; set; }
		public Gender? Gender { get; set; }
		public string Profession { get; set; }
		public FamilyStatus FamilyStatus { get; set; }
		public Hobby hobby { get; set; }
		public Purpose purpose { get; set; }
		public DateTime checkin { get; set; }
		public DateTime checkout { get; set; }
	}
}
