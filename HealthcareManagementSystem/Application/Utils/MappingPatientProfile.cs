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
				opt => opt.MapFrom(src =>
					src.DateOfBirth != null
						? ParseDateOnly(src.DateOfBirth)
						: default));

            CreateMap<UpdatePatientCommand, Patient>()
			.ForMember(dest => dest.DateOfBirth,
				opt => opt.MapFrom(src =>
					src.DateOfBirth != null
						? ParseDateOnly(src.DateOfBirth)
						: default));


        }
		public static DateOnly ParseDateOnly(string date)
		{
			return DateOnly.ParseExact(date, "dd-MM-yyyy", new System.Globalization.CultureInfo("ro-RO"));
		}

	}
}
