namespace Domain.Entities
{
	public class HealthRiskPrediction
	{
		public Guid Id { get; set; }
		public DateTime LastUpdate { get; set; }
		public float RiskScore { get; set; }
		public List<string> RiskFactors { get; } = [];
		public List<string> PredictedRisks { get; } = [];

		public required Patient Patient { get; set; }
		public Guid PatientId { get; set; }
	}
}
