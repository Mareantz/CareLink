using MediatR;
using Application.DTOs;

namespace Application.UseCases.Queries
{
	public class GetDoctorByIdQuery : IRequest<DoctorDto>
	{
		public Guid Id { get; set; }
	}
}