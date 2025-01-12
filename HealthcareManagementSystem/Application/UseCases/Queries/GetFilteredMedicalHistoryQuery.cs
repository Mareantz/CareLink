using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetFilteredMedicalHistoryQuery : IRequest<Result<PagedResult<MedicalHistoryDTO>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? Id { get; set; }
        public string? Diagnosis { get; set; }
        public string? Medication { get; set; }
        public DateTime? DateRecorded { get; set; }
    }
    
    }

