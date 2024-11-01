using Domain.Enums;

namespace Domain.Entities
{
    public class User
	{
		public Guid UserId { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public UserRole Role { get; set; }

		public Patient Patient {  get; set; }
		public int PatientId { get; set; }
		public Doctor Doctor { get; set; }
		public int DoctorId { get; set; }

	}
}
