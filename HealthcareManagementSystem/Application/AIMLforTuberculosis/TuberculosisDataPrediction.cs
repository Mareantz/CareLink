using Microsoft.ML.Data;

namespace Application.AIMLforTuberculosis
{
    public class TuberculosisDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}