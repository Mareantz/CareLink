using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetDoctorsQuery : IRequest<List<DoctorDTO>>
    {
    }
}
