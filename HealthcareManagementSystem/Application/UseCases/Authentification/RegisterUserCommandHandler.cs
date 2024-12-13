using Application.UseCases.Authentification;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using FluentValidation;
using MediatR;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
	private readonly IUserRepository _userRepository;
	private readonly IPatientRepository _patientRepository;
	private readonly IDoctorRepository _doctorRepository;
	private readonly IValidator<RegisterUserCommand> _validator;

	public RegisterUserCommandHandler(
		IUserRepository userRepository,
		IPatientRepository patientRepository,
		IDoctorRepository doctorRepository,
		IValidator<RegisterUserCommand> validator)
	{
		_userRepository = userRepository;
		_patientRepository = patientRepository;
		_doctorRepository = doctorRepository;
		_validator = validator;
	}

	public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var validationResult = await _validator.ValidateAsync(request, cancellationToken);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
		var user = new User
		{
			Id = Guid.NewGuid(),
			Username = request.Username,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
			Email = request.Email,
			PhoneNumber = request.PhoneNumber,
			Role = request.Role
		};

		await _userRepository.Register(user, cancellationToken);

		if (request.Role == UserRole.Patient)
		{
			if (request.DateOfBirth == null || string.IsNullOrEmpty(request.Gender) || string.IsNullOrEmpty(request.Address))
			{
				throw new ArgumentException("Missing required fields for Patient.");
			}

			var patient = new Patient
			{
				UserId = user.Id,
				FirstName = request.FirstName,
				LastName = request.LastName,
				DateOfBirth = request.DateOfBirth.Value,
				Gender = request.Gender,
				Address = request.Address
			};

			await _patientRepository.AddPatient(patient);
		}
		else if (request.Role == UserRole.Doctor)
		{
			var doctor = new Doctor
			{
				UserId = user.Id,
				FirstName = request.FirstName,
				LastName = request.LastName,
				Specialization = request.Specialization,
				Bio = ""
			};

			await _doctorRepository.AddDoctor(doctor);
		}
		else
		{
			throw new ArgumentException("Invalid role specified.");
		}

		return user.Id;
	}
}
