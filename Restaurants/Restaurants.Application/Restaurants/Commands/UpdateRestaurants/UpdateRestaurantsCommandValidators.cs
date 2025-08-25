using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurants
{
    public class UpdateRestaurantsCommandValidators:AbstractValidator<UpdateRestaurantsCommand>
    {
        public UpdateRestaurantsCommandValidators()
        {
            RuleFor(c => c.Name)
                .Length(3, 100);
                
        }
    }
}
