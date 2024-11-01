namespace Domain.Entities
{
	public class HealthRiskPrediction
	{
		public int PredictionId { get; set; }
		public DateTime DateCalculated { get; set; }
		public string RiskLevel { get; set; }
		public string RiskFactors { get; set; }
		public string PredictedRisks { get; set; }

		public Patient Patient { get; set; }
		public int PatientId { get; set; }
	}
}
