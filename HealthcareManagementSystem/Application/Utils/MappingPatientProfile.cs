using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Commands;

namespace Application.Utils
{
	public class MappingPatientProfile : Profile
	{
		public MappingPatientProfile()
		{
			CreateMap<Patient, PatientDto>();
			CreateMap<CreatePatientCommand, Patient>();
			CreateMap<UpdatePatientCommand, Patient>();
        }
	}
}
