using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Queries;
using Application.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

public class GetPatientsQueryHandlerTests
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly GetPatientsQueryHandler _handler;

    public GetPatientsQueryHandlerTests()
    {
        _patientRepository = Substitute.For<IPatientRepository>();

        // Create a mock AutoMapper configuration
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString("dd-MM-yyyy")));
        });
        _mapper = config.CreateMapper();

        _handler = new GetPatientsQueryHandler(_patientRepository, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Patients_From_Repository()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new Patient
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1990, 2, 21),
                Gender = "Male",
                Address = "123 Main St"
            },
            new Patient
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateOnly(1985, 5, 15),
                Gender = "Female",
                Address = "456 Oak Ave"
            }
        };

        _patientRepository.GetPatients().Returns(patients);

        var query = new GetPatientsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal("John", result[0].FirstName);
        Assert.Equal("Doe", result[0].LastName);
        Assert.Equal("21-02-1990", result[0].DateOfBirth);
        Assert.Equal("Male", result[0].Gender);
        Assert.Equal("123 Main St", result[0].Address);

        Assert.Equal("Jane", result[1].FirstName);
        Assert.Equal("Smith", result[1].LastName);
        Assert.Equal("15-05-1985", result[1].DateOfBirth);
        Assert.Equal("Female", result[1].Gender);
        Assert.Equal("456 Oak Ave", result[1].Address);
    }
}
