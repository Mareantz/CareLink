using Application;
using Application.Queries;
using Application.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace PatientManagementUnitTest
{
    public class GetPatientssQueryHandlerTest
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;
        public GetPatientssQueryHandlerTest()
        {
            repository = Substitute.For<IPatientRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfPatients()
        {
            List<Patient> patients = GeneratePatients();
            repository.GetAllAsync().Returns(patients);
            var query = new GetPatientsQuery();
            GeneratePatientDTO(patients);
            var handler = new GetPatientsQueryHandler(repository, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patients.Count, result.Count);
            Assert.Equal(patients.First().Id, result.First().Id);
            Assert.Equal(patients.First().Name, result.First().Name);
            Assert.Equal(patients.First().Age, result.First().Age);
            Assert.Equal(patients.Last().Id, result.Last().Id);
            Assert.Equal(patients.Last().Name, result.Last().Name);
            Assert.Equal(patients.Last().Age, result.Last().Age);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(patients.Count);
            result.First().Id.Should().Be(patients.First().Id);
            result.First().Name.Should().Be(patients.First().Name);
            result.First().Age.Should().Be(patients.First().Age);
            result.Last().Id.Should().Be(patients.Last().Id);
            result.Last().Name.Should().Be(patients.Last().Name);
            result.Last().Age.Should().Be(patients.Last().Age);
        }

        private void GeneratePatientDTO(List<Patient> patients)
        {
            mapper.Map<List<PatientDTO>>(Arg.Any<List<Patient>>()).Returns(new List<PatientDTO>
            {
                new PatientDTO { Id = patients.First().Id, Name = patients.First().Name, Age = patients.First().Age },
                new PatientDTO { Id = patients.Last().Id, Name = patients.Last().Name, Age = patients.Last().Age }
            });
        }

        private List<Patient> GeneratePatients()
        {
            return new List<Patient>
            {
                new Patient { Id = 1, Name = "Patient 1", Age = 20 },
                new Patient { Id = 2, Name = "Patient 2", Age = 25 }
            };
        }

    }
}
