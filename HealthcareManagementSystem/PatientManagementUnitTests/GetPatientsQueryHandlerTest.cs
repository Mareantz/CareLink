using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Queries;
using Application.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace PatientManagementUnitTests
{
	public class GetPatientsQueryHandlerTest
	{
		private readonly IPatientRepository _patientRepository;
		private readonly IMapper _mapper;

		public GetPatientsQueryHandlerTest()
		{
			_patientRepository = Substitute.For<IPatientRepository>();
			_mapper = Substitute.For<IMapper>();
		}
		[Fact]
		public async Task Handle_ShouldReturnListOfPatients()
		{
			// Arrange
			List<Patient> patients = GeneratePatients();
			_patientRepository.GetPatients().Returns(patients);

			var patientDtos = GeneratePatientDto(patients);
			_mapper.Map<List<PatientDto>>(patients).Returns(patientDtos);

			var query = new GetPatientsQuery();
			var handler = new GetPatientsQueryHandler(_patientRepository, _mapper);
			// Act
			var result = await handler.Handle(query, CancellationToken.None);
			// Assert
			Assert.NotNull(result);
			Assert.Equal(patients.Count, result.Count);
			for (int i = 0; i < patients.Count; i++)
			{
				Assert.Equal(patients[i].FirstName, result[i].FirstName);
				Assert.Equal(patients[i].LastName, result[i].LastName);
				Assert.Equal(patients[i].DateOfBirth, result[i].DateOfBirth);
				Assert.Equal(patients[i].Gender, result[i].Gender);
				Assert.Equal(patients[i].Address, result[i].Address);
			}
		}

		private static List<PatientDto> GeneratePatientDto(List<Patient> patients)
		{
			return patients.Select(patient => new PatientDto
			{
				UserId = Guid.NewGuid(),
				FirstName = patient.FirstName,
				LastName = patient.LastName,
				DateOfBirth = patient.DateOfBirth,
				Gender = patient.Gender,
				Address = patient.Address
			}).ToList();
		}

		private static List<Patient> GeneratePatients()
		{
			return new List<Patient>
			{
				new Patient
				{
					FirstName = "John",
					LastName = "Doe",
					DateOfBirth = new DateOnly(1990, 02, 21),
					Gender = "Male",
					Address = "123 Main St",
					UserId = Guid.NewGuid()
				},
				new Patient
				{
					FirstName = "Jane",
					LastName = "Doe",
					DateOfBirth = new DateOnly(1992,05,15),
					Gender="Female",
					Address = "456 Main St",
					UserId = Guid.NewGuid()
				}
			};
		}
	}
}
