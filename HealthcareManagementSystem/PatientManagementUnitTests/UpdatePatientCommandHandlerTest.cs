//using NSubstitute;
//using Application.Commands;
//using Domain.Repositories;
//using Application.CommandHandlers;
//using AutoMapper;
//using Domain.Entities;

//namespace PatientManagementUnitTests
//{
//	public class UpdatePatientCommandHandlerTest
//	{
//		private readonly IPatientRepository repository;
//		private readonly IMapper mapper;
//		private readonly UpdatePatientCommandHandler handler;

//		public UpdatePatientCommandHandlerTest()
//		{
//			repository = Substitute.For<IPatientRepository>();
//			mapper = Substitute.For<IMapper>();
//			handler = new UpdatePatientCommandHandler(repository, mapper);
//		}

//		[Fact]
//		public async Task Handle_ShouldUpdatePatient()
//		{
//			// Arrange
//			var command = new UpdatePatientCommand
//			{
//				Id = 1,
//				FirstName = "Ion",
//				LastName = "Doi",
//				DateOfBirth = new DateTime(1991, 2, 2),
//				Gender = "Mascul",
//				Email = "john.doe@example.com",
//				PhoneNumber = "0987654321",
//				Address = "1234 Main St"
//			};

//			var patient = new Patient
//			{
//				Id = command.Id,
//				FirstName = command.FirstName,
//				LastName = command.LastName,
//				DateOfBirth = command.DateOfBirth,
//				Gender = command.Gender,
//				Email = command.Email,
//				PhoneNumber = command.PhoneNumber,
//				Address = command.Address
//			};

//			mapper.Map<Patient>(command).Returns(patient);
//			repository.UpdatePatient(Arg.Any<Patient>()).Returns(Task.CompletedTask);

//			// Act
//			await handler.Handle(command, CancellationToken.None);

//			// Assert
//			await repository.Received(1).UpdatePatient(Arg.Is<Patient>(p =>
//				p.Id == command.Id &&
//				p.FirstName == command.FirstName &&
//				p.LastName == command.LastName &&
//				p.DateOfBirth == command.DateOfBirth &&
//				p.Gender == command.Gender &&
//				p.Email == command.Email &&
//				p.PhoneNumber == command.PhoneNumber &&
//				p.Address == command.Address
//			));
//		}
//	}
//}
