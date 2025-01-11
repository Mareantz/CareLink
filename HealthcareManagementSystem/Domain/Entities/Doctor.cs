namespace Domain.Entities
{
	public class Doctor
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
        public required string Specialization { get; set; }
        public required string? Bio { get; set; }

		public User User { get; set; } = null!;
		public Guid UserId { get; set; }
		public List<Patient> Patients { get; } = [];
		public ICollection<Appointment> Appointments { get; } = [];
	}
}
