namespace FaceRecognitionTest.DataModels
{
    public class FaceTrainData
    {
        public FaceTrainData(string path, string label)
        {
            Path = path;
            Label = label;
        }

        public readonly string Path;

        public readonly string Label;
    }

}
