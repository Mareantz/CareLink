using Moq;
using Application.Commands;
using Domain.Repositories;
using Application.CommandHandlers;
using AutoMapper;
using Domain.Entities;

namespace PatientManagementUnitTests
{
    public class UpdatePatientCommandHandlerTest
    {
        private readonly Mock<IPatientRepository> repositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly UpdatePatientCommandHandler handler;

        public UpdatePatientCommandHandlerTest()
        {
            repositoryMock = new Mock<IPatientRepository>();
            mapperMock = new Mock<IMapper>();
            handler = new UpdatePatientCommandHandler(repositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdatePatient()
        {
            // Arrange
            var command = new UpdatePatientCommand
            {
                Id = 1,
                FirstName = "Ion",
                LastName = "Doi",
                DateOfBirth = new DateTime(1991, 2, 2),
                Gender = "Mascul",
                Email = "john.doe@example.com",
                PhoneNumber = "0987654321",
                Address = "1234 Main St"
            };

            var patient = new Patient
            {
                Id = command.Id,
                FirstName = command.FirstName,
                LastName = command.LastName,
                DateOfBirth = command.DateOfBirth,
                Gender = command.Gender,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Address = command.Address
            };

            
            mapperMock.Setup(m => m.Map<Patient>(command)).Returns(patient);

              repositoryMock.Setup(r => r.UpdatePatient(It.IsAny<Patient>()))
                           .Returns(Task.CompletedTask);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.UpdatePatient(It.Is<Patient>(p =>
                p.Id == command.Id &&
                p.FirstName == command.FirstName &&
                p.LastName == command.LastName &&
                p.DateOfBirth == command.DateOfBirth &&
                p.Gender == command.Gender &&
                p.Email == command.Email &&
                p.PhoneNumber == command.PhoneNumber &&
                p.Address == command.Address
            )), Times.Once);
        }
    }
}
