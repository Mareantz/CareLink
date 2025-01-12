namespace Domain.Entities
{
	public class MedicalHistory
	{
		public Guid Id { get; set; }
		public required DateTime DateRecorded { get; set; }
		public string? Diagnosis { get; set; }
		public string? Medication { get; set; }
		public string? Notes { get; set; }
		public ICollection<string> Attachments { get; set; } = new List<string>(); // file paths

		public required Patient Patient { get; set; }
		public Guid PatientId { get; set; }
	}
}
