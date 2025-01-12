using Microsoft.ML.Data;

namespace Application.AIMLforEpilepsy
{
    public class EpilepsyDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}