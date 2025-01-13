using Application.UseCases.Authentification;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Authentification
{
	public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
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

		public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			var validationResult = await _validator.ValidateAsync(request, cancellationToken);

			if (!validationResult.IsValid)
			{
				var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
				return Result<Guid>.Failure(string.Join("; ", errors));
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

			var registerResult = await _userRepository.Register(user, cancellationToken);
			if (!registerResult.IsSuccess)
			{
				return Result<Guid>.Failure(registerResult.ErrorMessage);
			}

			if (request.Role == UserRole.Patient)
			{
				if (request.DateOfBirth == null || string.IsNullOrEmpty(request.Gender) || string.IsNullOrEmpty(request.Address))
				{
					return Result<Guid>.Failure("Missing required fields for Patient.");
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

				var addPatientResult = await _patientRepository.AddPatient(patient);
				if (!addPatientResult.IsSuccess)
				{
					return Result<Guid>.Failure("Failed to add patient details.");
				}
			}
			else if (request.Role == UserRole.Doctor)
			{
				var doctor = new Doctor
				{
					UserId = user.Id,
					FirstName = request.FirstName,
					LastName = request.LastName,
					Specialization = request.Specialization ?? string.Empty,
					Bio = string.Empty
				};

				var addDoctorResult = await _doctorRepository.AddDoctor(doctor);
				if (!addDoctorResult.IsSuccess)
				{
					return Result<Guid>.Failure("Failed to add doctor details.");
				}
			}
			else
			{
				return Result<Guid>.Failure("Invalid role specified.");
			}

			return Result<Guid>.Success(user.Id);
		}
	}
}