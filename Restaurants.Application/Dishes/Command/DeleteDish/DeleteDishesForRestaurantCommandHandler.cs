﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Command.DeleteDish;

public class DeleteDishesForRestaurantCommandHandler(ILogger<DeleteDishesForRestaurantCommandHandler> logger
    ,IRestaurantsRepository restaurantsRepository
    ,IDishesRepository dishesRepository) : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting all dish for restaurant with id : {RestaurantId}", request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        await dishesRepository.DeleteAll(restaurant.Dishes);
    }
}
