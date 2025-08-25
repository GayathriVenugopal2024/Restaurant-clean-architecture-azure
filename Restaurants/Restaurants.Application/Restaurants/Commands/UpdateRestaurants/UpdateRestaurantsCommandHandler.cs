using AutoMapper;
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

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurants
{
    public class UpdateRestaurantsCommandHandler (ILogger<UpdateRestaurantsCommandHandler> logger,IRestaurantsRepository restaurantsRepository,IMapper mapper) : IRequestHandler<UpdateRestaurantsCommand>
    {
        public async Task  Handle(UpdateRestaurantsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating Restaurant {request.Id} with {@updaterestaurant}",request.Id,request );
            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
            if (restaurant == null) { throw new NotFoundException(nameof(Restaurant), request.Id.ToString()); }

            mapper.Map(request, restaurant);
            await restaurantsRepository.SaveChanges();
             
        }
    }
}
