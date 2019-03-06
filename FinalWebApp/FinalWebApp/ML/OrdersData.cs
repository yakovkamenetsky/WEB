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


	public enum Profession
	{
		independent,
		employee
	};

	public class OrdersData
	{
		[Column("0")]
		public float Age { get; set; }
		[Column("1")]
		public float Gender { get; set; }
		[Column("2")]
		public float Profession { get; set; }
		[Column("3")]
		public float FamilyStatus { get; set; }
		[Column("4")]
		public float hobby { get; set; }
		[Column("5")]
		public float purpose { get; set; }
		[Column("6")]
		public float PriceForHotelId { get; set; }
	}
}
