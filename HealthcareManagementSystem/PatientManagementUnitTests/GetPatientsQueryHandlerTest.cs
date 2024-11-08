//using Application.DTOs;
//using Application.Queries;
//using Application.QueryHandlers;
//using AutoMapper;
//using Domain.Entities;
//using Domain.Repositories;
//using NSubstitute;

//namespace PatientManagementUnitTests
//{
//    public class GetPatientsQueryHandlerTest
//    {
//        private readonly IPatientRepository repository;
//        private readonly IMapper mapper;

//        public GetPatientsQueryHandlerTest()
//        {
//            repository = Substitute.For<IPatientRepository>();
//            mapper = Substitute.For<IMapper>();
//        }

//        [Fact]
//        public async Task Handle_ShouldReturnListOfPatients()
//        {
//            // Arrange
//            List<Patient> patients = GeneratePatients();
//            repository.GetPatients().Returns(patients);

//            var PatientDtos = GeneratePatientDto(patients);
//            mapper.Map<List<PatientDto>>(patients).Returns(PatientDtos);

//            var query = new GetPatientsQuery();
//            var handler = new GetPatientsQueryHandler(repository, mapper);

//            // Act
//            var result = await handler.Handle(query, CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(patients.Count, result.Count);

//            for (int i = 0; i < patients.Count; i++)
//            {
//                Assert.Equal(patients[i].Id, result[i].Id);
//                Assert.Equal(patients[i].FirstName, result[i].FirstName);
//                Assert.Equal(patients[i].LastName, result[i].LastName);
//                Assert.Equal(patients[i].DateOfBirth, result[i].DateOfBirth);
//                Assert.Equal(patients[i].Address, result[i].Address);
//                Assert.Equal(patients[i].Email, result[i].Email);
//                Assert.Equal(patients[i].Gender, result[i].Gender);
//                Assert.Equal(patients[i].PhoneNumber, result[i].PhoneNumber);
//            }
//        }

//        private List<PatientDto> GeneratePatientDto(List<Patient> patients)
//        {
//            return patients.Select(patient => new PatientDto
//            {
//                Id = patient.Id,
//                FirstName = patient.FirstName,
//                LastName = patient.LastName,
//                DateOfBirth = patient.DateOfBirth,
//                Address = patient.Address,
//                Email = patient.Email,
//                Gender = patient.Gender,
//                PhoneNumber = patient.PhoneNumber
//            }).ToList();
//        }

//        private List<Patient> GeneratePatients()
//        {
//            return new List<Patient>
//            {
//                new Patient { Id = 1, FirstName = "Patient", LastName = "one", DateOfBirth = new DateTime(2000, 1, 1), Gender = "Male", Email = "pat1@gmail.com", PhoneNumber = "1234567", Address = "Address 1" },
//                new Patient { Id = 2, FirstName = "Patient", LastName = "two", DateOfBirth = new DateTime(2000, 1, 2), Gender = "Male", Email = "pat2@gmail.com", PhoneNumber = "1234568", Address = "Address 2" },
//            };
//        }
//    }
//}
