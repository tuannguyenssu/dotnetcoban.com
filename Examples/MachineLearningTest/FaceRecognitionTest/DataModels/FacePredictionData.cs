namespace FaceRecognitionTest.DataModels
{
    public class FacePredictionData
    {
        public FacePredictionData(byte[] imageDataBytes)
        {
            ImageDataBytes = imageDataBytes;
        }

        public readonly byte[] ImageDataBytes;

    }
}