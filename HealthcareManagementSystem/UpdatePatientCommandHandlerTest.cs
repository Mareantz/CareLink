using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;
using HealthcareManagementSystem.Controllers;

namespace PatientManagementUnitTest
{
    public class UpdatePatientCommandHandlerTest
    {
        private readonly Mock<IPatientRepository> repositoryMock; 
        private readonly UpdatePatientCommandHandler handler;

        public UpdatePatientCommandHandlerTest()
        {
            repositoryMock = new Mock<IPatientRepository>();
            handler = new UpdatePatientCommandHandler(repositoryMock.Object);
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
                DateOfBirth = new DateOnly(1991, 2, 2),
                Gender = "Mascul",
                Email = "john.doe@example.com",
                PhoneNumber = "0987654321",
                Address = "1234 Main St"
            };

            repositoryMock.Setup(r => r.UpdatePatientAsync(It.IsAny<PatientDTO>()))
                           .Returns(Task.CompletedTask);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            repositoryMock.Verify(r => r.UpdatePatientAsync(It.IsAny<PatientDTO>()), Times.Once);
        }
    }
}
