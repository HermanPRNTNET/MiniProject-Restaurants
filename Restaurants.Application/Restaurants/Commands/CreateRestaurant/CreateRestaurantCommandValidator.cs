using FluentValidation;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string>? validCategories = ["Fast Food", "Traditional", "Vegetarian", "Vegan", "Asian", "Italian", "Mexican", "American"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required");
        RuleFor(x => x.Category)
            .Must(validCategories.Contains).WithMessage("Invalid Category");
        //.Custom((value, context) =>
        //{
        //    if (!validCategories.Contains(value))
        //    {
        //        context.AddFailure("Invalid Category");
        //    }
        //});
        RuleFor(x => x.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address");
        RuleFor(x => x.PostalCode)
            .Matches(@"^\d{2}(-\d{3})?$")
            .WithMessage("Invalid Postal Code, must be in format (XX-XXX)");
    }
}
