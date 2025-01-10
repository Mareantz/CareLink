using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using Gridify;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
	public class GetFilteredPatientsQueryHandler : IRequestHandler<GetFilteredPatientsQuery, Result<PagedResult<PatientDto>>>
	{
		private readonly IPatientRepository patientRepository;
		private readonly IMapper mapper;

		public GetFilteredPatientsQueryHandler(IPatientRepository patientRepository, IMapper mapper)
		{
			this.patientRepository = patientRepository;
			this.mapper = mapper;
		}

		public async Task<Result<PagedResult<PatientDto>>> Handle(GetFilteredPatientsQuery request, CancellationToken cancellationToken)
		{
			var patients = await patientRepository.GetFilteredPatientsAsync(
				request.Page,
				request.PageSize,
				request.FirstName,
				request.LastName,
				request.Gender,
				request.DateOfBirth);

			var patientDtos = mapper.Map<List<PatientDto>>(patients.Data);

			var pagedResult = new PagedResult<PatientDto>(patientDtos, patients.TotalCount);
			return Result<PagedResult<PatientDto>>.Success(pagedResult);
		}
	}
}
