

namespace Application.DTOs
{
    public class DoctorDto
    {
        public required Guid UserId { get; set; }
		public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialization { get; set; }
        public required string Bio { get; set; }
    }
}
