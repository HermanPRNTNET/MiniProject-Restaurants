using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;

namespace Restaurants.Infrastructure.Authorization.Requirements
{
    internal class CreatedMultipleRestaurantRequirement(int minimumRestaurantCreated) : IAuthorizationRequirement
    {
        public int MinimumRestaurantCreated { get; } = minimumRestaurantCreated;
    }
}
