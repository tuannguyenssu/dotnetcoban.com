using Microsoft.ML.Data;

namespace ImageClassificationTest.DataModels
{
    public class FaceImagePrediction
    {
        [ColumnName("Score")]
        public float[] Score;

        [ColumnName("PredictedLabel")]
        public string PredictedLabel;
    }
}