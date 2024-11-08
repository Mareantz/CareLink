namespace Domain.Entities
{
    public class Patient
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required string Address { get; set; }

        public User User { get; set; } = null!;
		public Guid UserId { get; set; }
		public List<Doctor> Doctors { get; } = [];
		public List<string> Allergies { get; } = [];
		public ICollection<MedicalHistory> MedicalHistories { get; } = [];
		public ICollection<Appointment> Appointments { get; } = [];
		public ICollection<HealthRiskPrediction> HealthRiskPredictions { get; } = [];
	}
}
