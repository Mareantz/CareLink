using Microsoft.ML.Data;

namespace Application.AIMLforDiabetes
{
    public class DiabetesDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}