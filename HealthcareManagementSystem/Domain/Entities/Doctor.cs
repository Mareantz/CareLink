namespace Domain.Entities
{
	public class Doctor
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Specialization { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public User User { get; set; }
		public Guid UserId { get; set; }
		public List<Patient> Patients { get; set; } = new List<Patient>();
		public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
	}
}
