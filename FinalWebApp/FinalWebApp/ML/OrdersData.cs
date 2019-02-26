using FinalWebApp.Models;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.ML
{
	public enum Hobby
	{
		swimming,
		music,
		football
	};

	public enum Purpose
	{
	    solo,
		couple,
		friends,
		family,
		other
	};

	public class OrdersData
	{
		[Column("0")]
		public int Age { get; set; }
		[Column("1")]
		public Gender? Gender { get; set; }
		[Column("2")]
		public string Profession { get; set; }
		[Column("3")]
		public FamilyStatus FamilyStatus { get; set; }
		[Column("4")]
		public Hobby hobby { get; set; }
		[Column("5")]
		public Purpose purpose { get; set; }
		[Column("6", name:"Lable")]
		public int hotelId { get; set; }
	}
}
