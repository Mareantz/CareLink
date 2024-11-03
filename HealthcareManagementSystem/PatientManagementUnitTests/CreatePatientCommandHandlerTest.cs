using NSubstitute;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using AutoMapper;
using Application.CommandHandlers;
using Domain.Common;
using FluentAssertions;

namespace PatientManagementUnitTests
{
	public class CreatePatientCommandHandlerTest
	{
		private readonly IPatientRepository repository;
		private readonly IMapper mapper;
		private readonly CreatePatientCommandHandler handler;

		public CreatePatientCommandHandlerTest()
		{
			repository = Substitute.For<IPatientRepository>();
			mapper = Substitute.For<IMapper>();
			handler = new CreatePatientCommandHandler(repository, mapper);
		}

		[Fact]
		public async Task Given_ValidCreatePatientCommand_WhenHandleIsCalled_Then_PatientShouldBeCreated()
		{
			// Arrange
			var command = new CreatePatientCommand
			{
				FirstName = "John",
				LastName = "Doe",
				DateOfBirth = new DateTime(1990, 1, 1),
				Gender = "Male",
				Email = "john.doe@gmail.com",
				PhoneNumber = "1234567890",
				Address = "123 Main St"
			};
			var patient = new Patient
			{ 
				Id = 123,
				FirstName = command.FirstName,
				LastName = command.LastName,
				DateOfBirth = command.DateOfBirth,
				Gender = command.Gender,
				Email = command.Email,
				PhoneNumber = command.PhoneNumber,
				Address = command.Address
			};

			mapper.Map<Patient>(command).Returns(patient);
			repository.AddPatient(patient).Returns(Result<int>.Success(patient.Id));

			// Act
			var response = await handler.Handle(command, CancellationToken.None);

			// Assert
			await repository.Received(1).AddPatient(patient);
			response.IsSuccess.Should().BeTrue();
			response.Data.Should().Be(patient.Id);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailureResult_WhenPatientCreationFails()
		{
			// Arrange
			var command = new CreatePatientCommand
			{
				FirstName = "John",
				LastName = "Doe",
				DateOfBirth = new DateTime(1990, 1, 1),
				Gender = "Male",
				Email = "john.doe@gmail.com",
				PhoneNumber = "1234567890",
				Address = "123 Main St"
			};
			var patient = new Patient
			{
				Id = 123,
				FirstName = command.FirstName,
				LastName = command.LastName,
				DateOfBirth = command.DateOfBirth,
				Gender = command.Gender,
				Email = command.Email,
				PhoneNumber = command.PhoneNumber,
				Address = command.Address

			};

			mapper.Map<Patient>(command).Returns(patient);
			repository.AddPatient(patient).Returns(Result<int>.Failure("Error adding patient"));

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			await repository.Received(1).AddPatient(patient);
			result.IsSuccess.Should().BeFalse();
			result.ErrorMessage.Should().Be("Error adding patient");
		}
	}
}
