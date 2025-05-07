using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<(IEnumerable<Restaurant>,int)> GetAllSearchAsync(string? searchParam, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
        {
            var searchPhraseLower = searchParam?.ToLower()?? string.Empty;

            var baseQuery = dbContext.Restaurants
                .Where(r => r.Name.ToLower().Contains(searchPhraseLower) ||
                            r.Description.ToLower().Contains(searchPhraseLower));

            var totalCount = await baseQuery.CountAsync();
            if (sortBy != null)
            {
                var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), r => r.Name },
                    { nameof(Restaurant.Description), r => r.Description },
                    { nameof(Restaurant.Category), r => r.Category }
                };

                var selectedCollumn = columnSelector[sortBy];

                baseQuery = sortDirection == SortDirection.Ascending 
                        ? baseQuery.OrderBy(selectedCollumn)
                        : baseQuery.OrderByDescending(selectedCollumn);
            }


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
