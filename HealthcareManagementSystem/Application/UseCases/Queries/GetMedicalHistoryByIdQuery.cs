using MediatR;
using Application.DTOs;

namespace Application.UseCases.Queries
{
    public class GetMedicalHistoryByIdQuery : IRequest<MedicalHistoryDTO>
    {
        public Guid Id { get; set; }
    }
}
