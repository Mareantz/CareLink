using NSubstitute;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using AutoMapper;
using Application.CommandHandlers;
using Domain.Common;
using FluentAssertions;
using Application.UseCases.CommandHandlers;
using Application.UseCases.Commands;


namespace PatientManagementUnitTests
{
    public class CreateUserCommandHandlerTest
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly CreateUserCommandHandler handler;

        public CreateUserCommandHandlerTest()
        {
            repository = Substitute.For<IUserRepository>();
            mapper = Substitute.For<IMapper>();
            handler = new CreateUserCommandHandler(repository, mapper);
        }

        [Fact]
        public async Task Given_ValidCreateUserCommand_WhenHandleIsCalled_Then_UserShouldBeCreated()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "andrei",
                PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqhnTgzAtJkwKUUK",
                Email = "andrei@gmail.com",
                PhoneNumber = "07182828",
                Role = 0
            };
            var user = new User
            {
                Id= new Guid("d1b34e0c-df0b-47ae-9778-8dcbd61d7e5b"),
                Username=command.Username,
                PasswordHash=command.PasswordHash,
                Email=command.Email,
                PhoneNumber=command.PhoneNumber,
                Role=command.Role
            };

            mapper.Map<User>(command).Returns(user);
            repository.AddUser(user).Returns(Result<Guid>.Success(user.Id));

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            await repository.Received(1).AddUser(user);
            response.IsSuccess.Should().BeTrue();
            response.Data.Should().Be(user.Id);
        }


        [Fact]
        public async Task Given_InvalidCreateUserCommand_WhenHandleIsCalled_Then_UserShouldNotBeCreated()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "",
                PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqhnTgzAtJkwKUUK",
                Email = "andrei@gmail.com",
                PhoneNumber = "07182828",
                Role = 0
            };

            var user = new User
            {
                Id = new Guid("d1b34e0c-df0b-47ae-9778-8dcbd61d7e5b"),
                Username = command.Username,
                PasswordHash = command.PasswordHash,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Role = command.Role
            };

            mapper.Map<User>(command).Returns(user);
            repository.AddUser(user).Returns(Result<Guid>.Failure("Error"));

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            await repository.Received(1).AddUser(user);
            response.IsSuccess.Should().BeFalse();
            response.ErrorMessage.Should().Be("Error");
            response.Data.Should().Be(default(Guid).ToString());


        }
    }
}
