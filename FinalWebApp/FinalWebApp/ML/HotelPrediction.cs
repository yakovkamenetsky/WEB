using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.ML
{
	public class HotelPrediction
	{
		[ColumnName("hotel id")]
		public int PredictedHotel { get; set; }
	}
}
