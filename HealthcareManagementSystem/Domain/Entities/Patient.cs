namespace Domain.Entities
{
    public class Patient
    {
		public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
		public string PhoneNumber { get; set; }
        public string Address { get; set; }

		//public Doctor Doctor { get; set; }
		//public Guid DoctorId { get; set; }
		//public ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
		//public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
		//public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
		//public ICollection<HealthRiskPrediction> HealthRiskPredictions { get; set; } = new List<HealthRiskPrediction>();
	}
}
