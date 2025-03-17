﻿using AutoMapper;
using Restaurants.Application.Dishes.Command.CreateDish;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.DTOs;

public class DishesProfile : Profile
{
    public DishesProfile() {
        CreateMap<CreateDishCommand, Dish>();
        CreateMap<Dish, DishDto>();
    }
}
