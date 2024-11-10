using MediatR;
using Application.DTOs;

public class GetDoctorByIdQuery : IRequest<DoctorDto>
{
	public Guid Id { get; set; }
}