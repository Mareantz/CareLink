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
                Gender = "Male",
                Email = "ex@gmail.com",
                PhoneNumber = "1234567890",
                Address = "123 Main Street"
            };
            var doctor = new Doctor
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Gender = command.Gender,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Address = command.Address,
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
                Gender = "Male",
                Email = "ex@gmail.com",
                PhoneNumber = "1234567890",
                Address = "123 Main Street"
            };

            var doctor = new Doctor
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Gender = command.Gender,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Address = command.Address,
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
