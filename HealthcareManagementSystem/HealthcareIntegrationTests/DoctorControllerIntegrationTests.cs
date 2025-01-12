//using Application.Commands;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using PredictiveHealthcare.Infrastructure.Persistence;
//using System.Net;
//using System.Net.Http.Json;
//using FluentAssertions;
//using Domain.Entities;
//using Application.UseCases.Commands;
//using Xunit.Abstractions;
//using Domain.Enums;

//namespace HealthcareManagementSystem.HealthcareIntegrationTests
//{
//	public class DoctorControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
//	{
//		private readonly WebApplicationFactory<Program> factory;
//		private readonly ApplicationDbContext dbContext;
//		private readonly ITestOutputHelper _output;
//		private string BaseUrl = "/api/v1/doctors";

//		public DoctorControllerIntegrationTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
//		{
//			_output = output;
//			this.factory = factory.WithWebHostBuilder(builder =>
//			{
//				builder.ConfigureServices(services =>
//				{
//					var descriptor = services.SingleOrDefault(
//						d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
//					if (descriptor != null)
//					{
//						services.Remove(descriptor);
//					}
//					services.AddDbContext<ApplicationDbContext>(options =>
//					{
//						options.UseInMemoryDatabase("InMemoryDbForTesting");
//					});
//				});
//			});
//			var scope = factory.Services.CreateScope();
//			dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//			dbContext.Database.EnsureCreated();
//		}

//		[Fact]
//		public async Task GivenDoctors_WhenGetAllIsCalled_ThenReturnsUnauthorized()
//		{
//			// Arrange
//			var client = factory.CreateClient();

//			// Act
//			var response = await client.GetAsync(BaseUrl);
//			_output.WriteLine($"Response Status Code: {response.StatusCode}");

//			// Assert
//			response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
//		}

//		[Fact]
//		public async Task GivenValidDoctor_WhenCreatedIsCalled_Then_ShouldAddToDatabase()
//		{
//			// Arrange
//			var client = factory.CreateClient();
//			var command = new CreateDoctorCommand
//			{
//				FirstName = "Jane",
//				LastName = "Doe",
//				Specialization = "Cardiology",
//				Bio = "Experienced cardiologist"
//			};

//			// Act
//			var postResponse = await client.PostAsJsonAsync(BaseUrl, command);
//			_output.WriteLine($"Post Response Status Code: {postResponse.StatusCode}");

//			var doctor = await dbContext.Doctors.FirstOrDefaultAsync(x => x.FirstName == "Jane");
//			_output.WriteLine($"Doctor Found: {doctor != null}");

//			// Assert
//			postResponse.StatusCode.Should().Be(HttpStatusCode.Created); // Assuming your API returns 201 Created
//			doctor.Should().NotBeNull();
//			doctor.FirstName.Should().Be("Jane");
//			doctor.LastName.Should().Be("Doe");
//		}

//		[Fact]
//		public async Task GivenExistingDoctor_WhenGetAllIsCalled_ThenReturnsTheRightDoctors()
//		{
//			// Arrange
//			var client = factory.CreateClient();
//			CreateSUT();

//			// Act
//			var response = await client.GetAsync(BaseUrl);
//			_output.WriteLine($"Response Status Code: {response.StatusCode}");
//			var doctors = await response.Content.ReadFromJsonAsync<List<Doctor>>();

//			// Assert
//			response.StatusCode.Should().Be(HttpStatusCode.OK);
//			doctors.Should().NotBeNull();
//			doctors.Count.Should().BeGreaterThan(0);
//			doctors.Should().Contain(d => d.FirstName == "Jane" && d.LastName == "Doe");
//		}

//		private void CreateSUT()
//		{
//			var user = new User
//			{
//				Id = Guid.NewGuid(),
//				Username = "jane_doe",
//				PasswordHash = "hashed_password",
//				Email = "jane@example.com",
//				PhoneNumber = "1234567890",
//				Role = UserRole.Doctor
//			};
//			dbContext.Users.Add(user);

//			var doctor = new Doctor
//			{
//				FirstName = "Jane",
//				LastName = "Doe",
//				Specialization = "Cardiology",
//				Bio = "Experienced cardiologist",
//				UserId = user.Id,
//				User = user
//			};
//			dbContext.Doctors.Add(doctor);
//			dbContext.SaveChanges();
//		}

//		public void Dispose()
//		{
//			dbContext.Dispose();
//		}
//	}
//}
