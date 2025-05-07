using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
    {
        public string? SearchParam { get; set; }
        public int PageNumber{ get; set; }
        public int PageSize{ get; set; }

    }
}
