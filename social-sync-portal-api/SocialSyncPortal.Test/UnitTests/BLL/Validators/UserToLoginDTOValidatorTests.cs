﻿using SocialSyncPortal.BLL.Validators;
using SocialSyncPortal.DTO.DTOs.User;
using FluentValidation.TestHelper;

namespace SocialSyncPortal.Test.UnitTests.BLL.Validators;

public class UserToLoginDTOValidatorTests
{
    private readonly UserToLoginDTOValidator _userToLoginDTOValidator;

    public UserToLoginDTOValidatorTests()
    {
        _userToLoginDTOValidator = new UserToLoginDTOValidator();
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("username", "")]
    [InlineData("", "password")]
    public void Validate_WhenUsernameOrPasswordOrBothEmpty_ThrowsValidationError(string username, string password)
    {
        var userToLoginDTO = new UserToLoginDTO
        {
            Username = username,
            Password = password
        };

        var result = _userToLoginDTOValidator.TestValidate(userToLoginDTO);
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Validate_WhenUsernameAndPasswordNotEmpty_ShouldNotThrowValidationError()
    {
        var userToLoginDTO = new UserToLoginDTO
        {
            Username = "username",
            Password = "password"
        };

        var result = _userToLoginDTOValidator.TestValidate(userToLoginDTO);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
