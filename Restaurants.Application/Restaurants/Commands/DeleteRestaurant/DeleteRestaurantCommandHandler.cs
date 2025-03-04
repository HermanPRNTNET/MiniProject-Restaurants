using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantCommand,bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id : {RestaurantId}", request.Id);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurant == null)
        {
            logger.LogWarning($"Restaurant with id {request.Id} not found");
            return false;
        } 

        await restaurantsRepository.Delete(restaurant);
        return true;

    }
}
