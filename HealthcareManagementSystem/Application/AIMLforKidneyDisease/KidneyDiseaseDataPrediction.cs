using Microsoft.ML.Data;

namespace Application.AIMLforKidneyDisease
{
    public class KidneyDiseaseDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}