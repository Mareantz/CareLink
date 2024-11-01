namespace Domain.Entities
{
	public class MedicalHistory
	{
		public int HistoryId { get; set; }
		public DateTime DateRecorded { get; set; }
		public string Diagnosis { get; set; }
		public string Medications { get; set; }
		public string Allergies { get; set; }
		public string Notes { get; set; }

		public Patient Patient { get; set; }
		public int PatientId { get; set; }
	}
}
