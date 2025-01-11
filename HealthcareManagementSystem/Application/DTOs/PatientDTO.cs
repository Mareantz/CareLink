namespace Application.DTOs
{
	public class PatientDto
	{
		public required Guid UserId { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required DateOnly DateOfBirth { get; set; }
		public required string Gender { get; set; }
		public required string Address { get; set; }

	}
}
