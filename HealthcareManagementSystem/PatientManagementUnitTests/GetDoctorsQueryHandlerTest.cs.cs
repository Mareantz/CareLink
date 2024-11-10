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
            mapper.Map<List<DoctorDTO>>(doctors).Returns(doctorDtos);

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
                Assert.Equal(doctors[i].Address, result[i].Address);
                Assert.Equal(doctors[i].Email, result[i].Email);
                Assert.Equal(doctors[i].Gender, result[i].Gender);
                Assert.Equal(doctors[i].PhoneNumber, result[i].PhoneNumber);

            }

        }

        private List<DoctorDTO> GenerateDoctorDto(List<Doctor> doctors)
        {
            return doctors.Select(doctor => new DoctorDTO
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Address = doctor.Address,
                Email = doctor.Email,
                Gender = doctor.Gender,
                PhoneNumber = doctor.PhoneNumber
            }).ToList();
        }

        private List<Doctor> GenerateDoctors()
        {
            return new List<Doctor>
            {
                new Doctor { UserId = Guid.NewGuid(), FirstName = "Doctor", LastName = "one", Gender = "Male", Email = "doc1@gmail.com", PhoneNumber = "1234567", Address = "Address 1" },
                new Doctor { UserId = Guid.NewGuid(), FirstName = "Doctor", LastName = "two", Gender = "Male", Email = "doc2@gmail.com", PhoneNumber = "1234568", Address = "Address 2" },
            };
        }

    }
}
