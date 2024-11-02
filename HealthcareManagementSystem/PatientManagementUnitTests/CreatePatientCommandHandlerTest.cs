using Moq;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using AutoMapper;
using Application.CommandHandlers;

namespace PatientManagementUnitTests
{
    public class CreatePatientCommandHandlerTest
    {
        private readonly Mock<IPatientRepository> repositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly CreatePatientCommandHandler handler;

        public CreatePatientCommandHandlerTest()
        {
            repositoryMock = new Mock<IPatientRepository>();
            mapperMock = new Mock<IMapper>();
            handler = new CreatePatientCommandHandler(repositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreatePatientAndReturnGuid()
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
            int newPatientId = 123;
            repositoryMock.Setup(r => r.AddPatient(It.IsAny<Patient>()))
                          .ReturnsAsync(newPatientId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(newPatientId, result);
            repositoryMock.Verify(r => r.AddPatient(It.IsAny<Patient>()), Times.Once);
        }
    }
}
