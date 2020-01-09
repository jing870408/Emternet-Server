using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using SDMLNET.Models;
using static Microsoft.ML.DataOperationsCatalog;


namespace SDMLNET
{
    class Program
    {

        private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        private static string _DataPath => Path.Combine(_appPath, "..", "..", "..", "Data", "XwithLosses.csv");
        //D:\VS\SDMLNET\SDMLNET\Data

        private static string _modelPath => Path.Combine(_appPath, "..", "..", "..", "Models", "EmoModel2.zip");


        public static IDataView data;
        public static IDataView trainData;
        public static IDataView testData;


        private static MLContext mlContext;
        //private static PredictionEngine<SDMLData, EmoPredict> _predEngine;
      
       



        static void Main(string[] args)
        {
            mlContext = new MLContext();

            data = mlContext.Data.LoadFromTextFile<SDMLData>(_DataPath, hasHeader: true,separatorChar: ',');

            TrainTestData dataSplit = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
            trainData = dataSplit.TrainSet;
            testData = dataSplit.TestSet;

            var model = Train();

            Evaluate(mlContext, model,trainData.Schema);
        }
        public static ITransformer Train()
        {


            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Score", outputColumnName: "Label")
                .Append(mlContext.Transforms.Concatenate("Features", "Sleep", "Sleeptime", "AvgHeartRate", "SocialApp", "GameApp", "FunApp", "OtherApp", "Temperture", "Humidity", "Sunshine", "Airquality"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy("Label", "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var Model = pipeline.Fit(trainData);

            return Model;
        }

        
        private static void Evaluate(MLContext mlContext, ITransformer model, DataViewSchema trainDataview)
         {

            var predictions = model.Transform(testData);
            var Metrics = mlContext.MulticlassClassification.Evaluate(predictions);
            //SaveModelAsFile(mlContext, model , trainDataview);
            

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Multi-class Classification model - Test Data     ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       MicroAccuracy:    {Metrics.MicroAccuracy:0.###}");
            Console.WriteLine($"*       MacroAccuracy:    {Metrics.MacroAccuracy:0.###}");
            Console.WriteLine($"*       LogLoss:          {Metrics.LogLoss:#.###}");
            Console.WriteLine($"*       LogLossReduction: {Metrics.LogLossReduction:#.###}");
            Console.WriteLine($"*************************************************************************************************************");




        }
        private static void SaveModelAsFile(MLContext mlContext,  ITransformer model ,DataViewSchema trainingDataViewSchema)
        {
            mlContext.Model.Save(model, trainingDataViewSchema, _modelPath);
        }

    }
}
