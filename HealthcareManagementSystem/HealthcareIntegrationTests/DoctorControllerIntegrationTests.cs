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
using Application.UseCases.Commands;

namespace HealthcareManagementSystem.HealthcareIntegrationTests
{
    public class DoctorControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly ApplicationDbContext dbContext;
        private string BaseUrl = "/api/v1/doctors";

        public DoctorControllerIntegrationTests(WebApplicationFactory<Program> factory)
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
        public void GivenDoctors_WhenGetAllIsCalled_ThenReturnsTheRightStatusCode()
        {
            var client = factory.CreateClient();
            var response = client.GetAsync(BaseUrl);

            response.Result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async void GivenValidDoctor_WhenCreatedIsCalled_Then_ShouldAddToDatabase()
        {
            var client = factory.CreateClient();
            var command = new CreateDoctorCommand
            {
                FirstName = "Jane",
                LastName = "Doe",
                Specialization = "Cardiology",
                Bio = "Experienced cardiologist"
            };
            await client.PostAsJsonAsync(BaseUrl, command);
            var doctor = await dbContext.Doctors.FirstOrDefaultAsync(x => x.FirstName == "Jane");
            doctor.Should().NotBeNull();
        }

        [Fact]
        public void GivenExistingDoctor_WhenGetAllIsCalled_ThenReturnsTheRightDoctors()
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
            var doctor = new Doctor
            {
                FirstName = "Jane",
                LastName = "Doe",
                Specialization = "Cardiology",
                Bio = "Experienced cardiologist",
                UserId = Guid.NewGuid()
            };
            dbContext.Doctors.Add(doctor);
            dbContext.SaveChanges();
        }

        [Fact]
        public void GivenDoctors_WhenGetAllIsCalled_ThenReturnsUnauthorized()
        {
            var client = factory.CreateClient();
            var response = client.GetAsync(BaseUrl);
            response.Result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
