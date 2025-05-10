using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _commandHandler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        _commandHandler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _restaurantRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object
        );
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
    {
        //arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "Updated Restaurant",
            Description = "Updated Description",
            HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Old Restaurant",
            Description = "Old Description"
            //HasDelivery = false
        };

        _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(restaurant, Domain.Constants.ResourceOperation.Update)).Returns(true);

        //_mapperMock.Setup(m => m.Map(command, restaurant))
        //    .Callback<UpdateRestaurantCommand, Restaurant>((src, dest) =>
        //    {
        //        dest.Name = src.Name;
        //        dest.Description = src.Description;
        //        dest.HasDelivery = src.HasDelivery;
        //});


        //act
        await _commandHandler.Handle(command, CancellationToken.None);



        //assert
        _restaurantRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);

    }

    [Fact()]
    public async Task Handle_WitNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        //arrange
        var restaurantId = 1;

        var request = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant)null);

        //act
        Func<Task> act = async () => await _commandHandler.Handle(request, CancellationToken.None);


        //assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id : {request.Id} not exist");


    }

    [Fact()]
    public async Task Handle_WitNotCorrectUser_ShouldThrowForbidException()
    {
        var restaurantId = 1;

        var request = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        var existingRestaurantId = new Restaurant()
        {
            Id = restaurantId
        };

        _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
            .ReturnsAsync(existingRestaurantId);

        _restaurantAuthorizationServiceMock.Setup(m => m.Authorize(existingRestaurantId, Domain.Constants.ResourceOperation.Update)).Returns(false);

        //act
        Func<Task> act = async () => await _commandHandler.Handle(request, CancellationToken.None);

        //assert
        await act.Should().ThrowAsync<ForbidenException>();
    }

}   