using NSubstitute;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using AutoMapper;
using Application.CommandHandlers;
using Domain.Common;
using FluentAssertions;
using Application.UseCases.CommandHandlers;
using Application.UseCases.Commands;


namespace PatientManagementUnitTests
{
    public class CreateDoctorCommandHandlerTest
    {
        private readonly IDoctorRepository repository;
        private readonly IMapper mapper;
        private readonly CreateDoctorCommandHandler handler;

        public CreateDoctorCommandHandlerTest()
        {
            repository = Substitute.For<IDoctorRepository>();
            mapper = Substitute.For<IMapper>();
            handler = new CreateDoctorCommandHandler(repository, mapper);
        }

        [Fact]
        public async Task Given_ValidCreateDoctorCommand_WhenHandleIsCalled_Then_DoctorShouldBeCreated()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Specialization = "ORL",
                Bio = "3 years of experience"
            };
            var doctor = new Doctor
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Specialization = command.Specialization,
                Bio = command.Bio,
                UserId = new Guid("33333333-3333-3333-3333-333333333333") 
            };

            mapper.Map<Doctor>(command).Returns(doctor);
            repository.AddDoctor(doctor).Returns(Result<Guid>.Success(doctor.UserId));

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            await repository.Received(1).AddDoctor(doctor);
            response.IsSuccess.Should().BeTrue();
            response.Data.Should().Be(doctor.UserId);
        }


        [Fact]
        public async Task Given_InvalidCreateDoctorCommand_WhenHandleIsCalled_Then_DoctorShouldNotBeCreated()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Specialization = "ORL",
                Bio = "3 years of experience"
			};

            var doctor = new Doctor
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
				Specialization = command.Specialization,
				Bio = command.Bio,
			};

            mapper.Map<Doctor>(command).Returns(doctor);
            repository.AddDoctor(doctor).Returns(Result<Guid>.Failure("Error"));

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            await repository.Received(1).AddDoctor(doctor);
            response.IsSuccess.Should().BeFalse();
            response.ErrorMessage.Should().Be("Error");
            response.Data.Should().Be(default(Guid).ToString());


        }
    }
}
