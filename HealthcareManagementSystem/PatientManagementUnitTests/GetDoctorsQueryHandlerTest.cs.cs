using Application.DTOs;
using Application.UseCases.Queries;
using Application.UseCases.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using System;

namespace PatientManagementUnitTests
{
    public class GetDoctorsQueryHandlerTest
    {
        private readonly IDoctorRepository repository;
        private readonly IMapper mapper;

        public GetDoctorsQueryHandlerTest()
        {
            repository = Substitute.For<IDoctorRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfDoctors()
        {
            // Arrange
            List<Doctor> doctors = GenerateDoctors();
            repository.GetDoctors().Returns(doctors);

            var doctorDtos = GenerateDoctorDto(doctors);
            mapper.Map<List<DoctorDto>>(doctors).Returns(doctorDtos);

            var query = new GetDoctorsQuery();
            var handler = new GetDoctorsQueryHandler(repository, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctors.Count, result.Count);

            for (int i = 0; i < doctors.Count; i++)
            {
                Assert.Equal(doctors[i].FirstName, result[i].FirstName);
                Assert.Equal(doctors[i].LastName, result[i].LastName);
                Assert.Equal(doctors[i].Specialization, result[i].Specialization);

			}

        }

        private static List<DoctorDto> GenerateDoctorDto(List<Doctor> doctors)
        {
            return doctors.Select(doctor => new DoctorDto
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Specialization = doctor.Specialization,
				Bio = doctor.Bio
			}).ToList();
        }

        private static List<Doctor> GenerateDoctors()
        {
            return new List<Doctor>
            {
                new Doctor { UserId = Guid.NewGuid(), FirstName = "Doctor", LastName = "one", Specialization = "ORL", Bio = "3 years of experience" },
                new Doctor { UserId = Guid.NewGuid(), FirstName = "Doctor", LastName = "two", Specialization = "Cardiolog", Bio = "5 years of experience" },
            };
        }

    }
}
