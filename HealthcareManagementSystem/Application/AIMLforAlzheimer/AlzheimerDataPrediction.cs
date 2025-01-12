using Microsoft.ML.Data;

namespace Application.AIMLforAlzheimer
{
    public class AlzheimerDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}