using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.UseCases.Queries;
using Application.UseCases.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

public class GetPatientByIdQueryHandlerTests
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly GetPatientByIdQueryHandler _handler;

    public GetPatientByIdQueryHandlerTests()
    {
        _patientRepository = Substitute.For<IPatientRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetPatientByIdQueryHandler(_patientRepository, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_PatientDto_When_Patient_Exists()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var patient = new Patient
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Gender = "Male",
            Address = "123 Main St",
            UserId = Guid.NewGuid()
        };

        var patientDto = new PatientDto
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = "1990-01-01",
            Gender = "Male",
            Address = "123 Main St"
        };

        _patientRepository.GetPatientById(patientId).Returns(patient);
        _mapper.Map<PatientDto>(patient).Returns(patientDto);

        var query = new GetPatientByIdQuery { Id = patientId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("1990-01-01", result.DateOfBirth);
        Assert.Null(result.DateOfBirth);

        Assert.Equal("Male", result.Gender);
        Assert.Equal("123 Main St", result.Address);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Patient_Does_Not_Exist()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        _patientRepository.GetPatientById(patientId).Returns((Patient?)null);

        var query = new GetPatientByIdQuery { Id = patientId };

        // Act
        PatientDto? result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

}
