using Application.UseCases.Commands;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PredictiveHealthcare.Infrastructure.Persistence;

namespace Application.UseCases.CommandHandlers
{
	public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<Guid>>
	{
		private readonly ApplicationDbContext _context;

		public CreateAppointmentCommandHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Guid>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
		{
			var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == request.DoctorId, cancellationToken);

			if (doctor == null)
			{
				return Result<Guid>.Failure("Doctor not found");
			}

			var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == request.PatientId, cancellationToken);

			if (patient == null)
			{
				return Result<Guid>.Failure("Patient not found");
			}
			var appointment = new Appointment
			{
				Id = Guid.NewGuid(),
				AppointmentDate = request.Date,
				Reason = request.Reason,
				Status = AppointmentStatus.Scheduled,
				DoctorId = request.DoctorId,
				Doctor = doctor,
				PatientId = request.PatientId,
				Patient = patient
				// Set PatientId accordingly
			};

			_context.Appointments.Add(appointment);
			await _context.SaveChangesAsync(cancellationToken);

			return Result<Guid>.Success(appointment.Id);
		}
	}
}