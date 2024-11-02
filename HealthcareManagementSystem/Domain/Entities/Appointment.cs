using Domain.Enums;

namespace Domain.Entities
{
    public class Appointment
	{
		public int Id { get; set; }
		public DateTime AppointmentDate { get; set; }
		public string Reason { get; set; }
		public AppointmentStatus Status { get; set; }

		public Doctor Doctor { get; set; }
		public Guid DoctorId { get; set; }
		public Patient Patient { get; set; }
		public int PatientId { get; set; }
	}
}
