using NSubstitute;
using Application.Commands;
using Application.CommandHandlers;
using Domain.Repositories;
using AutoMapper;
using Xunit;
using System;

public class UpdateUserCommandValidatorTests
{
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTests()
    {
        _validator = new UpdateUserCommandValidator();
    }

    
    [Fact]
    public void Validate_Should_Return_False_When_Username_Is_Empty()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Username = "",
            PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqynTgzAtJkwKUUK",
            Email = "andrei@gmail.com",
            PhoneNumber = "07182828",
            Role = 0
        };

		var result = ValidateCommand(command);
        Assert.False(result, "Expected validation to fail when Username is empty.");
    }

    [Fact]
    public void Validate_Should_Return_True_When_Username_Is_Valid()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Username = "andrei",
            PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqynTgzAtJkwKUUK",
            Email = "andrei@gmail.com",
            PhoneNumber = "07182828",
            Role = 0
        };

        var result = ValidateCommand(command);
        Assert.True(result, "Expected validation to pass when Username is valid.");
    }






    [Fact]
    public void Validate_Should_Return_False_When_PasswordHash_Is_Empty()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Username = "andrei",
            PasswordHash = "",
            Email = "andrei@gmail.com",
            PhoneNumber = "07182828",
            Role = 0
        };

        var result = ValidateCommand(command);
        Assert.False(result, "Expected validation to fail when PasswordHash is empty.");
    }

    [Fact]
    public void Validate_Should_Return_True_When_PasswordHash_Is_Valid()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Username = "andrei",
            PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqynTgzAtJkwKUUK",
            Email = "andrei@gmail.com",
            PhoneNumber = "07182828",
            Role = 0
        };

        var result = ValidateCommand(command);
        Assert.True(result, "Expected validation to pass when Password is valid.");
    }


    [Fact]
    public void Validate_Should_Return_False_When_Email_Is_Empty()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Username = "andrei",
            PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqynTgzAtJkwKUUK",
            Email = "",
            PhoneNumber = "07182828",
            Role = 0
        };

        var result = ValidateCommand(command);
        Assert.False(result, "Expected validation to fail when Email is empty.");
    }

    [Fact]
    public void Validate_Should_Return_True_When_Email_Is_Valid()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Username = "andrei",
            PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqynTgzAtJkwKUUK",
            Email = "andrei@gmail.com",
            PhoneNumber = "07182828",
            Role = 0
        };

        var result = ValidateCommand(command);
        Assert.True(result, "Expected validation to pass when Email is valid.");
    }







    [Fact]
    public void Validate_Should_Return_False_When_PhoneNumber_Is_Empty()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Username = "andrei",
            PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqynTgzAtJkwKUUK",
            Email = "andrei@gmail.com",
            PhoneNumber = "",
            Role = 0
        };

        var result = ValidateCommand(command);
        Assert.False(result, "Expected validation to fail when PhoneNumber is empty.");
    }

    [Fact]
    public void Validate_Should_Return_True_When_PhoneNumber_Is_Valid()
    {
        var command = new UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            Username = "andrei",
            PasswordHash = "$2y$10$js8Xhx39VfRPo/0QHmGtaO54UwyG96jQJS1SMwqynTgzAtJkwKUUK",
            Email = "andrei@gmail.com",
            PhoneNumber = "07182828",
            Role = 0
        };

        var result = ValidateCommand(command);
        Assert.True(result, "Expected validation to pass when PhoneNumber is valid.");
    }




    private bool ValidateCommand(UpdateUserCommand command)
    {
        var validationResult = _validator.Validate(command);
        return validationResult.IsValid;
    }
}
