using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;
using HealthcareManagementSystem.Controllers;

namespace PatientManagementUnitTest
{
    public class CreatePatientCommandHandlerTest
    {
        private readonly Mock<IPatientRepository> repositoryMock;
        private readonly CreatePatientCommandHandler handler;

        public UpdatePatientCommandHandlerTest()
        {
            repositoryMock = new Mock<IPatientRepository>();
            handler = new CreatePatientCommandHandler(repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreatePatientAndReturnGuid()
        {
            // Arrange
            var command = new CreatePatientCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "Male",
                Email = "john.doe@gmail.com",
                PhoneNumber = "1234567890",
                Address = "123 Main St"
            };
            var newPatientId = Guid.NewGuid();

            repositoryMock.Setup(r => r.CreatePatientAsync(It.IsAny<PatientDTO>()))
                           .ReturnsAsync(newPatientId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(newPatientId, result);
            repositoryMock.Verify(r => r.CreatePatientAsync(It.IsAny<PatientDTO>()), Times.Once);
        }
    }
}
