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
    public class MedicalHistoryControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly ApplicationDbContext dbContext;
        private string BaseUrl = "/api/v1/medicalhistories";

        public MedicalHistoryControllerIntegrationTests(WebApplicationFactory<Program> factory)
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
        public void GivenMedicalHistories_WhenGetAllIsCalled_ThenReturnsTheRightStatusCode()
        {
            var client = factory.CreateClient();
            var response = client.GetAsync(BaseUrl);

            response.Result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async void GivenValidMedicalHistory_WhenCreatedIsCalled_Then_ShouldAddToDatabase()
        {
            var client = factory.CreateClient();
            var command = new CreateMedicalHistoryCommand
            {
                Date = DateTime.Now,
                Diagnosis = "Flu",
                Medication = "Tamiflu",
                Notes = "Patient recovering well",
                PatientId = Guid.NewGuid()
            };
            await client.PostAsJsonAsync(BaseUrl, command);
            var medicalHistory = await dbContext.MedicalHistories.FirstOrDefaultAsync(x => x.Diagnosis == "Flu");
            medicalHistory.Should().NotBeNull();
        }


        

        [Fact]
        public void GivenMedicalHistories_WhenGetAllIsCalled_ThenReturnsUnauthorized()
        {
            var client = factory.CreateClient();
            var response = client.GetAsync(BaseUrl);
            response.Result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
