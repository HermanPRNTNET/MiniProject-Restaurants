using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(ILogger<GetRestaurantByIdQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantRepository) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
    {
        public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all restaurants");
            var restaurants = await restaurantRepository.GetAllAsync();

            //var restaurantsDto = restaurants.Select(RestaurantDto.FromEntity);
            var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            return restaurantsDto!;
        }
    }
}
