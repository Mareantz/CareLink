using Microsoft.ML.Data;

namespace Application.AIML
{
    public class UserDataPrediction
    {
		[ColumnName("Score")]
		public float risk { get; set; }
    }
}