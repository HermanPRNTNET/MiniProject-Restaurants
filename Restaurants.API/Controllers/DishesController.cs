using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Command.CreateDish;
using Restaurants.Application.Dishes.Command.DeleteDish;
using Restaurants.Application.Dishes.DTOs;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants/{restaurantId}/dishes")]
    [ApiController]
    public class DishesController(IMediator mediator) : ControllerBase  
    {
        [HttpPost]
        public async Task<IActionResult> CreateDish([FromRoute] int restaurantId,CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;

            var dishId = await mediator.Send(command);
            return CreatedAtAction(nameof(GetByIdForRestaurant),new {restaurantId,dishId },null);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurant([FromRoute] int restaurantId)
        {
            var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
            return Ok(dishes);
        }

        [HttpGet("{dishId}")]
        public async Task<ActionResult<IEnumerable<DishDto>>> GetByIdForRestaurant([FromRoute] int restaurantId,[FromRoute] int dishId )
        {
            var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId,dishId));
            return Ok(dish);
        }

        //[HttpDelete("{dishId}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DeleteDishForRestaurantById([FromRoute] int restaurantId, [FromRoute] int dishId)
        //{
        //    await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId, dishId));

        //    return NotFound("Data Not Found");
        //}

        [HttpDelete("{dishId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDishForRestaurant([FromRoute] int restaurantId)
        {
            await mediator.Send(new DeleteDishesForRestaurantCommand(restaurantId));

            return NoContent();
        }
    }
}
