﻿using SocialSyncPortal.BLL.Validators;
using SocialSyncPortal.DTO.DTOs.User;
using FluentValidation.TestHelper;
namespace SocialSyncPortal.Test.UnitTests.BLL.Validators;

public class UserToRegisterDTOValidatorTests
{
    private readonly UserToRegisterDTOValidator _userToRegisterDTOValidator;

    public UserToRegisterDTOValidatorTests()
    {
        _userToRegisterDTOValidator = new UserToRegisterDTOValidator();
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("username", "")]
    [InlineData("", "password")]
    public void Validate_WhenUsernameOrPasswordOrBothEmpty_ThrowsValidationError(string username, string password)
    {
        var userToRegisterDTO = new UserToRegisterDTO
        {
            Username = username,
            Password = password,
            Name = "",
            Surname = ""
        };

        var result = _userToRegisterDTOValidator.TestValidate(userToRegisterDTO);
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_WhenUsernameAndPasswordNotEmpty_ShouldNotThrowValidationError()
    {
        var userToRegisterDTO = new UserToRegisterDTO
        {
            Username = "username",
            Password = "password",
            Name = "",
            Surname = ""
        };

        var result = _userToRegisterDTOValidator.TestValidate(userToRegisterDTO);
        result.ShouldNotHaveAnyValidationErrors();
    }
}

