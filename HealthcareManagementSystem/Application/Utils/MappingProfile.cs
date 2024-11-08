using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Commands;

namespace Application.Utils
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Patient, PatientDto>();
			CreateMap<CreatePatientCommand, Patient>();
			CreateMap<UpdatePatientCommand, Patient>();
        }
	}
}
