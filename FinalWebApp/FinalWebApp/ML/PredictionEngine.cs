using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FinalWebApp.Models;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;

namespace FinalWebApp.ML
{
	public class PredictionEngine
	{

		static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory,  "TrainingData1.csv");
		static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory,  "TestData1.csv");
		static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Model.zip");
		static TextLoader _textLoader;

		public PredictionEngine()
		{
			
		}

		private static void Init()
		{
			MLContext mlContext = new MLContext(seed: 0);

			_textLoader = mlContext.Data.CreateTextLoader(new TextLoader.Arguments()
			{
				Separators = new[] { ',' },
				HasHeader = true,
				Column = new[]
				{
					new TextLoader.Column("Age", DataKind.Num, 0),
					new TextLoader.Column("Gender", DataKind.Text, 1),
					new TextLoader.Column("Profession", DataKind.Text, 2),
					new TextLoader.Column("FamilyStatus", DataKind.Text, 3),
					new TextLoader.Column("hobby", DataKind.Text, 4),
					new TextLoader.Column("purpose", DataKind.Text, 5),
					new TextLoader.Column("hotelId", DataKind.Num, 6)
				}
			}
			);

			var model = Train(mlContext, _trainDataPath);
			Evaluate(mlContext, model);
			TestSinglePrediction(mlContext);
		}

		public static ITransformer Train(MLContext mlContext, string dataPath)
		{
			IDataView dataView = _textLoader.Read(dataPath);

			var pipeline = mlContext.Transforms.CopyColumns(inputColumnName: "hotelId", outputColumnName: "Label")
				.Append(mlContext.Transforms.Categorical.OneHotEncoding("Gender"))
				.Append(mlContext.Transforms.Categorical.OneHotEncoding("Profession"))
				.Append(mlContext.Transforms.Categorical.OneHotEncoding("FamilyStatus"))
				.Append(mlContext.Transforms.Categorical.OneHotEncoding("hobby"))
				.Append(mlContext.Transforms.Categorical.OneHotEncoding("purpose"))
				.Append(mlContext.Transforms.Concatenate("Features", "Age", "Gender", "Profession", "FamilyStatus", "hobby", "purpose"))
				.Append(mlContext.Regression.Trainers.FastTree());

			var model = pipeline.Fit(dataView);
			SaveModelAsFile(mlContext, model);
			return model;
		}

		private static void Evaluate(MLContext mlContext, ITransformer model)
		{
			IDataView dataView = _textLoader.Read(_testDataPath);
			var predictions = model.Transform(dataView);
			var metrics = mlContext.Regression.Evaluate(predictions, "Label", "hotelId");
		}

		private static void TestSinglePrediction(MLContext mlContext)
		{
			ITransformer loadedModel;
			using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				loadedModel = mlContext.Model.Load(stream);
			}
			var predictionFunction = loadedModel.CreatePredictionEngine<OrdersData, HotelPrediction>(mlContext);

			var taxiTripSample = new OrdersData()
			{
				Age = 20,
				Gender = "Male",
				Profession = "employee",
				FamilyStatus = "MarriedPlus",
				hobby = "football",
				purpose = "friends",
				hotelId = 0 // To predict. Actual/Observed = 15.5
			};

			var prediction = predictionFunction.Predict(taxiTripSample);

		}

		private static void SaveModelAsFile(MLContext mlContext, ITransformer model)
		{
			using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
				mlContext.Model.Save(model, fileStream);
		}
		public int GetPrediction(FeaturesModel model)
		{
			Init();
			return 1;
		}

	}
}
