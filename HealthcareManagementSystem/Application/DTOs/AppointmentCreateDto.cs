public class AppointmentCreateDto
{
    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; }
    public Guid DoctorId { get; set; }
}
