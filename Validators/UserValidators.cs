using FluentValidation;
using PoliRiwi.Models;

namespace PoliRiwi.Validators;

public class UserValidators : AbstractValidator<Users>
{
    public UserValidators()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name cannot be null or empty");
        RuleFor(p => p.Email).NotEmpty().WithMessage("Email cannot be null or empty");
        RuleFor(p => p.Phone).NotEmpty().WithMessage("Phone cannot be null or empty");
    }
}