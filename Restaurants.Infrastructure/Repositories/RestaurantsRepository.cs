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
    internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
    {
        public async Task<int> Create(Restaurant entity)
        {
            dbContext.Restaurants.Add(entity);
            await dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(Restaurant entity)
        {
            dbContext.Restaurants.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants
                .Include(r=>r.Dishes).ToListAsync();
            return restaurants;
        }

        public async Task<(IEnumerable<Restaurant>,int)> GetAllSearchAsync(string? searchParam,int pageSize,int pageNumber)
        {
            var searchPhraseLower = searchParam?.ToLower()?? string.Empty;

            var baseQuery = dbContext.Restaurants
                .Where(r => r.Name.ToLower().Contains(searchPhraseLower) ||
                            r.Description.ToLower().Contains(searchPhraseLower));

            var totalCount = await baseQuery.CountAsync();

            //var restaurants = await dbContext.Restaurants
            //    .Where(r=> searchPhraseLower == null || (r.Name.ToLower().Contains(searchPhraseLower) ||
            //                r.Description.ToLower().Contains(searchPhraseLower))).ToListAsync();

            var restaurants = await baseQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            //.Include(r => r.Dishes).ToListAsync();
            return (restaurants,totalCount);
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurantById = await dbContext.Restaurants
                .Include(r=>r.Dishes)
                .FirstOrDefaultAsync(x => x.Id == id);
            return restaurantById;
        }

        public Task SaveChanges()
            => dbContext.SaveChangesAsync();
        
    }
}
