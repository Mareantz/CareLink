using Application.DTOs;
using Application.UseCases.Queries;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using Gridify;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
	public class GetFilteredPatientsQueryHandler : IRequestHandler<GetFilteredPatientsQuery, Result<PagedResult<PatientDto>>>
	{
		private readonly IPatientRepository repository;
		private readonly IMapper mapper;

		public GetFilteredPatientsQueryHandler(IPatientRepository repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}
		public async Task<Result<PagedResult<PatientDto>>> Handle(GetFilteredPatientsQuery request, CancellationToken cancellationToken)
		{
			var patients = await repository.GetPatients();
			var query = patients.AsQueryable();

			var pagedPatients = query.ApplyPaging(request.Page, request.PageSize);
			var patientDtos = mapper.Map<List<PatientDto>>(pagedPatients);
			var pagedResult = new PagedResult<PatientDto>(patientDtos, query.Count());
			return Result<PagedResult<PatientDto>>.Success(pagedResult);
		}
	}
}
