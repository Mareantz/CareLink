using Domain.Enums;

namespace Domain.Entities
{
    public class Appointment
	{
		public Guid Id { get; set; }
		public DateTime AppointmentDate { get; set; }
		public required string Reason { get; set; }
		public AppointmentStatus Status { get; set; }

		public required Doctor Doctor { get; set; }
		public Guid DoctorId { get; set; }
		public required Patient Patient { get; set; }
		public Guid PatientId { get; set; }
	}
}
