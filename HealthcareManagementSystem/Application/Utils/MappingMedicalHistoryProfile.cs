using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Commands;
using Application.UseCases.Commands;

namespace Application.Utils
{
    public class MappingMedicalHistoryProfile : Profile
    {
        public MappingMedicalHistoryProfile()
        {
            CreateMap<MedicalHistory, MedicalHistoryDTO>();
            CreateMap<CreateMedicalHistoryCommand, MedicalHistory>();
            CreateMap<UpdateMedicalHistoryCommand, MedicalHistory>();
        }
    }
}
