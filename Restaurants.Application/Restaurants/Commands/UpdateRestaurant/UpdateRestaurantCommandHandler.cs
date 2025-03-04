using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper) : IRequestHandler<UpdateRestaurantCommand, bool>
    {
        public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting restaurant with id : {RestaurantId} with {@UpdatedRestaurant}", request.Id,request);
            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
            if (restaurant == null)
            {
                logger.LogWarning($"Restaurant with id {request.Id} not found");
                return false;
            }

            mapper.Map(request, restaurant);

            await restaurantsRepository.SaveChanges();
            return true;
        }
    }
}
