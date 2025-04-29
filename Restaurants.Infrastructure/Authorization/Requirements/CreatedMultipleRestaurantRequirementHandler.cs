using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;


namespace Restaurants.Infrastructure.Authorization.Requirements
{
    internal class CreatedMultipleRestaurantRequirementHandler(IRestaurantsRepository restaurantsRepository,IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantRequirement requirement)
        {
            var currentUser = userContext.GetCurrentUser();

            var restaurantCount = await restaurantsRepository.GetAllAsync();

            var userRestaurantCreated = restaurantCount.Count(x => x.OwnerId == currentUser.Id);

            if (userRestaurantCreated >= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }   
}
