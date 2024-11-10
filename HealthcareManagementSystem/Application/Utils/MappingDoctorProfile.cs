using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Commands;
using Application.UseCases.Commands;

namespace Application.Utils
{
    public class MappingDoctorProfile : Profile
    {
        public MappingDoctorProfile()
        {
            CreateMap<Doctor, DoctorDto>();
            CreateMap<CreateDoctorCommand, Doctor>();
            CreateMap<UpdateDoctorCommand, Doctor>();
        }
    }

}
