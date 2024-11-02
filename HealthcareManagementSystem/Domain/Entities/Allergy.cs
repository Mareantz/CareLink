namespace Domain.Entities
{
	public class Allergy
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public Patient Patient { get; set; }
		public int PatientId { get; set; }
	}
}
