using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
	public class GetAppointmentsByDoctorIdQueryHandler : IRequestHandler<GetAppointmentsByDoctorIdQuery, Result<List<AppointmentResponseDto>>>
	{
		private readonly IAppointmentRepository appointmentRepository;
		private readonly IMapper mapper;

		public GetAppointmentsByDoctorIdQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
		{
			this.appointmentRepository = appointmentRepository;
			this.mapper = mapper;
		}

		public async Task<Result<List<AppointmentResponseDto>>> Handle(GetAppointmentsByDoctorIdQuery request, CancellationToken cancellationToken)
		{
			var appointments = await appointmentRepository.GetAppointmentsByDoctorIdAsync(request.DoctorId);
			var appointmentDtos = mapper.Map<List<AppointmentResponseDto>>(appointments);
			return Result<List<AppointmentResponseDto>>.Success(appointmentDtos);
		}
	}
}
