using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using Infrastructure.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
	public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentResponseDto>>
	{
		private readonly IAppointmentRepository repository;
		private readonly IMapper mapper;

		public GetAppointmentByIdQueryHandler(IAppointmentRepository repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		public async Task<Result<AppointmentResponseDto>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
		{
			var appointment = await repository.GetAppointmentByIdAsync(request.Id);

			if (appointment == null)
			{
				return Result<AppointmentResponseDto>.Failure("Appointment not found.");
			}

			var appointmentDto = mapper.Map<AppointmentResponseDto>(appointment);
			return Result<AppointmentResponseDto>.Success(appointmentDto);
		}
	}
}
