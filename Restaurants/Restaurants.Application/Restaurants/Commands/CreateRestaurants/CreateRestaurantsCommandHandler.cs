using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurants
{
    public class CreateRestaurantsCommandHandler(ILogger<CreateRestaurantsCommandHandler> logger,IMapper mapper,IRestaurantsRepository restaurantsRepository) : IRequestHandler<CreateRestaurantsCommand, int>
    {
        public async Task<int> Handle(CreateRestaurantsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating ne restaurant");
            var restaurant = mapper.Map<Restaurant>(request);
            int id = await restaurantsRepository.CreateAsync(restaurant);
            return id;
        }
    }
}
