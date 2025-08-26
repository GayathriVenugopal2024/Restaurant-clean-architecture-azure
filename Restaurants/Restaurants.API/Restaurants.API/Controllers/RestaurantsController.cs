using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurants;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurants;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]

public class RestaurantsController(IMediator mediator) :ControllerBase
{
    [HttpGet]
    [Authorize(Roles="Reader") ]
    [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(IEnumerable<RestaurantDto>))]
    public async Task< IActionResult> GetAll()
    {
        var response = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(response);
    }
    [HttpGet]
    [Route("/{id}")]
    [Authorize(Roles = "Writer")]
    //[Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RestaurantDto>> GetById([FromRoute]int id)
    {
        var response = await mediator.Send(new GetRestaurantByIdQuery(id));
        if(response == null)
        {
            return NotFound();
        }
        return Ok(response);
    }
    [HttpDelete]
    [Route("/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        await mediator.Send(new DeleteRestaurantCommand (id));
        return NoContent();
        
    }
    [HttpPatch]
    [Route("/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantsCommand command,IValidator<UpdateRestaurantsCommand> validator)
    {
        command.Id = id;
        var result = await validator.ValidateAsync(command);
        if (!result.IsValid)
            return BadRequest(result.Errors);
         await mediator.Send(command);
            return NoContent();
        
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRestaurants([FromBody] CreateRestaurantsCommand command, IValidator<CreateRestaurantsCommand> validator)
    {
        var result = await validator.ValidateAsync(command);

        if (!result.IsValid)
            return BadRequest(result.Errors);
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById),new { id },null);
    }
}
