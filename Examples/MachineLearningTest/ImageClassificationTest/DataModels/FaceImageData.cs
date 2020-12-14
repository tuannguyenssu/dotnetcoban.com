namespace ImageClassificationTest.DataModels
{
    public class FaceImageData
    {
        public FaceImageData(byte[] image, string label)
        {
            Image = image;
            Label = label;
        }

        public readonly byte[] Image;

        public readonly string Label;
    }
}