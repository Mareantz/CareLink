using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Utils
{
	public class MappingAppointmentProfile : Profile
	{
		public MappingAppointmentProfile()
		{
			CreateMap<Appointment, AppointmentCreateDto>();
			CreateMap<Appointment, AppointmentResponseDto>();
			CreateMap<CreateAppointmentCommand, Appointment>();
			CreateMap<UpdateAppointmentCommand, Appointment>();
		}
	}
}
