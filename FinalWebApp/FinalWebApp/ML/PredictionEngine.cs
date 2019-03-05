using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using FinalWebApp.Models;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;

namespace FinalWebApp.ML
{
	public class Prediction
	{
		MLContext mlContext;
		static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "TrainingData.csv");
		static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "TestData.csv");
		static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Model.zip");
		static TextLoader _textLoader;
		PredictionEngine<OrdersData, HotelPrediction> predictionEngine;

		public Prediction()
		{
			mlContext = new MLContext(seed: 0);

			_textLoader = mlContext.Data.CreateTextLoader(new TextLoader.Arguments()
			{
				Separators = new[] { ',' },
				HasHeader = true,
				Column = new[]
				{
					new TextLoader.Column("Age", DataKind.Num, 0),
					new TextLoader.Column("Gender", DataKind.Num, 1),
					new TextLoader.Column("Profession", DataKind.Num, 2),
					new TextLoader.Column("FamilyStatus", DataKind.Num, 3),
					new TextLoader.Column("hobby", DataKind.Num, 4),
					new TextLoader.Column("purpose", DataKind.Num, 5),
					new TextLoader.Column("PriceForHotelId", DataKind.Num, 6)
				}
			});

			Engine();

			var startTimeSpan = TimeSpan.Zero;
			var periodTimeSpan = TimeSpan.FromMinutes(5);
			var timer = new System.Threading.Timer((e) =>
			{
				Engine();
			}, null, startTimeSpan, periodTimeSpan);

		}

		public void Engine()
		{
			var MLModel = Train(mlContext, _trainDataPath);
			Evaluate(mlContext, MLModel);
			ITransformer loadedModel;
			using (var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				loadedModel = mlContext.Model.Load(stream);
			}
			 predictionEngine = loadedModel.CreatePredictionEngine<OrdersData, HotelPrediction>(mlContext);
		}

		public static ITransformer Train(MLContext mlContext, string dataPath)
		{
			IDataView dataView = _textLoader.Read(dataPath);

			var pipeline = mlContext.Transforms.CopyColumns(inputColumnName: "PriceForHotelId", outputColumnName: "Label")
				.Append(mlContext.Transforms.Concatenate("Features", "Age", "Gender", "Profession", "FamilyStatus", "hobby", "purpose"))
				.Append(mlContext.Regression.Trainers.FastTree()); // Predict a target using a decision tree 

			var model = pipeline.Fit(dataView);
			SaveModelAsFile(mlContext, model);
			return model;
		}

		private static void Evaluate(MLContext mlContext, ITransformer model)
		{
			IDataView dataView = _textLoader.Read(_testDataPath);
			var predictions = model.Transform(dataView);
			var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
		}

		private static void TestSinglePrediction(MLContext mlContext)
		{
			var taxiTripSample = new OrdersData()
			{
				Age = 30,
				Gender = 1,
				Profession = 0,
				FamilyStatus = 3,
				hobby = 1,
				purpose = 1,
				PriceForHotelId = 0
			};
		}

		private static void SaveModelAsFile(MLContext mlContext, ITransformer model)
		{
			using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
				mlContext.Model.Save(model, fileStream);
		}
		public int GetPrediction(FeaturesModel model)
		{
			var userDetails = new OrdersData()
			{
				Age = model.Age,
				Gender = model.Gender,
				Profession = model.Profession,
				FamilyStatus = model.FamilyStatus,
				hobby = model.hobby,
				purpose = model.purpose,
				PriceForHotelId = 0 // To predict.
			};

			var prediction = predictionEngine.Predict(userDetails);
			return (int)prediction.PriceForHotelId;
		}
	}
}
