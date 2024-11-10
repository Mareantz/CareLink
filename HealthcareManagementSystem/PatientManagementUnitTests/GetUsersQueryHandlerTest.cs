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
    public class GetUsersQueryHandlerTest
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public GetUsersQueryHandlerTest()
        {
            repository = Substitute.For<IUserRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfUsers()
        {
            // Arrange
            List<User> users = GenerateUsers();
            repository.GetUsers().Returns(users);

            var userDtos = GenerateUserDto(users);
            mapper.Map<List<UserDTO>>(users).Returns(userDtos);

            var query = new GetUserQuery();
            var handler = new GetUserQueryHandler(repository, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);

            for (int i = 0; i < users.Count; i++)
            {
                Assert.Equal(users[i].Id, result[i].Id);
                Assert.Equal(users[i].Username, result[i].Username);
                Assert.Equal(users[i].PasswordHash, result[i].PasswordHash);
                Assert.Equal(users[i].Email, result[i].Email);
                Assert.Equal(users[i].PhoneNumber, result[i].PhoneNumber);
                Assert.Equal(users[i].Role, result[i].Role)
            }

        }

        private List<UserDTO> GenerateUserDto(List<User> users)
        {
            return users.Select(user => new UserDTO
            {
                Id=user.Id,
                Username=user.Username,
                PasswordHash=user.PasswordHash,
                Email=user.Email,
                PhoneNumber=user.PhoneNumber,
                Role=user.Role
            }).ToList();
        }

        private List<User> GenerateUsers()
        {
            return new List<User>
            {
                new User {
                    Id = Guid.NewGuid(),
                    Username = "dan",
                    PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqhnTgzAtJkwKUUK",
                    Email = "andrei@gmail.com",
                    PhoneNumber = "07182828",
                    Role = 0",

                new User {
                    Id = Guid.NewGuid(),
                    Username = "ion",
                    PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqhnTgzAtJkwKUUK",
                    Email = "andrei@gmail.com",
                    PhoneNumber = "07182828",
                    Role = 0",
            };
        }

    }
}
