using FluentValidation;
using PoliRiwi.Models;

namespace PoliRiwi.Validators;

public class ReservationValidatiors : AbstractValidator<Reservations>
{
    public ReservationValidatiors()
    {
        RuleFor(p => p.Date).GreaterThan(DateTime.Today).WithMessage("Date cannot be in the future");
        RuleFor(p => p.EndTime).GreaterThan(p => p.StartTime).WithMessage("Start time cannot be in the future");
        RuleFor(p => p.Date).LessThan(p => DateTime.Today).WithMessage("Can not do reservations for yesterday");
        RuleFor(p => p.Status).IsInEnum().NotEmpty().WithMessage("Status cannot be null or empty");
    }
}