using Microsoft.ML.Data;

namespace Application.AIMLforDiabetesMellitus
{
    public class DiabetesMellitusDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}