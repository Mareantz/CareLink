using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Commands;
using System.Globalization;
using Domain.Common;
namespace Application.Utils
{
	public class MappingPatientProfile : Profile
	{
		public MappingPatientProfile()
		{
			CreateMap<Patient, PatientDto>();
            CreateMap<CreatePatientCommand, Patient>()
            .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src => DateOnly.ParseExact(src.DateOfBirth, "dd-MM-yyyy")));

            CreateMap<UpdatePatientCommand, Patient>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.ParseExact(src.DateOfBirth, "dd-MM-yyyy")));


        }
    }
}
