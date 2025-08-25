

using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;
using System.Linq;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurants
{
    public class CreateRestaurantsCommandValidators : AbstractValidator<CreateRestaurantsCommand>
    {
        private readonly List<string> valdCategory = ["Italian", "Mexican", "Japanese", "American", "Indian"];
        public CreateRestaurantsCommandValidators()
        {
            RuleFor(x => x.Name)
                    .Length(3, 100);
            RuleFor(x => x.Description)
                 .NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Category)
                .Must(valdCategory.Contains)
                .WithMessage("Invalid Category.Please choose from the valid categories.");
            //.Custom((value, context) => {
            //    var isValid = valdCategory.Contains(value);
            //    if(!isValid)
            //    {
            //        context.AddFailure("Category", "Invalid Category.Please choose from the valid categories.");
            //    }
            //});
            RuleFor(x => x.ContactEmail)
                .EmailAddress().WithMessage("Please provide valid email address.");
            RuleFor(x => x.PostalCode)
                .Matches(@"^\d{2}-\d{3}$").WithMessage("Please provide valid postal code (xx-xxx)");


        }
    }
}
