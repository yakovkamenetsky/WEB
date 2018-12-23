using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Models
{
	public enum Gender
	{
		Male,
		Female,
		Other
	}

	public enum FamilyStatus{
		Singel,
		Married,
		MarriedPlus,
		Other
	}

	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public ICollection<Order> Orders { get; set; }
		[DataType(DataType.Date)]
		public DateTime? Birthday { get; set; }
		public Gender? Gender { get; set; }
		public string CityName { get; set; }
		public string ContryName { get; set; }
		public string Profession { get; set; }
		public FamilyStatus FamilyStatus { get; set; }
	}
}
