using Restaurants.Application.Dishes.DTOs;
using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.DTOs;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public bool HasDelivery { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }

    //public Address? Address { get; set; }
    public List<DishDto> Dishes { get; set; } = [];


    public static RestaurantDto? FromEntity(Restaurant? restaurant)
    {
        if (restaurant == null)
        {
            return null;
        }

        return new RestaurantDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Description = restaurant.Description,
            HasDelivery = restaurant.HasDelivery,
            Category = restaurant.Category,
            City = restaurant.Address?.City,
            Street = restaurant.Address?.Street,
            PostalCode = restaurant.Address?.PostalCode,
            Dishes = restaurant.Dishes.Select(DishDto.FromEntity).ToList()
        };

    }
}
