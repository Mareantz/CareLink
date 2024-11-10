using Application.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.QueryHandlers
{
	public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto>
	{
		private readonly IDoctorRepository repository;
		private readonly IMapper mapper;

		public GetDoctorByIdQueryHandler(IDoctorRepository repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
		{
			var doctor = await repository.GetDoctorById(request.Id);
			return mapper.Map<DoctorDto>(doctor);
		}
	}
}