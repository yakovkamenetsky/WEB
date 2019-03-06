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

	public enum Profession
	{
		Independent,
		Employee
	}

	public enum Purpose
	{
		solo,
		couple,
		friends,
		family,
		other
	}

	public enum FamilyStatus{
		Singel,
		Married,
		MarriedPlus,
		Other
	}

	public enum Hobby
	{
		Swimming,
		Music,
		Sports
	}

	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public string Email { get; set; }
		public string Password { get; set; }
		public ICollection<Order> Orders { get; set; }
		[DataType(DataType.Date)]
		public DateTime? Birthday { get; set; }
		public Gender? Gender { get; set; }
        public City City { get; set; }
		public int? CityId { get; set; }
		public string Profession { get; set; }
		public FamilyStatus FamilyStatus { get; set; }
		public bool IsAdmin { get; set; }
    }
}
