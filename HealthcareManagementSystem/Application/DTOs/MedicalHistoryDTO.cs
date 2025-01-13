

namespace Application.DTOs
{
    public class MedicalHistoryDTO
    {
        public required Guid Id { get; set; }
        public required Guid PatientId { get; set; }
        public required string Diagnosis { get; set; }
        public required string Medication { get; set; }
        public required string Notes { get; set; }
        public required DateTime DateRecorded { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
    }
}
