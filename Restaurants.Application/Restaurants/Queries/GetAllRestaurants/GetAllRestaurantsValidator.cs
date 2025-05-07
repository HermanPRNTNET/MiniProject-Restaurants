using FluentValidation;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsValidator : AbstractValidator<GetAllRestaurantsQuery>
{

    private int[] allowedPageSizes = [5, 10, 20, 50, 100];
    private string[] allowedSortBy = [nameof(RestaurantDto.Name), nameof(RestaurantDto.Category), nameof(RestaurantDto.Description)];
    public GetAllRestaurantsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0");

        RuleFor(x => x.PageSize)
            .Must(value => allowedPageSizes.Contains(value))
            .WithMessage($"Page size must be one of the following values: [{string.Join(", ", allowedPageSizes)}]");

        RuleFor(x => x.SortBy)
            .Must(value => allowedSortBy.Contains(value))
            .When(q => q.SortBy != null)
            .WithMessage($"Sort by is optional,or must be in : [{string.Join(", ", allowedSortBy)}]");
    }
}
