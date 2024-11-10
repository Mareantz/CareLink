namespace Domain.Entities
{
	public class Doctor
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
        public required string Gender { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
		public required string PhoneNumber { get; set; }

		public User User { get; set; } = null!;
		public Guid UserId { get; set; }
		public List<Patient> Patients { get; } = [];
		public ICollection<Appointment> Appointments { get; } = [];
	}
}
