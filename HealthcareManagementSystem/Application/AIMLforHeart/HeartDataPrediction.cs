using Microsoft.ML.Data;

namespace Application.AIMLforHeart
{
    public class HeartDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}