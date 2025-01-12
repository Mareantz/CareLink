using Application.DTOs;
using MediatR;


namespace Application.Queries
{
    public class GetMedicalHistoriesQuery : IRequest<List<MedicalHistoryDTO>>
    {
    }
}
