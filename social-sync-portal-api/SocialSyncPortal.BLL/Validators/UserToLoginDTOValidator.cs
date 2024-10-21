using SocialSyncPortal.DTO.DTOs.User;
using FluentValidation;

namespace SocialSyncPortal.BLL.Validators;

public class UserToLoginDTOValidator : AbstractValidator<UserToLoginDTO>
{
    public UserToLoginDTOValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
