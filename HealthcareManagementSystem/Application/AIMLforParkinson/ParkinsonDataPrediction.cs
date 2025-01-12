using Microsoft.ML.Data;

namespace Application.AIMLforParkinson
{
    public class ParkinsonDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}