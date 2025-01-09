using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IAppointmentRepository
	{
		Task<Result<Guid>> AddAppointment(Appointment appointment);
		Task<IEnumerable<Appointment>> GetAppointments();
		Task<Result> UpdateAppointment(Appointment appointment);
		Task<Appointment?> GetAppointmentById(Guid id);
	}
}
