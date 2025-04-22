using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization.Services
{
    public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
        IUserContext userContext) : IRestaurantAuthorizationService
    {
        public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
        {
            var user = userContext.GetCurrentUser();
            logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for restaurant {RestaurantName}",
                user.Email,
                resourceOperation,
                restaurant.Name);

            if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("User {UserEmail} is allowed to {Operation} restaurant {RestaurantName}",
                    user.Email,
                    resourceOperation,
                    restaurant.Name);
                return true;
            }
            if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
            {
                logger.LogInformation("User {UserEmail} is an admin, allowing delete operation",
                    user.Email);
                return true;
            }

            if (resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update
                && user.Id == restaurant.OwnerId)
            {
                logger.LogInformation("User {UserEmail} is the owner of restaurant {RestaurantName}, allowing {Operation}",
                    user.Email,
                    restaurant.Name,
                    resourceOperation);
                return true;
            }


            return false;
        }
    }
}
