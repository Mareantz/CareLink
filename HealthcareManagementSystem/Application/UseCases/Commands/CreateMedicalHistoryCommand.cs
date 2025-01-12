using MediatR;
using Domain.Common;
namespace Application.Commands
{
    public class CreateMedicalHistoryCommand : IRequest<Result<Guid>>
    {
        public required string? Diagnosis { get; set; }
        public required string? Medication { get; set; }
        public required DateTime? Date { get; set; }
        public required string? Notes { get; set; }
        public required Guid PatientId { get; set; }
    }
}
