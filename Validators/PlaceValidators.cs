using FluentValidation;
using PoliRiwi.Models;

namespace PoliRiwi.Validators;

public class PlaceValidators : AbstractValidator<Places>
{
    public PlaceValidators()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Place name cannot be null or empty");
        RuleFor(p => p.Status).IsInEnum().WithMessage("Place status cannot be null or empty");
        RuleFor(p=> p.SpaceType).IsInEnum().WithMessage("Place space type cannot be null or empty");
    }
}