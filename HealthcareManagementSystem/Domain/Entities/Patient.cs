namespace Domain.Entities
{
    public class Patient
    {
        

		public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
		public string PhoneNumber { get; set; }
        public string Address { get; set; }

		public User User { get; set; }
		public Guid UserId { get; set; }
		public ICollection<MedicalHistory> MedicalHistories { get; set; }
		public ICollection<Appointment> Appointments { get; set; }
        public ICollection<HealthRiskPrediction> HealthRiskPredictions { get; set; }
	}
}
