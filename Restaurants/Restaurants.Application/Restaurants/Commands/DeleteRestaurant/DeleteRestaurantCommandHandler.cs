using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,IRestaurantsRepository restaurantsRepository) 
        : IRequestHandler<DeleteRestaurantCommand>
    {
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting Restaurant {request.Id}",request.Id);
            var restaurant=await restaurantsRepository.GetByIdAsync(request.Id);
            if(restaurant == null) { throw new NotFoundException(nameof(Restaurant), request.Id.ToString() ); }
            await restaurantsRepository.Delete(restaurant);
             
        }
    }
}
