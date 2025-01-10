using Domain.Common;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
	public interface IAppointmentRepository
	{
		Task<Result<Guid>> AddAppointment(Appointment appointment);
		Task<IEnumerable<Appointment>> GetAppointments();
		Task<Appointment?> GetAppointmentByIdAsync(Guid id);
		Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
		Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status);
		Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId);
		Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId);
		Task<Result> UpdateAppointment(Appointment appointment);
	}
}
