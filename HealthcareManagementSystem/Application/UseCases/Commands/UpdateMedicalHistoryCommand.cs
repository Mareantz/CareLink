using MediatR;
using Domain.Common;


namespace Application.Commands
{
    public class UpdateMedicalHistoryCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public required Guid PatientId { get; set; }
        public required string Diagnosis { get; set; }
        public required string Medication { get; set; }
        public required string Notes { get; set; }

    }
}
