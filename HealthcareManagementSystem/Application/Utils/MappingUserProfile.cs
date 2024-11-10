using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Commands;
using Application.UseCases.Commands;

namespace Application.Utils
{
    public class MappingUserProfile : Profile
    {
        public MappingUserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserCommand, User>();
            CreateMap<UpdateUserCommand, User>();
        }
    }
}
