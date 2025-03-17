using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories
{
    internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
    {
        public async Task<int> Create(Dish entity)
        {
            dbContext.Dishes.Add(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }
        public async Task DeleteAll(IEnumerable<Dish> entity)
        {
            dbContext.Dishes.RemoveRange(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(Dish entity)
        {
            dbContext.Dishes.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
