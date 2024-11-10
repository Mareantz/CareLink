using MediatR;
using Application.DTOs;
namespace Application.UseCases.Queries
{
    public class GetPatientByIdQuery : IRequest<PatientDto>
    {
        public Guid Id { get; set; }
    
    }
}
