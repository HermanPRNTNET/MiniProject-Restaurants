using FluentValidation;

namespace Restaurants.Application.Dishes.Command.CreateDish;

internal class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative number");
        RuleFor(x => x.KiloCalories).GreaterThanOrEqualTo(0).WithMessage("Calories must be non-negative number");
    }
}
