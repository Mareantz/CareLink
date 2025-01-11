using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using PredictiveHealthcare.Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
	public class AppointmentRepository : IAppointmentRepository
	{
		private readonly ApplicationDbContext context;

		public AppointmentRepository(ApplicationDbContext context)
		{
			this.context = context;
		}
		public async Task<Result<Guid>> AddAppointment(Appointment appointment)
		{
			try
			{
				await context.Appointments.AddAsync(appointment);
				await context.SaveChangesAsync();
				return Result<Guid>.Success(appointment.Id);
			}
			catch (Exception ex)
			{
				var errorMessage = ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
				return Result<Guid>.Failure(errorMessage);
			}
		}

		public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
		{
			return await context.Appointments.FindAsync(id);
		}

		public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
		{
			return await context.Appointments
				.Where(a => a.AppointmentDate.Date == date.Date)
				.ToListAsync();
		}

		public async Task<IEnumerable<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status)
		{
			return await context.Appointments
				.Where(a => a.Status == status)
				.ToListAsync();
		}

		public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
		{
			return await context.Appointments
				.Where(a => a.DoctorId == doctorId)
				.ToListAsync();
		}

		public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId)
		{
			return await context.Appointments
				.Where(a => a.PatientId == patientId)
				.ToListAsync();
		}

		public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDateAsync(Guid doctorId, DateOnly date)
		{
			return await context.Appointments
				.Where(a => a.DoctorId == doctorId && DateOnly.FromDateTime(a.AppointmentDate) == date)
				.ToListAsync();
		}

		public async Task<IEnumerable<Appointment>> GetAppointments()
		{
			return await context.Appointments.ToListAsync();
		}

		public Task<Result> UpdateAppointment(Appointment appointment)
		{
			throw new NotImplementedException();
		}
	}
}
