using Xunit;
using Restaurants.Application.Users;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using FluentAssertions;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class CreatedMultipleRestaurantRequirementHandlerTests
{
    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        //arrange

        var currentUser = new CurrentUser("1", "test@mailc.com", [],"Indonesian",null);
        var userContextMoq = new Mock<IUserContext>();
        userContextMoq.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = "2",
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantRequirement(2);
        var handler = new CreatedMultipleRestaurantRequirementHandler(restaurantsRepositoryMock.Object, userContextMoq.Object);

        var context = new AuthorizationHandlerContext([requirement],null,null);

        //act
        await handler.HandleAsync(context); 


        //asert
        context.HasSucceeded.Should().BeTrue();


    }

    [Fact()]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        //arrange

        var currentUser = new CurrentUser("1", "test@mailc.com", [], "Indonesian", null);
        var userContextMoq = new Mock<IUserContext>();
        userContextMoq.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = currentUser.Id,
            },
            new()
            {
                OwnerId = "2",
            }
        };

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantRequirement(2);
        var handler = new CreatedMultipleRestaurantRequirementHandler(restaurantsRepositoryMock.Object, userContextMoq.Object);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        //act
        await handler.HandleAsync(context);


        //asert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();


    }
}