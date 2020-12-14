using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ImageClassificationTest.Common;
using ImageClassificationTest.DataModels;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;

namespace ImageClassificationTest
{
    class Program
    {
        static void Main()
        {

            //TestTrain();
            //TestPredict();

            FaceTrain();
            FacePredict();
            Console.WriteLine("Press any key to finish");
            Console.ReadKey();
        }

        private static void FaceTrain()
        {
            var userId = "tuannv";
            var rootImagePath = $"D:/FACEDATA/images";
            var imagePath = $"{rootImagePath}/{userId}";
            var modelPath = $"D:/FACEDATA/models/{userId}.zip";

            var testPath = $"{rootImagePath}/nelka";

            var mlContext = new MLContext(seed: 1);
            mlContext.Log += FilterMLContextLog;

            var positiveImagePaths = Directory
                .GetFiles(imagePath, "*", SearchOption.AllDirectories)
                .Where(x => Path.GetExtension(x) == ".jpg" || Path.GetExtension(x) == ".png");

            var negativeImagePaths = Directory
                .GetFiles(testPath, "*", SearchOption.AllDirectories)
                .Where(x => Path.GetExtension(x) == ".jpg" || Path.GetExtension(x) == ".png");


            var images = new List<ImageData>();

            foreach (var path in negativeImagePaths)
            {
                for (int i = 0; i < 10; i++)
                {
                    images.Add(new ImageData(path, "nelka"));
                }
            }

            foreach (var path in positiveImagePaths)
            {
                for (int i = 0; i < 10; i++)
                {
                    images.Add(new ImageData(path, userId));
                }
            }

            var imageData = mlContext.Data.LoadFromEnumerable(images);

            var shuffledData = mlContext.Data.ShuffleRows(imageData);

            var featureColumnName = "Image";
            var labelColumnName = "LabelAsKey";

            var preprocessingPipeline = mlContext.Transforms.Conversion.MapValueToKey(labelColumnName, "Label").Append(mlContext.Transforms.LoadRawImageBytes(featureColumnName, rootImagePath, "ImagePath"));

            var preProcessedData = preprocessingPipeline
                .Fit(shuffledData)
                .Transform(shuffledData);

            var trainSplit = mlContext.Data.TrainTestSplit(preProcessedData);
            var trainSet = trainSplit.TrainSet;

            var options = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = featureColumnName,
                LabelColumnName = labelColumnName,
                Arch = ImageClassificationTrainer.Architecture.MobilenetV2,
                Epoch = 50,       //100
                BatchSize = 10,
                LearningRate = 0.01f,
                MetricsCallback = Console.WriteLine
            };

            var pipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(options).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));


            ITransformer trainedModel = pipeline.Fit(trainSet);
            mlContext.Model.Save(trainedModel, trainSet.Schema, modelPath);

        }

        private static void FacePredict()
        {
            var userId = "tuannv";
            var testImagePath = $"D:/FACEDATA/images/nelka";
            var modelPath = $"D:/FACEDATA/models/{userId}.zip";

            var mlContext = new MLContext(seed: 1);

            var loadedModel = mlContext.Model.Load(modelPath, out _);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<FaceImageData, FaceImagePrediction>(loadedModel);

            var imagesPath = Directory
                .GetFiles(testImagePath, "*", SearchOption.AllDirectories)
                .Where(x => Path.GetExtension(x) == ".jpg" || Path.GetExtension(x) == ".png");
            foreach (var path in imagesPath)
            {
                var bytes = File.ReadAllBytes(path);
                var imageToPredict = new FaceImageData(bytes, userId);

                var prediction = predictionEngine.Predict(imageToPredict);

                Console.WriteLine($"Image Filename : [{Path.GetFileName(path)}], " +
                                  $"Predicted Label : [{prediction.PredictedLabel}], " +
                                  $"Probability : [{prediction.Score.Max()}] "
                );
            }

        }

        private static void TestPredict()
        {
            const string assetsRelativePath = @"Assets";
            var assetsPath = GetAbsolutePath(assetsRelativePath);

            var imagesFolderPathForPredictions = Path.Combine(assetsPath, "inputs", "images-for-predictions");

            var imageClassifierModelZipFilePath = Path.Combine(assetsPath, "inputs", "MLNETModel", "imageClassifier.zip");

            try
            {
                var mlContext = new MLContext(seed: 1);

                Console.WriteLine($"Loading model from: {imageClassifierModelZipFilePath}");

                // Load the model
                var loadedModel = mlContext.Model.Load(imageClassifierModelZipFilePath, out var modelInputSchema);

                // Create prediction engine to try a single prediction (input = ImageData, output = ImagePrediction)
                var predictionEngine = mlContext.Model.CreatePredictionEngine<InMemoryImageData, ImagePrediction>(loadedModel);

                //Predict the first image in the folder
                var imagesToPredict = FileUtils.LoadInMemoryImagesFromDirectory(imagesFolderPathForPredictions, false);

                var imageToPredict = imagesToPredict.First();

                // Measure #1 prediction execution time.
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var prediction = predictionEngine.Predict(imageToPredict);

                // Stop measuring time.
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("First Prediction took: " + elapsedMs + "mlSecs");

                // Measure #2 prediction execution time.
                var watch2 = System.Diagnostics.Stopwatch.StartNew();

                var prediction2 = predictionEngine.Predict(imageToPredict);

                // Stop measuring time.
                watch2.Stop();
                var elapsedMs2 = watch2.ElapsedMilliseconds;
                Console.WriteLine("Second Prediction took: " + elapsedMs2 + "mlSecs");

                // Get the highest score and its index
                var maxScore = prediction.Score.Max();

                ////////
                // Double-check using the index
                var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
                VBuffer<ReadOnlyMemory<char>> keys = default;
                predictionEngine.OutputSchema[3].GetKeyValues(ref keys);
                var keysArray = keys.DenseValues().ToArray();
                var predictedLabelString = keysArray[maxIndex];
                ////////

                Console.WriteLine($"Image Filename : [{imageToPredict.ImageFileName}], " +
                                  $"Predicted Label : [{prediction.PredictedLabel}], " +
                                  $"Probability : [{maxScore}] "
                                  );

                //Predict all images in the folder
                //
                Console.WriteLine("");
                Console.WriteLine("Predicting several images...");

                foreach (var currentImageToPredict in imagesToPredict)
                {
                    var currentPrediction = predictionEngine.Predict(currentImageToPredict);

                    Console.WriteLine(
                        $"Image Filename : [{currentImageToPredict.ImageFileName}], " +
                        $"Predicted Label : [{currentPrediction.PredictedLabel}], " +
                        $"Probability : [{currentPrediction.Score.Max()}]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private static void TestTrain()
        {
            const string assetsRelativePath = @"Assets";
            string assetsPath = GetAbsolutePath(assetsRelativePath);

            string outputMlNetModelFilePath = Path.Combine(assetsPath, "outputs", "imageClassifier.zip");
            string imagesFolderPathForPredictions = Path.Combine(assetsPath, "inputs", "test-images");

            string imagesDownloadFolderPath = Path.Combine(assetsPath, "inputs", "images");

            // 1. Download the image set and unzip
            string finalImagesFolderName = DownloadImageSet(imagesDownloadFolderPath);
            string fullImagesetFolderPath = Path.Combine(imagesDownloadFolderPath, finalImagesFolderName);

            var mlContext = new MLContext(seed: 1);

            // Specify MLContext Filter to only show feedback log/traces about ImageClassification
            // This is not needed for feedback output if using the explicit MetricsCallback parameter
            mlContext.Log += FilterMLContextLog;

            // 2. Load the initial full image-set into an IDataView and shuffle so it'll be better balanced
            IEnumerable<ImageData> images = LoadImagesFromDirectory(folder: fullImagesetFolderPath, useFolderNameAsLabel: true);
            IDataView fullImagesDataset = mlContext.Data.LoadFromEnumerable(images);
            IDataView shuffledFullImageFilePathsDataset = mlContext.Data.ShuffleRows(fullImagesDataset);

            // 3. Load Images with in-memory type within the IDataView and Transform Labels to Keys (Categorical)
            IDataView shuffledFullImagesDataset = mlContext.Transforms.Conversion.
                    MapValueToKey(outputColumnName: "LabelAsKey", inputColumnName: "Label", keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
                .Append(mlContext.Transforms.LoadRawImageBytes(
                                                outputColumnName: "Image",
                                                imageFolder: fullImagesetFolderPath,
                                                inputColumnName: "ImagePath"))
                .Fit(shuffledFullImageFilePathsDataset)
                .Transform(shuffledFullImageFilePathsDataset);

            // 4. Split the data 80:20 into train and test sets, train and evaluate.
            var trainTestData = mlContext.Data.TrainTestSplit(shuffledFullImagesDataset, testFraction: 0.2);
            IDataView trainDataView = trainTestData.TrainSet;
            IDataView testDataView = trainTestData.TestSet;

            // 5. Define the model's training pipeline using DNN default values
            //
            //var pipeline = mlContext.MulticlassClassification.Trainers
            //        .ImageClassification(featureColumnName: "Image",
            //                             labelColumnName: "LabelAsKey",
            //                             validationSet: testDataView)
            //    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel",
            //                                                          inputColumnName: "PredictedLabel"));

            // 5.1 (OPTIONAL) Define the model's training pipeline by using explicit hyper-parameters
            //
            var options = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "Image",
                LabelColumnName = "LabelAsKey",
                // Just by changing/selecting InceptionV3/MobilenetV2/ResnetV250  
                // you can try a different DNN architecture (TensorFlow pre-trained model). 
                Arch = ImageClassificationTrainer.Architecture.MobilenetV2,
                Epoch = 50,       //100
                BatchSize = 10,
                LearningRate = 0.01f,
                MetricsCallback = (metrics) => Console.WriteLine(metrics),
                ValidationSet = testDataView,

            };

            var pipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(options)
                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(
                        outputColumnName: "PredictedLabel",
                        inputColumnName: "PredictedLabel"));

            // 6. Train/create the ML model
            Console.WriteLine("*** Training the image classification model with DNN Transfer Learning on top of the selected pre-trained model/architecture ***");

            // Measuring training time
            var watch = Stopwatch.StartNew();

            //Train
            ITransformer trainedModel = pipeline.Fit(trainDataView);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine($"Training with transfer learning took: {elapsedMs / 1000} seconds");

            // 7. Get the quality metrics (accuracy, etc.)
            EvaluateModel(mlContext, testDataView, trainedModel);

            // 8. Save the model to assets/outputs (You get ML.NET .zip model file and TensorFlow .pb model file)
            mlContext.Model.Save(trainedModel, trainDataView.Schema, outputMlNetModelFilePath);
            Console.WriteLine($"Model saved to: {outputMlNetModelFilePath}");

            // 9. Try a single prediction simulating an end-user app
            TrySinglePrediction(imagesFolderPathForPredictions, mlContext, trainedModel);
        }

        private static void EvaluateModel(MLContext mlContext, IDataView testDataset, ITransformer trainedModel)
        {
            Console.WriteLine("Making predictions in bulk for evaluating model's quality...");

            // Measuring time
            var watch = Stopwatch.StartNew();

            var predictionsDataView = trainedModel.Transform(testDataset);

            var metrics = mlContext.MulticlassClassification.Evaluate(predictionsDataView, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");
            ConsoleHelper.PrintMultiClassClassificationMetrics("TensorFlow DNN Transfer Learning", metrics);

            watch.Stop();
            var elapsed2Ms = watch.ElapsedMilliseconds;

            Console.WriteLine($"Predicting and Evaluation took: {elapsed2Ms / 1000} seconds");
        }

        private static void TrySinglePrediction(string imagesFolderPathForPredictions, MLContext mlContext, ITransformer trainedModel)
        {
            // Create prediction function to try one prediction
            var predictionEngine = mlContext.Model
                .CreatePredictionEngine<InMemoryImageData, ImagePrediction>(trainedModel);

            var testImages = FileUtils.LoadInMemoryImagesFromDirectory(
                imagesFolderPathForPredictions, false);

            var imageToPredict = testImages.First();

            var prediction = predictionEngine.Predict(imageToPredict);

            Console.WriteLine(
                $"Image Filename : [{imageToPredict.ImageFileName}], " +
                $"Scores : [{string.Join(",", prediction.Score)}], " +
                $"Predicted Label : {prediction.PredictedLabel}");
        }


        public static IEnumerable<ImageData> LoadImagesFromDirectory(
            string folder,
            bool useFolderNameAsLabel = true)
            => FileUtils.LoadImagesFromDirectory(folder, useFolderNameAsLabel)
                .Select(x => new ImageData(x.imagePath, x.label));

        public static string DownloadImageSet(string imagesDownloadFolder)
        {
            // get a set of images to teach the network about the new classes

            //SINGLE SMALL FLOWERS IMAGESET (200 files)
            const string fileName = "flower_photos_small_set.zip";
            var url = $"https://mlnetfilestorage.file.core.windows.net/imagesets/flower_images/flower_photos_small_set.zip?st=2019-08-07T21%3A27%3A44Z&se=2030-08-08T21%3A27%3A00Z&sp=rl&sv=2018-03-28&sr=f&sig=SZ0UBX47pXD0F1rmrOM%2BfcwbPVob8hlgFtIlN89micM%3D";
            Web.Download(url, imagesDownloadFolder, fileName);
            Compress.UnZip(Path.Join(imagesDownloadFolder, fileName), imagesDownloadFolder);

            //SINGLE FULL FLOWERS IMAGESET (3,600 files)
            //string fileName = "flower_photos.tgz";
            //string url = $"http://download.tensorflow.org/example_images/{fileName}";
            //Web.Download(url, imagesDownloadFolder, fileName);
            //Compress.ExtractTGZ(Path.Join(imagesDownloadFolder, fileName), imagesDownloadFolder);

            return Path.GetFileNameWithoutExtension(fileName);
        }

        public static string GetAbsolutePath(string relativePath)
            => FileUtils.GetAbsolutePath(typeof(Program).Assembly, relativePath);

        public static void ConsoleWriteImagePrediction(string ImagePath, string Label, string PredictedLabel, float Probability)
        {
            var defaultForeground = Console.ForegroundColor;
            var labelColor = ConsoleColor.Magenta;
            var probColor = ConsoleColor.Blue;

            Console.Write("Image File: ");
            Console.ForegroundColor = labelColor;
            Console.Write($"{Path.GetFileName(ImagePath)}");
            Console.ForegroundColor = defaultForeground;
            Console.Write(" original labeled as ");
            Console.ForegroundColor = labelColor;
            Console.Write(Label);
            Console.ForegroundColor = defaultForeground;
            Console.Write(" predicted as ");
            Console.ForegroundColor = labelColor;
            Console.Write(PredictedLabel);
            Console.ForegroundColor = defaultForeground;
            Console.Write(" with score ");
            Console.ForegroundColor = probColor;
            Console.Write(Probability);
            Console.ForegroundColor = defaultForeground;
            Console.WriteLine("");
        }

        private static void FilterMLContextLog(object sender, LoggingEventArgs e)
        {
            if (e.Message.StartsWith("[Source=ImageClassificationTrainer;"))
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
