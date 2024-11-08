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
			CreateMap<CreatePatientCommand, Patient>()
				.ForMember(dest => dest.DateOfBirth,
						   opt => opt.MapFrom(src => DateOnly.ParseExact(src.DateOfBirth, "dd-MM-yyyy")));
			CreateMap<UpdatePatientCommand, Patient>();
        }
	}
}
