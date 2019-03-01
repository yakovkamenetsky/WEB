using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.ML
{
	public class HotelPrediction
	{
		[ColumnName("Score")]
		public float PriceForHotelId { get; set; }
	}
}
