using Microsoft.ML.Data;

namespace Application.AIMLforStroke
{
    public class StrokeDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}