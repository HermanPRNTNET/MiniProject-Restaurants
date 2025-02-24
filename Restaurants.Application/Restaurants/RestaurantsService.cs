using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantRepository restaurantRepository
    , ILogger<RestaurantsService> logger
    , IMapper mapper) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantRepository.GetAllAsync();

        //var restaurantsDto = restaurants.Select(RestaurantDto.FromEntity);
        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return restaurantsDto!;
    }

    public async Task<RestaurantDto?> GetRestaurantById(int id)
    {
        logger.LogInformation($"Getting all restaurants by id : {id}");
        var restaurant = await restaurantRepository.GetByIdAsync(id);

        //var restaurantDtos = RestaurantDto.FromEntity(restaurant);

        var restaurantsDto = mapper.Map<RestaurantDto>(restaurant);

        return restaurantsDto;
    }
}
