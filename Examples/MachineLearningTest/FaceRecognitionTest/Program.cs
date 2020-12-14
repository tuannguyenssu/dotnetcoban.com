using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FaceRecognitionTest.DataModels;
using FaceRecognitionTest.Utils;
using Microsoft.ML;
using Microsoft.ML.Vision;

namespace FaceRecognitionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FaceTrain();
            FacePredict();
            Console.WriteLine("Press any key to finish");
            Console.ReadKey();
        }

        private static void FaceTrain()
        {
            var modelName = "FaceRecognitionModel";
            var rootImagePath = $"D:/FACEDATA/images";
            var modelPath = $"D:/FACEDATA/models/{modelName}.zip";

            var mlContext = new MLContext();
            mlContext.Log += (sender, args) => { Console.WriteLine(args.Message); };

            var allImages = FileUtils.LoadImagesFromDirectory(rootImagePath);

            var images = new List<FaceTrainData>();

            foreach (var image in allImages)
            {
                for (int i = 0; i < 2; i++)
                {
                    images.Add(new FaceTrainData(image.imagePath, image.label));
                }
            }

            var imageData = mlContext.Data.LoadFromEnumerable(images);

            var shuffledData = mlContext.Data.ShuffleRows(imageData);

            var labelColumnName = "LabelAsKey";

            var preprocessingPipeline = mlContext.Transforms.Conversion.MapValueToKey(labelColumnName, nameof(FaceTrainData.Label)).Append(mlContext.Transforms.LoadRawImageBytes(nameof(FacePredictionData.ImageDataBytes), rootImagePath, nameof(FaceTrainData.Path)));

            var preProcessedData = preprocessingPipeline
                .Fit(shuffledData)
                .Transform(shuffledData);

            var trainSplit = mlContext.Data.TrainTestSplit(preProcessedData);
            var trainSet = trainSplit.TrainSet;

            var options = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = nameof(FacePredictionData.ImageDataBytes),
                LabelColumnName = labelColumnName,
                Arch = ImageClassificationTrainer.Architecture.MobilenetV2,
                Epoch = 50,       //100
                BatchSize = 10,
                LearningRate = 0.01f,
                MetricsCallback = Console.WriteLine,
            };

            var pipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(options)
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(nameof(FacePredictionResult.PredictedLabel)));


            ITransformer trainedModel = pipeline.Fit(trainSet);
            mlContext.Model.Save(trainedModel, trainSet.Schema, modelPath);

        }

        private static void FacePredict()
        {
            var modelName = "FaceRecognitionModel";
            var rootImagePath = $"D:/FACEDATA/images";
            var modelPath = $"D:/FACEDATA/models/{modelName}.zip";

            var mlContext = new MLContext();

            var loadedModel = mlContext.Model.Load(modelPath, out _);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<FacePredictionData, FacePredictionResult>(loadedModel);

            var allImages = FileUtils.LoadImagesFromDirectory(rootImagePath);
            foreach (var image in allImages)
            {
                var bytes = File.ReadAllBytes(image.imagePath);
                var imageToPredict = new FacePredictionData(bytes);

                var prediction = predictionEngine.Predict(imageToPredict);

                Console.WriteLine($"Filename : [{Path.GetFileName(image.imagePath)}], " +
                                  $"Predicted Label : [{prediction.PredictedLabel}], " +
                                  $"Probability : [{prediction.Score.Max()}] "
                );
            }

        }
    }
}
