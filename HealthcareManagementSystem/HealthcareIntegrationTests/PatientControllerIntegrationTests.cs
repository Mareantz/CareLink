using Application.Commands;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PredictiveHealthcare.Infrastructure.Persistence;
using System.Net;
using Domain.Common;
using System.Net.Http.Json;
using FluentAssertions;
using Domain.Entities;

namespace HealthcareManagementSystem.HealthcareIntegrationTests
{
    public class PatientControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly ApplicationDbContext dbContext;
        private string BaseUrl = "/api/v1/patients";

        public PatientControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });
            var scope = factory.Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        [Fact]
        public void GivenPatients_WhenGetAllIsCalled_ThenReturnsTheRightStatusCode()
        {
            var client = factory.CreateClient();
            var response = client.GetAsync(BaseUrl);

            response.Result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async void GivenValidPatient_WhenCreatedIsCalled_Then_ShouldAddToDatabase()
        {
            var client = factory.CreateClient();
            var command = new CreatePatientCommand
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = "2000-01-12",
                Gender = "Male",
                Address = "123 Main St"
            };
            await client.PostAsJsonAsync(BaseUrl, command);
            var patient = await dbContext.Patients.FirstOrDefaultAsync(x => x.FirstName == "John");
            patient.Should().NotBeNull();
        }

        [Fact]
        public void GivenExistingPatient_WhenGetAllIsCalled_ThenReturnsTheRightPatients()
        {
            var client = factory.CreateClient();
            CreateSUT();
            var response = client.GetAsync(BaseUrl);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        private void CreateSUT()
        {
            var patient = new Patient
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "Male",
                Address = "123 Main St"
            };
            dbContext.Patients.Add(patient);
            dbContext.SaveChanges();
        }

        [Fact]
        public void GivenPatients_WhenGetAllIsCalled_ThenReturnsUnauthorized()
        {
            var client = factory.CreateClient();
            var response = client.GetAsync(BaseUrl);
            response.Result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
