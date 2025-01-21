using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.User).NotNull().WithMessage("User cannot be null");
        RuleFor(x => x.User.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email cannot be empty");
        RuleFor(x => x.User.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty");
    }
}