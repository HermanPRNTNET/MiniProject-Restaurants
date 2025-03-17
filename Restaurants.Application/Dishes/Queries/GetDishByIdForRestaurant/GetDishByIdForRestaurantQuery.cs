using MediatR;
using Restaurants.Application.Dishes.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantQuery(int restaurantId, int dishId) : IRequest<DishDto>
{
    public int DishId { get; } = dishId;
    public int RestaurantId { get; } = restaurantId;
}
